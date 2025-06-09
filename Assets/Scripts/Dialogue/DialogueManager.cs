using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using System.Data;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject dialoguePanelSpeaker;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private GameObject pictureCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Animator pictureImgAnimator;
    [SerializeField] private Animator fadeAnimator;

    [Header("Choices")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    [SerializeField] private bool makePredictible;

    private DialogueAudioInfoSO currentAudioInfo;
    private AudioSource audioSource;
    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;

    private Animator layoutAnimator;
    private Animator pictureCanvasAnimator;   

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = true;
    private bool canBeFlipped = false;
    private bool isTyping = false;
    private bool canSkip = false;

    private bool isPictureOpened = false;

    public static string layout = "right";
    public static DialogueManager instance { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string PICTURE_TAG = "picture";
    private const string FADER_TAG = "fade";
    private const string AUDIO_TAG = "audio";

    private DialogueVariables dialogueVariables;

    public delegate void voidFunc();
    public List<string> ExternalFunctionNames;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }

        instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        ExternalFunctionNames = new List<string>();

        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = LevelDataHolder.audioVolume;
        currentAudioInfo = defaultAudioInfo;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        pictureCanvas.SetActive(false);
        dialoguePanelSpeaker.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        pictureCanvasAnimator = pictureCanvas.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        InitializeAudioInfoDictionary();
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);

        foreach(DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);

        if(audioInfo != null)
        {
            this.currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }
    }

    private void Update()
    {
        if (audioSource.volume != LevelDataHolder.audioVolume)
            audioSource.volume = LevelDataHolder.audioVolume;

        if (!dialogueIsPlaying)
        {
            return;
        }

        if (PlayerController.instance.GetInterectPressed() || Input.GetMouseButtonDown(0))
        {
            if(isTyping)
            {
                canSkip = true;
            }

            if(isPictureOpened)
            {
                StartCoroutine(ClosePicture());
                ContinueStory();
            }
            else if(canContinueToNextLine)
            {
                ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, null, ExecuteEvents.submitHandler);

                if (currentStory.currentChoices.Count == 0)
                    ContinueStory();
            }
        }
    }

    private IEnumerator ClosePicture()
    {
        pictureCanvasAnimator.Play("end");

        yield return new WaitForSeconds(0.2f);

        isPictureOpened = false;
        pictureCanvas.SetActive(false);
    }

    private IEnumerator OpenPicture()
    {
        yield return new WaitForSeconds(0.2f);

        pictureCanvasAnimator.Play("start");
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        dialogueText.text = "";
        displayNameText.text = "";
        portraitAnimator.Play("default");

        SetCurrentAudioInfo(defaultAudioInfo.id);

        StartCoroutine(StartDialogueMode());
    }

    private IEnumerator StartDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        switch(layout)
        {
            case "right":
                layoutAnimator.Play("showRight");
                break;
            case "rightPortrait":
                layoutAnimator.Play("showRightPortrait");
                break;
            case "left":
                layoutAnimator.Play("showLeft");
                break;
            case "leftPortrait":
                layoutAnimator.Play("showLeftPortrait");
                break;
            case "hidden":
                layoutAnimator.Play("hidden");
                fadeAnimator.Play("in");
                break;
        }

        yield return new WaitForSeconds(0.2f);

        canBeFlipped = true;
    }

    private IEnumerator ExitDialogueMode()
    {
        switch (layout)
        {
            case "right":
                layoutAnimator.Play("hideRight");
                break;
            case "showRight":
                layoutAnimator.Play("hideRight");
                break;
            case "rightPortrait":
                layoutAnimator.Play("hideRightPortrait");
                break;
            case "showRightPortrait":
                layoutAnimator.Play("hideRightPortrait");
                break;
            case "left":
                layoutAnimator.Play("hideLeft");
                break;
            case "showLeft":
                layoutAnimator.Play("hideLeft");
                break;
            case "leftPortrait":
                layoutAnimator.Play("hideLeftPortrait");
                break;
            case "showLeftPortrait":
                layoutAnimator.Play("hideLeftPortrait");
                break;
            default:
                fadeAnimator.Play("out");
                break;
        }

        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);

        foreach(string name in ExternalFunctionNames)
        {
            UnBindExternalFunction(name);
        }
        ExternalFunctionNames.Clear();

        dialogueIsPlaying = false;
        canBeFlipped = false;
        dialoguePanel.SetActive(false);

        dialoguePanelSpeaker.SetActive(false);
        dialogueText.text = "";
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            string nextLine = currentStory.Continue();
            HandleTags(currentStory.currentTags);

            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));

            
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
    private IEnumerator DisplayLine(string line)
    {
        if(line.Length > 0 && line[0] != ':')
        {       
            dialogueText.SetText(line);

            dialogueText.maxVisibleCharacters = 0;
        }

        if(line == "" && !currentStory.canContinue)
        {
            StartCoroutine(ExitDialogueMode());
            yield return null;
        }

        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        yield return new WaitForEndOfFrame();

        foreach (char letter in line.ToCharArray())
        {
            if (line.Length > 0 && line[0] == ':') //////////
                break;

            isTyping = true;

            if (canSkip)
            {

                dialogueText.maxVisibleCharacters = line.Length;
                
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;

                if(letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;

                yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
        isTyping = false;
        canSkip = false;
    }

    private void PlayDialogueSound(int currentDisplayedCharaktersCount, char currentCharakter)
    {
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;


        if (currentDisplayedCharaktersCount % frequencyLevel == 0)
        {
            AudioClip soundClip = null;

            if (stopAudioSource)
            {
                audioSource.Stop();
            }

            if(makePredictible)
            {
                int hashCode = currentCharakter.GetHashCode();
                int predictibleIndex = hashCode % dialogueTypingSoundClips.Length;

                soundClip = dialogueTypingSoundClips[predictibleIndex];

                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if(pitchRangeInt != 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;

                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, dialogueTypingSoundClips.Length);
                audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
                soundClip = dialogueTypingSoundClips[randomIndex];
            }

            
            audioSource.PlayOneShot(soundClip);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    
                    dialoguePanelSpeaker.SetActive(tagValue != "" && tagValue != "empty");

                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    if(canBeFlipped)
                        layoutAnimator.Play(tagValue);

                    layout = tagValue;
                    dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.preferredWidth, dialogueText.rectTransform.sizeDelta.y);
                    break;
                case PICTURE_TAG:
                    isPictureOpened = true;

                    pictureCanvas.SetActive(true);
                    pictureImgAnimator.Play(tagValue);


                    if (!isPictureOpened)
                    {
                        isPictureOpened = true;
                        StartCoroutine(OpenPicture());
                    }

                    break;
                case FADER_TAG:
                    fadeAnimator.Play(tagValue);
                    break;
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;

            }
        }
    }

    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;

        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i<choices.Length;i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        if (currentChoices.Count > 1)
        {
            PlayerController.changeCursorState(true);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
            currentStory.ChooseChoiceIndex(choiceIndex);

        PlayerController.changeCursorState(false);
    }

    public Ink.Runtime.Object GetVariableState(string name)
    {
        Ink.Runtime.Object state = null;

        if(!dialogueVariables.variables.TryGetValue(name, out state))
        {
            Debug.LogWarning("Ink Variable was found to be null: " + name);
        }

        return state;
    }

    public void BindExternalFunction(string functionName, Func<string> func)
    {
        if(currentStory == null)
        {
            Debug.LogError("null");
            return;
        }

        if(!ExternalFunctionNames.Contains(functionName))
        {
            currentStory.BindExternalFunction(functionName, func);
            ExternalFunctionNames.Add(functionName);
        }
    }

    public void UnBindExternalFunction(string functionName)
    {
        currentStory.UnbindExternalFunction(functionName);
    }
}
