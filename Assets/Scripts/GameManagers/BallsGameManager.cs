using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BallsGameManager : MonoBehaviour
{
    [Header("ObjectsToControl")]
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private GameObject door;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject aquarium;
    [SerializeField] private GameObject balls;
    [SerializeField] private GameObject book;

    [Header("Ink")]
    [SerializeField] private TextAsset beginningDialogue;

    private bool allowCenterTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        door.GetComponentInChildren<BoxCollider2D>().enabled = false;
        foreach (var bookCollider in book.GetComponentsInChildren<BoxCollider2D>())
            bookCollider.enabled = false;

        StartCoroutine(Beginning());
    }

    // Update is called once per frame
    void Update()
    {
        if(!allowCenterTrigger
            && ((Ink.Runtime.BoolValue)DialogueManager.instance.GetVariableState("wasWindowInteracted")).value
            && ((Ink.Runtime.BoolValue)DialogueManager.instance.GetVariableState("wasAquariumInteracted")).value)
        {
            StartCoroutine(allowCenterAlg());
            allowCenterTrigger = true;
        }

        if(LevelDataHolder.storyAct != "act 5"
            && ((Ink.Runtime.BoolValue)DialogueManager.instance.GetVariableState("wasBookInteracted")).value)
        {
            DoorActivation();
        }

    }

    private void DoorActivation()
    {
        LevelDataHolder.storyAct = "act 5";

        StartCoroutine(Ending());
        
    }

    private IEnumerator Ending()
    {
        PlayerController.instance.canMove = false;

        cameraAnimator.Play("door");

        yield return new WaitForSeconds(1.05f);

        door.GetComponent<Animator>().Play("activation");

        yield return new WaitForSeconds(1);

        door.GetComponent<DoorManager>().isActive = true;
        door.GetComponentInChildren<BoxCollider2D>().enabled = true;

        PlayerController.instance.canMove = true;
    }

    private IEnumerator Beginning()
    {
        PlayerController.instance.canMove = false;
        toOutAnim();

        yield return new WaitForSeconds(0.8f);
        faderAnimator.Play("Out");

        yield return new WaitForSeconds(0.5f);
        door.GetComponent<Animator>().Play("inactivation");
        door.GetComponentInChildren<ShowObjectByPlayer>().enabled = false;
        door.GetComponent<DoorManager>().isActive = false;

        yield return new WaitForSeconds(1);

        var windowAnimators = window.GetComponentsInChildren<Animator>();
        windowAnimators[1].Play("fadeIn");
        yield return new WaitForSeconds(0.2f);
        windowAnimators[0].Play("fadeIn");

        yield return new WaitForSeconds(0.5f);
        var aquariumAnimators = aquarium.GetComponentsInChildren<Animator>();
        aquariumAnimators[1].Play("fadeIn");
        yield return new WaitForSeconds(0.2f);
        aquariumAnimators[0].Play("fadeIn");

        yield return new WaitForSeconds(0.4f);

        DialogueManager.instance.EnterDialogueMode(beginningDialogue);
        DialogueManager.instance.ContinueStory();

        PlayerController.instance.canMove = true;
    }

    private void toOutAnim()
    {
        foreach(Animator windowAnimator in window.GetComponentsInChildren<Animator>())
        {
            windowAnimator.Play("out");
        }

        foreach (Animator aquariumAnimator in aquarium.GetComponentsInChildren<Animator>())
        {
            aquariumAnimator.Play("out");
        }

        foreach (Animator ballAnimator in balls.GetComponentsInChildren<Animator>())
        {
            ballAnimator.Play("out");
        }

        book.GetComponent<Animator>().Play("out");
    }

    private IEnumerator allowCenterAlg()
    {
        PlayerController.instance.canMove = false;
        var ballAnimators = balls.GetComponentsInChildren<Animator>();
        var ballAnimManagers = balls.GetComponentsInChildren<BallsAnimManager>();

        for (int i = 0; i < ballAnimators.Length; i++)
        {
            if (!ballAnimManagers[i].wasReminded)
            {
                ballAnimators[i].Play("fadeIn");
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.7f);
        cameraAnimator.Play("onBook");

        yield return new WaitForSeconds(1);
        book.GetComponent<Animator>().Play("fadeIn");

        yield return new WaitForSeconds(1);
        cameraAnimator.Play("fromBook");

        yield return new WaitForSeconds(0.5f);
        foreach (var bookCollider in book.GetComponentsInChildren<BoxCollider2D>())
            bookCollider.enabled = true;
        PlayerController.instance.canMove = true;
    }
}
