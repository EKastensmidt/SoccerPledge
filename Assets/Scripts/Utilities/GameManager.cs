using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private Button masterStartButton;
    [SerializeField] private TextMeshProUGUI startCountdownText;
    [SerializeField] private GameObject waitingForMasterText;
    public static bool isGameStarted = false;

    private List<SoccerPlayer> playerList;

    private void Start()
    {
        SetStartRequirements();
        playerList = new List<SoccerPlayer>();
    }


    // Called by MasterButton.
    public void StartGameButtonPressed()
    {
        pv.RPC("GameStarted", RpcTarget.All);
    }

    public void SetStartRequirements()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterStartButton.gameObject.SetActive(true);
        }
        else
        {
            waitingForMasterText.SetActive(true);
        }
    }
    private IEnumerator WaitToStart()
    {
        startCountdownText.gameObject.SetActive(true);
        startCountdownText.text = "GAME STARTING: 3";
        yield return new WaitForSeconds(1f);
        startCountdownText.text = "GAME STARTING: 2";
        yield return new WaitForSeconds(1f);
        startCountdownText.text = "GAME STARTING: 1";
        yield return new WaitForSeconds(1f);
        startCountdownText.gameObject.SetActive(false);
        isGameStarted = true;

        playerList = GameObject.FindObjectsOfType<SoccerPlayer>().ToList();
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("ResetPlayerPositions", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ResetPlayerPositions()
    {
        foreach (var player in playerList)
        {
            player.transform.position = player.InitialPos;
        }
    }

    [PunRPC]
    public void GameStarted()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            masterStartButton.gameObject.SetActive(false);
        }
        else
        {
            waitingForMasterText.SetActive(false);
        }
        StartCoroutine(WaitToStart());
    }
}
