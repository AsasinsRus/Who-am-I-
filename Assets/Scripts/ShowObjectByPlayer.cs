using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectByPlayer : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private bool showWhenDialogue;
    [SerializeField] public bool show = true;

    private bool isInTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        obj.SetActive(false);
    }

    private void Update()
    {
        if (!showWhenDialogue && isInTrigger && show)
        {
            if (DialogueManager.instance.dialogueIsPlaying)
            {
                if (obj.active)
                    obj.SetActive(false);
            }
            else
            {
                if (!obj.active)
                    obj.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerCollider")
        {
            isInTrigger = true;
            obj.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            isInTrigger = false;
            obj.SetActive(false);
        }
    }
}
