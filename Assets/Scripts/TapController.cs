using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]

public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public Button buttonUp;
    public Button buttonDown;



    float tapForce = 200;
    float tiltSmooth = 2;
    Vector3 startPos;
    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;

    Rigidbody2D rigidBody;
    Quaternion downRotation;
    Quaternion upRotation;

    Quaternion forwardRotation;
    Quaternion backwardRotation;
    Quaternion _rotation;

   

    GameManager game;

    private static bool isOnLine = false;
    private static bool gravityDown = false;

    private static Vector2 forceVector;



    void Start()
    {
        buttonUp.GetComponent<Button>().onClick.AddListener(delegate { TaskOnButtonUpClick(); });
        buttonDown.GetComponent<Button>().onClick.AddListener(delegate { TaskOnButtonDownClick(); });

        buttonDown.gameObject.SetActive(false);
        buttonUp.gameObject.SetActive(false);

        startPos = new Vector3(-4.2f, 0, 0);
        rigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        upRotation = Quaternion.Euler(0, 0, 90);

        forwardRotation = Quaternion.Euler(0, 0, 35);
        backwardRotation = Quaternion.Euler(0, 0, -35);

        game = GameManager.Instance;
        rigidBody.simulated = false;
        Physics2D.gravity = new Vector2(0, -9.8f);

        game.StartGame();


    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0) && isOnLine == false)
        {                
                transform.rotation = _rotation;
                rigidBody.velocity = Vector3.zero;
                rigidBody.AddForce(forceVector * tapForce, ForceMode2D.Force);
                tapSound.Play();
                //Debug.Log("gravityDown : " + gravityDown);
        }

        //Debug.Log("UPDATE_gravityDown : " + gravityDown);
        //Debug.Log("UPDATE_isOnline : " + isOnLine);

        if (isOnLine == false && gravityDown == true)
            transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);

        if (isOnLine == false && gravityDown == false)
            transform.rotation = Quaternion.Lerp(transform.rotation, upRotation, tiltSmooth * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W) && buttonDown.gameObject.activeSelf)
        {
            TaskOnButtonUpClick();
            Debug.Log("W key was pressed.");
        }

        if (Input.GetKeyUp(KeyCode.S) && buttonDown.gameObject.activeSelf)
        {
            TaskOnButtonDownClick();
            Debug.Log("S key was released.");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ScoreZone")
        {
            OnPlayerScored(); // event sent to GameManager
            scoreSound.Play();

        }

        if (collision.gameObject.tag == "DeadZone")
        {
            rigidBody.simulated = false;
            OnPlayerDied(); // event sent to GameManager
            dieSound.Play();

        }

        if (collision.gameObject.tag == "Line")
        {
            buttonDown.gameObject.SetActive(true);
            buttonUp.gameObject.SetActive(true);
            isOnLine = true;
            rigidBody.velocity = Vector3.zero;
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
            Physics2D.gravity = new Vector2(0, 0);            
        }
    }

    void TaskOnButtonUpClick()
    {
        isOnLine = false;
        gravityDown = false;
        Physics2D.gravity = new Vector2(0, 9.8f);
        forceVector = Vector2.down;
        _rotation = backwardRotation;
        buttonDown.gameObject.SetActive(false);
        buttonUp.gameObject.SetActive(false);
        //Debug.Log("Clicked Up");
    }

    void TaskOnButtonDownClick()
    {
        isOnLine = false;
        gravityDown = true;
        Physics2D.gravity = new Vector2(0, -9.8f);
        forceVector = Vector2.up;
        _rotation = forwardRotation;
        buttonDown.gameObject.SetActive(false);
        buttonUp.gameObject.SetActive(false);
        //Debug.Log("Clicked Down");
    }

}
