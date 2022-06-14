using KartGame.AI;
using SpeedTutorMainMenuSystem;
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
    public GameObject PantallaFinal;
    public GameObject MenuPausa;
    public TextMeshProUGUI txtVueltas;
    public TextMeshProUGUI txtHelpCambio;
    public TextMeshProUGUI txt3;
    public TextMeshProUGUI txt2;
    public TextMeshProUGUI txt1;

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
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuPausa.SetActive(true);
            }
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
        StartCoroutine(CuentaAtras());
    }

    public void FinalPartida(string nombre)
    {
        PantallaFinal.SetActive(true);
        GameObject.Find("txtNombreGanador").GetComponent<TextMeshProUGUI>().text = nombre;
    }

    public void volverMenu()
    {
        GameManager.Instance.LoadVolverMenu();
    }

    public void QuitarPausa()
    {
        MenuPausa.SetActive(false);
    }

    IEnumerator CuentaAtras()
    {
        txtHelpCambio.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        txt3.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        txt3.gameObject.SetActive(false);
        txt2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        txt2.gameObject.SetActive(false);
        txt1.gameObject.SetActive(true);
        txtHelpCambio.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        txt1.gameObject.SetActive(false);
        FindObjectOfType<Leaderboard>().ArrancarCoches();
    }
}
