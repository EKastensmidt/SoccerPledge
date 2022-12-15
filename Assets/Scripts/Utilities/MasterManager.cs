using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using System.Linq;

public class MasterManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PhotoChat photoChat;
    public static MasterManager _instance;

    Dictionary<Player, SoccerPlayer> _dicChars = new Dictionary<Player, SoccerPlayer>();
    Dictionary<SoccerPlayer, Player> _dicPlayer = new Dictionary<SoccerPlayer, Player>();
    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    // RPC CALL
    public void RPCMaster(string name, params object[] p)
    {
        RPC(name, PhotonNetwork.MasterClient, p);
    }
    public void RPC(string name, Player target, params object[] p)
    {
        photonView.RPC(name, target, p);
    }

    [PunRPC]
    public void AddPlayerToDic(Player client, int id)
    {
        PhotonView pv = PhotonView.Find(id);
        SoccerPlayer soccerPlayer = pv.gameObject.GetComponent<SoccerPlayer>();
        _dicChars[client] = soccerPlayer;
        _dicPlayer[soccerPlayer] = client;
    }

    [PunRPC]
    public void RequestMoveBall(Vector3 soccerPlayerPos)
    {
        gameManager.Ball.MoveBall(soccerPlayerPos);
    }

    [PunRPC]
    public void SyncTime(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            SoccerPlayer soccerPlayer = _dicChars[client];
            soccerPlayer.GameManager.CurrentTime = gameManager.CurrentTime;
        }
    }

    [PunRPC]
    public void RequestSpawnWall(Vector3 spawnPos)
    {
        GameObject wall = PhotonNetwork.Instantiate("Wall", spawnPos, Quaternion.identity);
        StartCoroutine(DestroyObject(wall, 4f));
    }

    [PunRPC]
    public void RequestServerInfo(Player client)
    {
        var currentMessage = "<color=orange>" + "Room Name:"+ PhotonNetwork.CurrentRoom.Name + " / Server Region:"+ PhotonNetwork.CloudRegion + "</color>";
        photoChat.MasterSendMessage(currentMessage, client);
    }

    [PunRPC]
    public void RequestChangeBallColor(string r, string g, string b)
    {
        photonView.RPC("ChangeBallColor", RpcTarget.AllBuffered, int.Parse(r), int.Parse(g), int.Parse(b));
    }

    [PunRPC]
    public void ChangeBallColor(int r, int g, int b)
    {
        gameManager.Ball = FindObjectOfType<Ball>();

        if (gameManager.Ball == null)
            return;

        gameManager.Ball.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    private IEnumerator DestroyObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(obj);
    }
}
