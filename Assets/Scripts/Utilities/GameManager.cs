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
    [SerializeField] private TextMeshProUGUI startCountdownText, winnerText, timeText, redScore, blueScore;
    [SerializeField] private GameObject waitingForMasterText, redScored, blueScored;
    public static bool isGameStarted = false;
    private int blueTeamScore;
    private int redTeamScore;
    private float currentTime = 0f;

    private List<SoccerPlayer> playerList;
    private Ball ball;

    public Ball Ball { get => ball; set => ball = value; }
    public int BlueTeamScore { get => blueTeamScore; set => blueTeamScore = value; }
    public int RedTeamScore { get => redTeamScore; set => redTeamScore = value; }
    public PhotonView Pv { get => pv; set => pv = value; }
    public bool IsEndOfGame { get => isEndOfGame; set => isEndOfGame = value; }
    public float CurrentTime { get => currentTime; set => currentTime = value; }

    private bool isEndOfGame = false;

    private List<CrowdController> crowd;

    private void Start()
    {
        SetStartRequirements();
        playerList = new List<SoccerPlayer>();
        crowd = GameObject.FindObjectsOfType<CrowdController>().ToList();
    }
    private void Update()
    {
        SetTime();
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

        if (team == "RED")
        {
            pv.RPC("UpdateTeamScore", RpcTarget.All, team);
            pv.RPC("RedScored", RpcTarget.All, true);
            UpdateCrowdAnimations(true, Team.red);
            if (redTeamScore == 3)
            {
                pv.RPC("SetWinner", RpcTarget.All, team);
                IsEndOfGame = true;
            }
        }
        else
        {
            pv.RPC("UpdateTeamScore", RpcTarget.All, team);
            pv.RPC("BlueScored", RpcTarget.All, true);
            UpdateCrowdAnimations(true, Team.blue);
            if (blueTeamScore == 3)
            {
                pv.RPC("SetWinner", RpcTarget.All, team);
                IsEndOfGame = true;
            }
        }
        StartCoroutine(AfterScore(team));
    }

    private IEnumerator AfterScore(string team)
    {
        PhotonNetwork.Destroy(ball.gameObject);

        yield return new WaitForSeconds(2f);

        if (team == "RED") {
            pv.RPC("RedScored", RpcTarget.All, false);
            UpdateCrowdAnimations(false, Team.red);
        }
        else {
            pv.RPC("BlueScored", RpcTarget.All, false);
            UpdateCrowdAnimations(false, Team.blue);
        }

        if (IsEndOfGame == false)
        {
            pv.RPC("ResetPlayerPositions", RpcTarget.All);
            SpawnBall();
        }
    }

    private int minutes, seconds;
    private float syncCd = 3f;
    private void SetTime()
    {
        if (!isGameStarted) return;

        currentTime += Time.deltaTime;
        minutes = (int)(currentTime / 60f);
        seconds = (int)(currentTime - minutes * 60f);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (!PhotonNetwork.IsMasterClient)
        {
            if (syncCd <= 0f)
            {
                MasterManager._instance.RPCMaster("SyncTime", PhotonNetwork.LocalPlayer);
                syncCd = 3f;
            }
            syncCd -= Time.deltaTime;
        }
    }

    private void UpdateCrowdAnimations(bool value, Team team)
    {
        foreach (var fan in crowd)
        {
            Debug.Log(fan.name);
            fan.UpdateAnimation(value, team);
        }
    }

    public void SetFastBall()
    {
        ball.ForceMultiplier *= 2f;
    }

    [PunRPC]
    public void SetWinner(string winningTeam)
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = winningTeam + " TEAM WINS!!!";

        if (winningTeam == "RED")
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
    public void UpdateTeamScore(string team)
    {
        if (team == "RED")
            redTeamScore++;
        else
            blueTeamScore++;
    }

    [PunRPC]
    public void RedScored(bool value)
    {
        redScored.SetActive(value);
        redScore.text = redTeamScore.ToString();
    }
    [PunRPC]
    public void BlueScored(bool value)
    {
        blueScored.SetActive(value);
        blueScore.text = blueTeamScore.ToString();
    }
}
