using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoccerPlayer : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private PhotonView pv;
    private Vector2 initialPos;

    public float Speed { get => speed; set => speed = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public PhotonView Pv { get => pv; set => pv = value; }
    public Vector2 InitialPos { get => initialPos; set => initialPos = value; }

    public virtual void Start()
    {
        initialPos = transform.position;
    }

    public virtual void Update()
    {
        
    }
}
