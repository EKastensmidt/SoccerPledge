using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Instantiator : MonoBehaviourPun
{
    [SerializeField] private Button redButton, blueButton;
    [SerializeField] private Transform redSide, blueSide;

    public void BlueSideSelected()
    {
        PhotonNetwork.Instantiate("PlayerBlue", blueSide.position, Quaternion.identity);
        redButton.gameObject.SetActive(false);
        blueButton.gameObject.SetActive(false);
    }
    public void RedSideSelected()
    {
        PhotonNetwork.Instantiate("PlayerRed", redSide.position, Quaternion.identity);
        redButton.gameObject.SetActive(false);
        blueButton.gameObject.SetActive(false);
    }
}
