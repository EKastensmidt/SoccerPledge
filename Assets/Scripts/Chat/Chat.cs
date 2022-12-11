using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Chat : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;
    string command = "w/";
    public void ChatSendMessage()
    {
        var message = inputField.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string [] words = message.Split(' ');

        if (words.Length > 2 && words[0] == command)
        {
            var target = words[1];
            foreach (var currentPlayer in PhotonNetwork.PlayerList)
            {
                if (target == currentPlayer.NickName)
                {
                    var currentMessage = string.Join(" ", words, 2, words.Length - 2);
                    photonView.RPC("GetChatMessage", currentPlayer, PhotonNetwork.NickName, currentMessage,true);
                    GetChatMessage(PhotonNetwork.NickName,currentMessage);
                    return;
                }
            }
            content.text += "<color=blue>" +"No existe target" + "</color>" + "\n";
            inputField.text = " ";
        }
        else
        {
            photonView.RPC("GetChatMessage", RpcTarget.All, PhotonNetwork.NickName, message, false);
            inputField.text = " ";
        }
        inputField.text = " ";
    }

    [PunRPC]
    public void GetChatMessage(string nameClient,string message, bool wm=false)
    {
        string color;
        if (PhotonNetwork.NickName == nameClient)
        {
            color = "<color=red>";

        }
        else if (wm)
        {
            color = "<color=yellow>";
        }
        else
        {
            color = "<color=blue>";
        }

        content.text += color + nameClient + ": " + "</color>" + message + "\n";
    }

}
