using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RoomGameManager : MonoBehaviour
{
    [Header("ObjectsToControl")]    
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private GameObject classDoor;
    [SerializeField] private GameObject ballsDoor;
    [SerializeField] private GameObject clockTrigger;
    [SerializeField] private GameObject mirrorTrigger;
    [SerializeField] private GameObject menu;

    [Header("Credits")]
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject continueButton;

    [Header("Ink JSON act 1")]
    [SerializeField] private TextAsset beginningDialogue;
    [SerializeField] private TextAsset introductionDialogue;

    [Header("Ink JSON act 3")]
    [SerializeField] private TextAsset beginningDialogue3;
    [SerializeField] private TextAsset mirrorDialogue3;
    [SerializeField] private TextAsset clockDialogue3;

    private bool wasClassDoorAppeared = false;
    private bool wasBallsDoorAppeared = false;

    private Animator[] creditsAnimators;

    public static RoomGameManager instance { get; private set; }

    private void Awake()
    {
        instance = this;

        if (LevelDataHolder.storyAct != "act 1")
        {
            classDoor.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            classDoor.SetActive(true);
        }
    }
    void Start()
    {
        clockTrigger.GetComponent<BoxCollider2D>().enabled = false;

        switch (LevelDataHolder.storyAct)
        {
            case "act 1":
                act1Start();
                break;
            case "act 3":
                act3Start();
                break;
            case "act 5":
                CreditsStart();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mirrorTrigger.GetComponent<WasInteracted>().wasInteracted)
        {
            clockTrigger.GetComponent<BoxCollider2D>().enabled = true;
            mirrorTrigger.GetComponent<WasInteracted>().wasInteracted = false;
        }

        switch (LevelDataHolder.storyAct)
        {
            case "act 1":
                act1Update();
                break;
            case "act 3":
                act3Update();
                break;
        }
    }

    #region act1
    private void act1Start()
    {
        StartCoroutine(Beginning());
    }

    private void act1Update()
    {
        if(clockTrigger.GetComponent<WasInteracted>().wasInteracted)
        {
            DialogueManager.instance.BindExternalFunction("classDoorAppearance", classDoorAppearance);
            clockTrigger.GetComponent<WasInteracted>().wasInteracted = false;
        }

        if(classDoor.GetComponent<WasInteracted>().wasInteracted)
        {
            LevelDataHolder.storyAct = "act 2";
            classDoor.GetComponent<WasInteracted>().wasInteracted  = false;
        }    
    }

    private IEnumerator Beginning()
    {
        yield return new WaitForSeconds(0.5f);
        DialogueManager.instance.EnterDialogueMode(beginningDialogue);
        DialogueManager.instance.BindExternalFunction("FadeOut", FadeOut);
        DialogueManager.instance.ContinueStory();
    }

    private string FadeOut()
    {
        faderAnimator.Play("Out");
        StartCoroutine(IntroductionStart());
        return "";
    }

    private IEnumerator IntroductionStart()
    {
        float turningSpeed = 0.4f;

        yield return new WaitForEndOfFrame();
        PlayerController.instance.canMove = false;

        yield return new WaitForSeconds(turningSpeed * 2);
        PlayerController.instance.animator.Play("Idle_Left");

        yield return new WaitForSeconds(turningSpeed);
        PlayerController.instance.animator.Play("Idle_Backward");

        yield return new WaitForSeconds(turningSpeed);
        PlayerController.instance.animator.Play("Idle_Right");

        yield return new WaitForSeconds(turningSpeed);
        PlayerController.instance.animator.Play("Idle_Forward");

        yield return new WaitForSeconds(0.5f);
        DialogueManager.instance.EnterDialogueMode(introductionDialogue);
        DialogueManager.instance.ContinueStory();

        PlayerController.instance.canMove = true;
    }

    private string classDoorAppearance()
    {
        if(!wasClassDoorAppeared)
            StartCoroutine(classDoorAppearanceAnim());

        return "";
    }

    private IEnumerator classDoorAppearanceAnim()
    {
        PlayerController.instance.canMove = false;
        clockTrigger.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1);

        PlayerController.instance.animator.Play("Idle_Left");
        cameraAnimator.Play("CameraOnClassDoor");

        yield return new WaitForSeconds(2);

        classDoor.SetActive(true);

        yield return new WaitForSeconds(2);

        cameraAnimator.Play("CameraFromClassDoor");

        yield return new WaitForSeconds(1);

        PlayerController.instance.canMove = true;
        wasClassDoorAppeared = true;

        clockTrigger.GetComponent<BoxCollider2D>().enabled = true;
    }

    #endregion

    #region act3
    private void act3Start()
    {
        mirrorTrigger.GetComponent<DialogueTrigger>().inkJSON = mirrorDialogue3;
        clockTrigger.GetComponent<DialogueTrigger>().inkJSON = clockDialogue3;

        StartCoroutine(act3Beginning());
    }

    private void act3Update()
    {
        if (clockTrigger.GetComponent<WasInteracted>().wasInteracted)
        {
            DialogueManager.instance.BindExternalFunction("ballsDoorAppearance", ballsDoorAppearance);
            clockTrigger.GetComponent<WasInteracted>().wasInteracted = false;
        }

        if (ballsDoor.GetComponent<WasInteracted>().wasInteracted)
        {
            LevelDataHolder.storyAct = "act 4";
            ballsDoor.GetComponent<WasInteracted>().wasInteracted = false;
        }
    }

    private IEnumerator act3Beginning()
    {
        PlayerController.instance.canMove = false;

        yield return new WaitForSeconds(0.8f);
        faderAnimator.Play("Out");

        yield return new WaitForSeconds(0.5f);
        PlayerController.instance.animator.Play("Idle_Backward");
        cameraAnimator.Play("CameraOnClassDoor");

        yield return new WaitForSeconds(1);
        classDoor.GetComponent<Animator>().Play("inactivation");
        classDoor.GetComponent<DoorManager>().isActive = false;

        yield return new WaitForSeconds(1);
        PlayerController.instance.animator.Play("Idle_Forward");
        cameraAnimator.Play("CameraFromClassDoor");

        yield return new WaitForSeconds(0.5f);
        DialogueManager.instance.EnterDialogueMode(beginningDialogue3);
        DialogueManager.instance.ContinueStory();
        PlayerController.instance.canMove = true;
    }

    private string ballsDoorAppearance()
    {
        if (!wasBallsDoorAppeared)
            StartCoroutine(ballsDoorAppearanceAnim());

        return "";
    }

    private IEnumerator ballsDoorAppearanceAnim()
    {
        PlayerController.instance.canMove = false;
        clockTrigger.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(1);

        PlayerController.instance.animator.Play("Idle_Left");
        cameraAnimator.Play("CameraOnBallsDoor");

        yield return new WaitForSeconds(2);

        ballsDoor.SetActive(true);

        yield return new WaitForSeconds(2);

        cameraAnimator.Play("CameraFromBallsDoor");

        yield return new WaitForSeconds(1);

        PlayerController.instance.canMove = true;
        wasBallsDoorAppeared = true;
        clockTrigger.GetComponent<BoxCollider2D>().enabled = true;
    }

    #endregion

    private void CreditsStart()
    {
        creditsAnimators = credits.GetComponentsInChildren<Animator>();
        PlayerController.instance.canMove = false;
        StartCoroutine(CreditsAnim());
    }

    private IEnumerator CreditsAnim()
    {
        menu.GetComponent<MenuManager>().SetActive(false);

        creditsAnimators[0].Play("goingDownTilOutOfScreen");
        yield return new WaitForSeconds(1);

        creditsAnimators[1].Play("goingDownTilOutOfScreen");
        yield return new WaitForSeconds(2);

        creditsAnimators[2].Play("goingDownTilOutOfScreen");
        yield return new WaitForSeconds(1.5f);

        creditsAnimators[3].Play("goingDownTilOutOfScreen");
        yield return new WaitForSeconds(4);

        creditsAnimators[4].Play("goingDownTilOutOfScreen");
        yield return new WaitForSeconds(8);

        menu.GetComponent<MenuManager>().SetActive(true);
        continueButton.SetActive(false);
        menu.GetComponent<Animator>().Play("goingDown");
    }
}
