using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Input Field configuracion Twitch")]
    public InputField username;
    public InputField token;
    public InputField channelName;

    [Header("Menus")]
    public GameObject menuConfigTwitch;
    public GameObject menu;

    void Start()
    {
    }

    public void LoadConfigTwitch()
    {
        GameManager.Instance.setConfigTwitch(username.text, token.text, channelName.text);
        menuConfigTwitch.SetActive(false);
        menu.SetActive(true);
    }

    public void LoadHelp()
    {
        GameManager.Instance.setGameState(GameStates.HELP);
    }

    public void LoadPlay()
    {
        GameManager.Instance.setGameState(GameStates.PLAY);
    }

    public void Return()
    {
        menu.SetActive(true);
        /*creditos.SetActive(false);
        settings.SetActive(false);
        ayuda.SetActive(false);
        play.SetActive(false);*/
    }

    public void Salir()
    {
        GameManager.Instance.setGameState(GameStates.END);
    }
}
