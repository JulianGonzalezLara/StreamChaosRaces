using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { MENU, CAMPOTIRO, LEVEL1, HELP, END };

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameStates GameState { get; private set; }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("GameManager es Null");
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _instance = this;
    }

    void Start()
    {
        LoadMenu();
    }

    public void LoadMenu()
    {
        setGameState(GameStates.MENU);
        SceneManager.LoadScene("Menu");

    }

    public void setGameState(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.MENU:
                SceneManager.LoadScene("Menu");
                break;
            case GameStates.HELP:
                SceneManager.LoadScene("Help");
                break;
            case GameStates.CAMPOTIRO:
                SceneManager.LoadScene("CampoDeTiro");
                break;
            case GameStates.LEVEL1:
                SceneManager.LoadScene("hangar");
                break;
            case GameStates.END:
                Application.Quit();
                break;
        }
        this.GameState = gameState;

    }
}
