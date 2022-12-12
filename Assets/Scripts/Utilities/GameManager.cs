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
    [SerializeField] private TextMeshProUGUI startCountdownText, winnerText;
    [SerializeField] private GameObject waitingForMasterText, redScored, blueScored;
    public static bool isGameStarted = false;
    private int blueTeamScore;
    private int redTeamScore;

    private List<SoccerPlayer> playerList;
    private Ball ball;

    public Ball Ball { get => ball; set => ball = value; }
    public int BlueTeamScore { get => blueTeamScore; set => blueTeamScore = value; }
    public int RedTeamScore { get => redTeamScore; set => redTeamScore = value; }
    public PhotonView Pv { get => pv; set => pv = value; }
    public bool IsEndOfGame { get => isEndOfGame; set => isEndOfGame = value; }

    private bool isEndOfGame = false;

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
        pv.RPC("ResetPlayerPositions", RpcTarget.All);
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnBall();
        }
    }

    public void SpawnBall()
    {
        GameObject ballObject = PhotonNetwork.Instantiate("Ball", new Vector3(0, -1.6f), Quaternion.identity);
        ball = ballObject.GetComponent<Ball>();
    }

    public void TeamScored(string team)
    {
        if (IsEndOfGame)
            return;

        if(team == "RED")
        {
            redTeamScore++;
            if (redTeamScore == 3)
            {
                pv.RPC("SetWinner", RpcTarget.All, "RED TEAM");
                IsEndOfGame = true;
            }
        }
        else
        {
            blueTeamScore++;
            if (blueTeamScore == 3)
            {
                pv.RPC("SetWinner", RpcTarget.All, "BLUE TEAM");
                IsEndOfGame = true;
            }
        }

        if (IsEndOfGame == false)
        {
            StartCoroutine(AfterScore(team));
        }
    }

    private IEnumerator AfterScore(string team)
    {
        if(team == "RED"){
            pv.RPC("RedScored", RpcTarget.All, true);
        }
        else{
            pv.RPC("BlueScored", RpcTarget.All, true);
        }

        yield return new WaitForSeconds(1f);

        PhotonNetwork.Destroy(ball.gameObject);

        yield return new WaitForSeconds(2f);

        if (team == "RED"){
            pv.RPC("RedScored", RpcTarget.All, false);
        }
        else{
            pv.RPC("BlueScored", RpcTarget.All, false);
        }
        pv.RPC("ResetPlayerPositions", RpcTarget.All);
        SpawnBall();
    }
    

    [PunRPC]
    public void SetWinner(string winningTeam)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = winningTeam + " WINS!!!";

        if(winningTeam == "RED TEAM")
        {
            winnerText.color = Color.red;
        }
        else
        {
            winnerText.color = Color.blue;
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

    [PunRPC]
    public void RedScored(bool value)
    {
        redScored.SetActive(value);
    }
    [PunRPC]
    public void BlueScored(bool value)
    {
        blueScored.SetActive(value);
    }
}
