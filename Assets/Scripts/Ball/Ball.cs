using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    private SoccerPlayer currentCarrier;
    private Rigidbody2D rb;
    [SerializeField] private float forceMultiplier;

    public float ForceMultiplier { get => forceMultiplier; set => forceMultiplier = value; }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient) {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (currentCarrier != null && currentCarrier.HasBall)
        {
            transform.position = currentCarrier.BallPos.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SoccerPlayer player = collision.gameObject.GetComponent<SoccerPlayer>();
            if (player)
            {
                SetCarrier(player);
            }
        }
    }

    public void SetCarrier(SoccerPlayer carrier)
    {
        currentCarrier = carrier;
        currentCarrier.HasBall = true;
        pv.RPC("UpdateTarget", RpcTarget.Others, carrier.photonView.ViewID);
    }

    public void MoveBall(Vector3 soccerPlayerPos)
    {
        rb.AddForce((transform.position - soccerPlayerPos) * forceMultiplier);
    }

    [PunRPC]
    public void UpdateTarget(int id)
    {
        PhotonView view = PhotonView.Find(id);
        Debug.Log("view");
        if (view != null)
        {
            currentCarrier = view.gameObject.GetComponent<SoccerPlayer>();
            currentCarrier.HasBall = true;
            Debug.Log("asdasd");
        }
    }
}
