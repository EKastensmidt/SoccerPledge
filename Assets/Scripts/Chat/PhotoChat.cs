using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
//using Photon.Voice.Unity;
using TMPro;

public class PhotoChat : MonoBehaviour, IChatClientListener
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;
    ChatClient chatClient;
    string command = "w/";
    string commandRoll = "r/";

    string channel;
    private void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion,
            new AuthenticationValues(PhotonNetwork.NickName));
    }
    private void Update()
    {
        chatClient.Service();
    }

    public void ChatSendMessage()
    {
        var message = inputField.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');

        if (words.Length > 2 && words[0] == command)
        {
            var target = words[1];
            foreach (var currentPlayer in PhotonNetwork.PlayerList)
            {
                if (target == currentPlayer.NickName)
                {
                    var currentMessage = string.Join(" ", words, 2, words.Length - 2);
                    chatClient.SendPrivateMessage(target, currentMessage);
                    return;
                }
            }
            content.text += "<color=blue>" + "No existe target" + "</color>" + "\n";
            inputField.text = " ";
        }
        else if (words.Length > 2 && words[0] == commandRoll)
        {
            var numero = Random.Range(1,100);
            message = numero.ToString();
            chatClient.PublishMessage(channel, message);
            inputField.text = " ";
        }
        else
        {
            chatClient.PublishMessage(channel,message);
            inputField.text = " ";
        }
        inputField.text = " ";

    }
    void IChatClientListener.DebugReturn(DebugLevel level, string message)
    {

    }

    void IChatClientListener.OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        content.text += "SI SE CONECTO" + "\n";
        channel = PhotonNetwork.CurrentRoom.Name;
        chatClient.Subscribe(channel);
        string[] friends = new string[] {"eze","juan"};
        chatClient.AddFriends(friends);

        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        content.text += "NO SE CONECTO" + "\n";

    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

        for (int i = 0; i < senders.Length; i++)
        {
            var currSenders = senders[i];
            string color;
            if (PhotonNetwork.NickName == currSenders)
            {
                color = "<color=red>";

            }
            else
            {
                color = "<color=blue>";
            }

            content.text += color + currSenders + ": " + "</color>" + messages[i] + "\n";
        }


    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string color;
        color = "<color=yellow>";
        content.text += color + sender + ": " + "</color>" + message + "\n";
        inputField.text = " ";
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        string userStatus = "";
        if (status == 2)
        {
            userStatus = "connected";
        }
        else if (status == 0)
        {
            userStatus = "disconnected";

        }
        content.text += "<color=green>" + user + " "+ userStatus + "</color>" + "\n";

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            content.text += "Subscribe to" + channels[i] + "\n";

        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            content.text += "UnSubscribe to" + channels[i] + "\n";

        }
    }

    void IChatClientListener.OnUserSubscribed(string channel, string user)
    {
    }

    void IChatClientListener.OnUserUnsubscribed(string channel, string user)
    {
    }
}
