using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicUi : MonoBehaviour
{
    public Image sprite;

    public void Show(bool v)
    {
        sprite.enabled = v;
    }

}
