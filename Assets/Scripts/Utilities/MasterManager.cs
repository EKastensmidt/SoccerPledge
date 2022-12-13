using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MasterManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gameManager;
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
    public void SyncTime(float currentTime)
    {
        currentTime = gameManager.CurrentTime;
    }
}
