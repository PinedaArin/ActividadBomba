﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameState currentState = GameState.MainScreen;

    [Header("Game Windows")]
    public EasyTween MainScreen;
    public EasyTween Legals;
    public EasyTween DataBase;
    public EasyTween GameInstruccions;
    public EasyTween Questions;
    public EasyTween congratulations;
    public EasyTween yourLose;

    [Header("VFX")]
    public GameObject cameraVFX;
    public GameObject fireworksVFX;

    public AudioClip bgmWinMusic;

    public static GameManager Instance;

    private bool bCanAdvanceScreen = true;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       
        currentState = GameState.MainScreen;

        cameraVFX.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.PageDown) && bCanAdvanceScreen)
        {
            switch (currentState)
            {
                case GameState.MainScreen:
                    ShowLegals();
                    currentState = GameState.Legals;
                    break;
                case GameState.Legals:
                    AcceptLegas();
                    currentState = GameState.Database;
                    break;
                case GameState.Database:
                    Submit();           
                    break;
                case GameState.GameInstructions:
                   
                    break;

                case GameState.Questions:
                case GameState.Congratulations:
                case GameState.YourLose:
                default:
                    break;
            }

        }

        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            ResetGame();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentState == GameState.GameInstructions)
            {
                if (bCanAdvanceScreen == false)
                    return;
                bCanAdvanceScreen = false;
                Invoke("AllowAdvanceScreen", 3.0f);
                    ToPlay();
                    currentState = GameState.Questions;
                
                 
            }
        }

    }

    public void ShowLegals ()
    {
        if (bCanAdvanceScreen == false)
            return;

        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        MainScreen.OpenCloseObjectAnimation();
        Legals.OpenCloseObjectAnimation();
    }

    public void AcceptLegas()
    {
        if (bCanAdvanceScreen == false)
            return;
        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        Legals.OpenCloseObjectAnimation();
        DataBase.OpenCloseObjectAnimation();
    }

    public void DeclineLegals()
    {
        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        Legals.OpenCloseObjectAnimation();
        MainScreen.OpenCloseObjectAnimation();
    }
    public void Submit()
    {
        if (bCanAdvanceScreen == false)
            return;

        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        currentState = GameState.GameInstructions;
        DataBase.OpenCloseObjectAnimation();
        GameInstruccions.OpenCloseObjectAnimation();
    }
    public void ToPlay()
    {
        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        GameInstruccions.OpenCloseObjectAnimation();
        Questions.OpenCloseObjectAnimation();
    }
    public void Congratulations()
    {
        bCanAdvanceScreen = false;
        Invoke("AllowAdvanceScreen", 3.0f);
        Questions.OpenCloseObjectAnimation();
        congratulations.OpenCloseObjectAnimation();
        Invoke("ResetGame", 15);
    }
    public void YourLose()
    {
        Invoke("ShowYouLoseScreen",2f);
    }

    public void ShowYouLoseScreen ()
    {
        Questions.OpenCloseObjectAnimation();
        yourLose.OpenCloseObjectAnimation();
        Invoke("ResetGame", 5);
    }

    private void ResetGame ()
    {
        SceneManager.LoadScene("Main");
    }

    private void AllowAdvanceScreen ()
    {
        bCanAdvanceScreen = true;
    }

    public void CongratulationsSequence ()
    {
        StartCoroutine(SpawnFireworks());
        Vector3 cameraPosition = Camera.main.transform.position;
        AudioSource.PlayClipAtPoint(bgmWinMusic, cameraPosition);
    }

    IEnumerator SpawnFireworks ()
    {
        while(true)
        {
            SpawnRandomFireworksVFX();
            yield return new WaitForSeconds(1.5f);

        }
    }

    public void SpawnRandomFireworksVFX()
    {
       
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
        Instantiate(fireworksVFX, screenPosition, Quaternion.identity);
    }
}

public enum GameState
{
    MainScreen,
    Legals,
    Database,
    GameInstructions,
    Questions,
    Congratulations,
    YourLose
}
