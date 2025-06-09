using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualQue;

    [Header("Ink JSON")]
    [SerializeField] public TextAsset inkJSON;

    [Header("Issues repair")]
    [SerializeField] public bool firstContinue;
    [SerializeField] public bool waitInteraction = true;
    [SerializeField] public string colliderTag = "PlayerCollider";

    private bool playerInRange;


    private void Update()
    {
        if(playerInRange)
        {
            visualQue.SetActive(true);

            if(waitInteraction && PlayerController.instance.GetInterectPressed() && !DialogueManager.instance.dialogueIsPlaying)
            {
                EnterDialogueMode();
            }
        }       
    }

    private void EnterDialogueMode()
    {
        DialogueManager.instance.EnterDialogueMode(inkJSON);

        if(firstContinue)
        {
            DialogueManager.instance.ContinueStory();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == colliderTag)
        {
            if(waitInteraction)
            {
                playerInRange = true;
            }
            else
            {
                EnterDialogueMode();
            }   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == colliderTag)
        {
            if(waitInteraction)
            {
                playerInRange = false;
            }
        }
    }
}
