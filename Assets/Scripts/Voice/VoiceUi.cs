using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class VoiceUi : MonoBehaviourPun
{
    Recorder mic;
    [SerializeField]private GameObject micUi;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
            Destroy(this);

        mic = GameObject.Find("VoiceManager").GetComponent<Recorder>();
        mic.TransmitEnabled = false;

        micUi.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            mic.TransmitEnabled = true;
            micUi.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            mic.TransmitEnabled = false;
            micUi.SetActive(false);
        }
    }
}
