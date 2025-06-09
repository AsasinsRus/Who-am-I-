using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    public string SceneName;
    public GameObject Fader;
    private bool isEnter = false;
    public bool isActive = true;

    private void Update()
    {
        if(isActive && isEnter && PlayerController.instance.GetInterectPressed())
        {
            GetComponent<ShowObjectByPlayer>().show = false;
            Fader.GetComponent<Animator>().SetBool("faded", true);

            LevelDataHolder.doorName = gameObject.name;
            Debug.Log(gameObject.name);
            Fader.GetComponent<SceneTransition>().SceneName = SceneName;

            isEnter = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        isEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }
}
