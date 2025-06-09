using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasInteracted : MonoBehaviour
{
    public bool wasInteracted = false;

    private bool playerInRange;

    private void Update()
    {
        if(playerInRange && PlayerController.instance.GetInterectPressed())
        {
            wasInteracted = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            playerInRange = false;
        }
    }
}
