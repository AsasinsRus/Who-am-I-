using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody2D playerRB;
    private Vector2 direction;
    public Animator animator { get; private set; }

    [Header("Technical issues")]
    [SerializeField] private BoxCollider2D collider2D;
    [SerializeField] private MenuManager menu;
    [SerializeField] private String currentAct;

    private float colliderOffsetX = 0.335f;
    private float colliderOffsetY = 0.7f;

    public bool canMove { get; set; }
    public bool wasMenuOpened = false;

    [Header("Interact button")]
    [SerializeField] public KeyCode[] interactButtons;

    public static PlayerController instance { get; private set; }

    private void Awake()
    {
        canMove = true;
        instance = this;

        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        playerStartPosition();

        changeCursorState(false);
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector2.zero;
        currentAct = LevelDataHolder.storyAct;

        if (DialogueManager.instance.dialogueIsPlaying || !canMove)
        {
            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", 0);
            playerRB.velocity = Vector2.zero;

            return;
        }

        if(!wasMenuOpened)
        {
            if (Input.GetKey(KeyCode.D))
            {
                direction.x = 1;
                collider2D.offset = new Vector2(colliderOffsetX, -0.48f);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                direction.x = -1;
                collider2D.offset = new Vector2(-colliderOffsetX, -0.48f);
            }

            if (Input.GetKey(KeyCode.W))
            {
                direction.y = 1;
                collider2D.offset = new Vector2(0, colliderOffsetY - 0.8f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction.y = -1;
                collider2D.offset = new Vector2(0, -colliderOffsetY);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.isOpen);
        }

        direction.Normalize();
        playerRB.velocity = direction * speed;

        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);

    }

    static public void changeCursorState(bool isOn)
    {

        UnityEngine.Cursor.visible = isOn;

        if (isOn)
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        else UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void playerStartPosition()
    {
        if (LevelDataHolder.doorName != "default")
            gameObject.transform.SetPositionAndRotation(GameObject.Find(LevelDataHolder.doorName).transform.position - new Vector3(0, 1), transform.rotation);
    }

    public bool GetInterectPressed()
    {
        for(int i=0;i< interactButtons.Length;i++)
        {
            if (Input.GetKeyDown(interactButtons[i]))
            {
                return true;
            }
        }

        return false;
    }
}
