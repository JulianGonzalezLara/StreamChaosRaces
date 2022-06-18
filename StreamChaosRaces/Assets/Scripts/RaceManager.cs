using KartGame.AI;
using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwitchChatConnect.Client;
using TwitchChatConnect.Config;
using TwitchChatConnect.Data;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    private static string START_COMMAND = "!play";

    public string username;
    public string userToken;
    public string channelName;

    public List<Transform> Spawns;
    public List<GameObject> playerPrefab; //posibles coches para correr
    private GameObject player;

    public int contadorPlayers = 0;
    public int maxJugadores = 0;
    public int numVueltas = 0;
    public int vueltaActual = 0;
    public bool partidaEmpezada = false;
    private bool partidaFinalizada = false;
    public List<KartAgent> car = new List<KartAgent>();

    // Start is called before the first frame update
    void Start()
    {

        TwitchChatClient.instance.Init(new TwitchConnectConfig(GameManager.Instance.getUsername(), GameManager.Instance.getToken(), GameManager.Instance.getChannelName()),() =>
        {
            TwitchChatClient.instance.onChatMessageReceived += OnChatMessageReceived;
            TwitchChatClient.instance.onChatCommandReceived += OnChatCommandReceived;
            TwitchChatClient.instance.onChatRewardReceived += OnChatRewardReceived;
        },
                message =>
                {
                    // Error when initializing.
                    Debug.LogError(message);
                });

        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Spawn"))
        {
            Spawns.Add(i.transform);
        }

        maxJugadores = Spawns.Count;
    }

    private void Update()
    {
        if (FindObjectOfType<Leaderboard>().car.Count > 0 && partidaFinalizada == false)
        {
            GameObject.Find("txtVueltas").GetComponent<TextMeshProUGUI>().text = string.Format("{0} / {1}", FindObjectOfType<Leaderboard>().getPrimero().gameObject.GetComponent<KartAgent>().finishLinePass, numVueltas);
            
            if(FindObjectOfType<Leaderboard>().getPrimero().gameObject.GetComponent<KartAgent>().finishLinePass == numVueltas + 1)
            {
                //final partida
                partidaFinalizada = true;
                Debug.Log("Partida finalizada");
                Debug.Log("El ganador es: " + FindObjectOfType<Leaderboard>().getPrimero().gameObject.name);
                FindObjectOfType<HUD>().FinalPartida(FindObjectOfType<Leaderboard>().getPrimero().gameObject.name);
            }
        }
    }

    private void OnChatCommandReceived(TwitchChatCommand chatCommand)
    {
        if (chatCommand.Command == START_COMMAND && partidaEmpezada == false && contadorPlayers < maxJugadores)
        {            
            string nombre = chatCommand.User.Username;
            Debug.Log(nombre);
            bool mismoUser = false;
            foreach (KartAgent i in car)
            {
                if(i.gameObject.name.Equals(nombre))
                {
                    mismoUser = true;
                }
            }
            if (!mismoUser)
            {
                int random = Random.Range(0, playerPrefab.Count);
                Debug.Log(random);
                player = Instantiate(playerPrefab[random], Spawns[contadorPlayers].transform.position, Spawns[contadorPlayers].transform.rotation);
                player.name = chatCommand.User.Username;
                player.GetComponent<ArcadeKart>().SetCanMove(false);
                player.gameObject.GetComponentInChildren<TextMeshPro>().text = chatCommand.User.Username;
                GameObject.FindGameObjectWithTag("CmCam").GetComponent<KartGame.Utilities.CineMachineTargeteer>().RefrescarCoches();
                contadorPlayers++;
                FindObjectOfType<HUD>().AddPlayerList(chatCommand.User);
            }

            FillCarList();
        }
        else
        {
            Debug.Log($"Unknown Command received: {chatCommand.Command}");
        }        
    }

    private void OnChatRewardReceived(TwitchChatReward chatReward)
    {
    }

    private void OnChatMessageReceived(TwitchChatMessage chatMessage)
    {
    }

    private void OnMatchBegin()
    {
        TwitchChatClient.instance.SendChatMessage("A new game has started");
    }

    private void FillCarList()
    {
        car.Clear();
        //get reference to all the cars
        foreach (KartAgent i in FindObjectsOfType<KartAgent>())
        {
            car.Add(i);
        }
    }
}
