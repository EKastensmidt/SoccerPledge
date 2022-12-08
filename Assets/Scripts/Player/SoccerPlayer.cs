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
    private Vector3 movement;
    [SerializeField] private Transform ballPos;

    private bool hasBall = false;

    public float Speed { get => speed; set => speed = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public PhotonView Pv { get => pv; set => pv = value; }
    public Vector2 InitialPos { get => initialPos; set => initialPos = value; }
    public bool HasBall { get => hasBall; set => hasBall = value; }
    public Vector3 Movement { get => movement; set => movement = value; }
    public Transform BallPos { get => ballPos; set => ballPos = value; }

    public virtual void Start()
    {
        initialPos = transform.position;
    }

    public virtual void Update()
    {
    }
}
