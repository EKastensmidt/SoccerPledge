using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoccerPlayerController : SoccerPlayer
{

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (!photonView.IsMine) return;
        Move();
    }

    private void Move() 
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Movement = new Vector3(x, y, 0);
        transform.position += Movement * Time.deltaTime * Speed;
        Debug.Log(Movement);
        if (Movement != Vector3.zero)
        {
            BallPos.position = transform.position + Movement;
        }

        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        UpdateAnimations(Movement);
    }

    private void UpdateAnimations(Vector3 movement)
    {
        Animator.SetFloat("MovX", movement.x);
        Animator.SetFloat("MovY", movement.y);
        Animator.SetFloat("Magnitude", movement.magnitude);
    }

}
