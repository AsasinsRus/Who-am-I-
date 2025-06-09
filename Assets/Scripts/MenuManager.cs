using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public bool isOpen { get; private set; }

    public void Start()
    {
        isOpen = true;
    }
    public void SetActive(bool active)
    {
        isOpen = active;
        gameObject.SetActive(active);
        PlayerController.changeCursorState(active);
    }

    public void AppExit()
    {
        Application.Quit();
    }

    public void AppRestart()
    {
        LevelDataHolder.storyAct = "act 1";
        LevelDataHolder.doorName = "default";

        SceneManager.LoadScene("Room");

        gameObject.SetActive(false);
    }
}
