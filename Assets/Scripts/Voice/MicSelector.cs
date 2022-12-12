using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Voice.Unity;

public class MicSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Recorder recorder;

    private void Start()
    {
        var list = new List<string>();
        foreach (var item in Microphone.devices)
        {
            list.Add(item);
        } 

        dropdown.AddOptions(list);
    }
    public void SetMic(int i)
    {
        var currMic = Microphone.devices[i];
        recorder.MicrophoneDevice = new Photon.Voice.DeviceInfo(currMic);
    }
}
