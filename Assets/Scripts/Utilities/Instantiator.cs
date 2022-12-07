using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Instantiator : MonoBehaviourPun
{
    [SerializeField] private Button redButton, blueButton;
    [SerializeField] private Transform redSide, blueSide;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }
        else
        {
            redButton.gameObject.SetActive(true);
            blueButton.gameObject.SetActive(true);
        }
    }

    //Called from blueTeam button
    public void BlueSideSelected()
    {
        PhotonNetwork.Instantiate("PlayerBlue", blueSide.position, Quaternion.identity);
        redButton.gameObject.SetActive(false);
        blueButton.gameObject.SetActive(false);
    }
    //Called from redTeam button
    public void RedSideSelected()
    {
        PhotonNetwork.Instantiate("PlayerRed", redSide.position, Quaternion.identity);
        redButton.gameObject.SetActive(false);
        blueButton.gameObject.SetActive(false);
    }
}
