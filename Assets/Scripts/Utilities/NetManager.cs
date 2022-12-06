using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetManager : MonoBehaviourPunCallbacks
{
    public Button button;
    public TextMeshProUGUI status;
    public TMP_InputField nickName;
    public TMP_InputField roomName;
    public TMP_InputField maxPlayers;


    private void Start()
    {
        nickName.text = "eze";
        roomName.text = "salita";
        maxPlayers.text = 4.ToString();


    PhotonNetwork.ConnectUsingSettings();
        button.interactable = false;
        status.text = "Connecting to master";
    }

    public override void OnConnectedToMaster()
    {
        button.interactable = false;
        PhotonNetwork.JoinLobby();
        status.text = "Connecting to lobby";

    }

    public override void OnJoinedLobby()
    {
        button.interactable = true;
        status.text = "Connected to lobby";

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        status.text = "Failed to connect";
    }

    public override void OnLeftLobby()
    {
        status.text = "Lobby failed";
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(roomName.text) || string.IsNullOrWhiteSpace(roomName.text)) return;
        if (string.IsNullOrEmpty(nickName.text) || string.IsNullOrWhiteSpace(nickName.text)) return;
        if (string.IsNullOrEmpty(maxPlayers.text) || string.IsNullOrWhiteSpace(maxPlayers.text)) return;


        PhotonNetwork.NickName = nickName.text;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse(maxPlayers.text);
        PhotonNetwork.JoinOrCreateRoom(roomName.text,options,TypedLobby.Default);
        button.interactable = false;
    }

    public override void OnCreatedRoom()
    {
        status.text = "Created Room";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "Created Room failed";
        button.interactable = true;

    }

    public override void OnJoinedRoom()
    {
        status.text = "Joined Room";
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        status.text = "Join Room failed";
        button.interactable = true;
    }
}
