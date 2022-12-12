using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class VoiceUi : MonoBehaviourPun
{
    public Recorder mic;
    MicUi micUi;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            micUi = FindObjectOfType<MicUi>();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            mic.TransmitEnabled = true;
            micUi.Show(true);
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            mic.TransmitEnabled = false;
            micUi.Show(false);
        }
    }
}
