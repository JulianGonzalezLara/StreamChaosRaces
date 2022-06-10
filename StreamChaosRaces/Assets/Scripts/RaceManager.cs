using System.Collections;
using System.Collections.Generic;
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
    public GameObject playerPrefab;
    private GameObject player;

    public int contadorPlayers = 0;
    public int maxJugadores = 0;

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

    private void OnChatCommandReceived(TwitchChatCommand chatCommand)
    {
        if (chatCommand.Command == START_COMMAND)
        {
            player = Instantiate(playerPrefab, Spawns[contadorPlayers].transform.position, Spawns[contadorPlayers].transform.rotation);
            player.name = chatCommand.User.Username;
            GameObject.FindGameObjectWithTag("CmCam").GetComponent<KartGame.Utilities.CineMachineTargeteer>().RefrescarCoches();
            contadorPlayers++;
            FindObjectOfType<HUD>().AddPlayerList(chatCommand.User);
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
}
