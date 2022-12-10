using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Goal : MonoBehaviourPun
{
    [SerializeField] private GameManager gameManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Ball ball = collision.GetComponent<Ball>();
        if(ball != null)
        {
            if(gameObject.layer == 6)
            {
                gameManager.TeamScored("BLUE");
            }
            else if(gameObject.layer == 7)
            {
                gameManager.TeamScored("RED");
            }
        }
        
    }
}
