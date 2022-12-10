using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MasterManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gameManager;
    public static MasterManager _instance;
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
    public void RequestMoveBall(Vector3 soccerPlayerPos, float forceMultiplier)
    {
        gameManager.Ball.MoveBall(soccerPlayerPos, forceMultiplier);
    }
}
