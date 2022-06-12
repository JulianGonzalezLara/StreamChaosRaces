using KartGame.AI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using TwitchChatConnect.Data;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Menus")]
    public GameObject MenuEspera;
    public GameObject Leaderboard;
    public TextMeshProUGUI txtVueltas;

    [Header("Elementos previos")]
    public TextMeshProUGUI txtListaJugadores;
    public TextMeshProUGUI txtNumJugadores;
    public InputField numVueltas;
    public Button btnPlay;
    private StringBuilder sb = new StringBuilder();

    // Update is called once per frame
    void Update()
    {
        if (MenuEspera.active)
        {
            if (int.Parse(numVueltas.text) < 1)
            {
                btnPlay.gameObject.SetActive(false);
            }
            else
            {
                btnPlay.gameObject.SetActive(true);
            }

            txtNumJugadores.text = string.Format("{0} / {1}", FindObjectOfType<RaceManager>().contadorPlayers, FindObjectOfType<RaceManager>().maxJugadores);
            
        }
    }

    public void AddPlayerList(TwitchUser user)
    {
        sb.AppendLine(user.Username);
        txtListaJugadores.text = sb.ToString();
    }

    public void PlayButton()
    {
        FindObjectOfType<RaceManager>().partidaEmpezada = true;
        MenuEspera.SetActive(false);
        Leaderboard.SetActive(true);
        txtVueltas.gameObject.SetActive(true);
        Leaderboard.GetComponentInChildren<Leaderboard>().FillCarList();
        FindObjectOfType<RaceManager>().numVueltas = int.Parse(numVueltas.text);
        FindObjectOfType<Leaderboard>().numVueltas = int.Parse(numVueltas.text);
        FindObjectOfType<Leaderboard>().ArrancarCoches();
    }
}
