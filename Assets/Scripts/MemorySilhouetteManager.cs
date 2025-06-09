using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySilhouetteManager : MonoBehaviour
{
    public bool wasReminded = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!wasReminded && collision.tag == "Player")
        {
            animator.Play("fadeOut");
            wasReminded = true;
        }
    }
}
