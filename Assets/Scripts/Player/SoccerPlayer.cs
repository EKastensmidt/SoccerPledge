using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoccerPlayer : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private PhotonView pv;

    private SpriteRenderer sprite;
    public float Speed { get => speed; set => speed = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public PhotonView Pv { get => pv; set => pv = value; }
    public SpriteRenderer Sprite { get => sprite; set => sprite = value; }

    public virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        
    }
}
