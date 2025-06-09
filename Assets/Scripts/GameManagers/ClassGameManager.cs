using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClassGameManager : MonoBehaviour
{
    [Header("ObjectsToControl")]
    [SerializeField] private Animator faderAnimator;
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private GameObject door;
    [SerializeField] private GameObject doorOutline;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset beginningDialogue;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(Beginning());
    }


    private void Update()
    {
        if (door.GetComponentInChildren<WasInteracted>().wasInteracted)
        {
            DialogueManager.instance.BindExternalFunction("DoorActivation", DoorActivation);
            door.GetComponentInChildren<WasInteracted>().wasInteracted = false;
        }
    }
    private IEnumerator Beginning()
    {
        PlayerController.instance.canMove = false;

        yield return new WaitForSeconds(0.8f);
        faderAnimator.Play("Out");

        yield return new WaitForSeconds(0.7f);
        PlayerController.instance.animator.Play("Idle_Backward");

        yield return new WaitForSeconds(0.4f);
        cameraAnimator.Play("CameraOnClassDoorFromClass");

        yield return new WaitForSeconds(1.5f);
        door.GetComponent<Animator>().Play("inactivation");
        door.GetComponent<DoorManager>().isActive = false;

        yield return new WaitForSeconds(1.3f);
        cameraAnimator.Play("CameraFromClassDoorFromClass");

        yield return new WaitForSeconds(1);
        PlayerController.instance.animator.Play("Idle_Forward");

        yield return new WaitForSeconds(0.4f);
        DialogueManager.instance.EnterDialogueMode(beginningDialogue);
        DialogueManager.instance.ContinueStory();

        PlayerController.instance.canMove = true;
    }

    private IEnumerator End()
    {
        PlayerController.instance.canMove = false;

        yield return new WaitForSeconds(0.5f);
        cameraAnimator.Play("CameraOnClassDoorFromClass");

        yield return new WaitForSeconds(1.5f);
        door.GetComponent<Animator>().Play("activation");
        door.GetComponent<DoorManager>().isActive = true;

        yield return new WaitForSeconds(1.3f);
        cameraAnimator.Play("CameraFromClassDoorFromClass");

        yield return new WaitForSeconds(0.4f);
        doorOutline.GetComponent<SpriteRenderer>().enabled = true;
        PlayerController.instance.canMove = true;
    }
    private string DoorActivation()
    {
        doorOutline.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponentInChildren<DialogueTrigger>().enabled = false;
        LevelDataHolder.storyAct = "act 3";

        StartCoroutine(End());

        return "";
    }
    
    
    public void OnBackgroundNoise(bool on)
    {
        GetComponentInChildren<AudioSource>().mute = !on;
    }
}
