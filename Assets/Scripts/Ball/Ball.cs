using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    private SoccerPlayer currentCarrier;
    private void Update()
    {
        if (currentCarrier != null)
        {
            transform.position = currentCarrier.transform.position + Vector3.up;
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
        pv.RPC("UpdateTarget", RpcTarget.Others, carrier.photonView.ViewID);
    }

    [PunRPC]
    public void UpdateTarget(int id)
    {
        PhotonView view = PhotonView.Find(id);
        if (view != null)
        {
            currentCarrier = view.gameObject.GetComponent<SoccerPlayer>();
        }
    }
}
