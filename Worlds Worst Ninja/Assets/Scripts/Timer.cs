using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
   

    public TMP_Text TimerText,ResultText;
    public GameObject ResultScreen,ControlScreen,WeaponWheel;
    public float timeSpent, minutes, seconds;
    private bool _controlScreen;

    private PlayerMovement pm;

    public UnityEvent Reload, Back;

    private Controls inputs;

    private void Awake()
    {
        inputs = new Controls();
        inputs.Player.Restart.started += context => Re();
        inputs.Player.WeaopnWheel.started += context => ActivateSwitch();
        inputs.Player.WeaopnWheel.canceled += context => DeactivateSwitch();
    }

    void Start()
    {
        Time.timeScale = 1;
        pm = FindObjectOfType<PlayerMovement>();
        
    }
    private void Update()
    {
        timeSpent+= Time.deltaTime;
        minutes = Mathf.FloorToInt(timeSpent / 60);
        seconds = Mathf.FloorToInt(timeSpent % 60);

        TimerText.text = "Time: " + TimeSpan.FromSeconds(timeSpent).ToString("mm\\:ss\\.fff");

        //ControlScreen.SetActive(_controlScreen);

        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    Time.timeScale = 1;
        //    ResultScreen.SetActive(false);
        //    Reload.Invoke();
        //}

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    _controlScreen = !_controlScreen;
        //}

        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    SceneManager.LoadScene(0);
        //}


    }

    void Re()
    {
        Debug.Log("y0");
        SceneManager.LoadScene(0);
    }

    void ActivateSwitch()
    {
        Time.timeScale = 0.01f;
        WeaponWheel.SetActive(true);
    }

    void DeactivateSwitch()
    {
        Time.timeScale = 1;
        WeaponWheel.SetActive(false);
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==11)
        {
            Time.timeScale = 0;
            
                ResultText.text = "Congrats! You beat the level in " + TimeSpan.FromSeconds(timeSpent).ToString("mm\\:ss\\.fff");

        
            ResultScreen.SetActive(true);
        }
        
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
