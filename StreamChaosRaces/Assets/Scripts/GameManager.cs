using SpeedTutorMainMenuSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { MENU, PLAY, HELP, END };

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameStates GameState { get; private set; }

    private string _username;
    private string _token;
    private string _channelName;

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
        SceneManager.LoadScene("MainMenu");

    }

    public void LoadVolverMenu()
    {
        StartCoroutine(CargarMenuAsync());
    }

    public void setGameState(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.MENU:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameStates.HELP:
                SceneManager.LoadScene("Help");
                break;
            case GameStates.PLAY:
                SceneManager.LoadScene("Circuito2");
                break;
            case GameStates.END:
                Application.Quit();
                break;
        }
        this.GameState = gameState;

    }

    public void setConfigTwitch(string username, string token, string channelName)
    {
        _channelName = channelName;
        _token = token;
        _username = username;
    }

    public string getUsername()
    {
        return _username;
    }

    public string getToken()
    {
        return _token;
    }

    public string getChannelName()
    {
        return _channelName;
    }

    IEnumerator CargarMenuAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
        while (!operation.isDone)
        {
            yield return null;
        }
        FindObjectOfType<MenuController>().OcultarConfig();
    }
}
