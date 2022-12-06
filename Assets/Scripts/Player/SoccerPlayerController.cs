using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayerController : SoccerPlayer
{
    private Vector3 movement;

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
        movement = new Vector3(x, y, 0);
        transform.position += movement * Time.deltaTime * Speed;

        if(x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        UpdateAnimations(movement);
    }

    private void UpdateAnimations(Vector3 movement)
    {
        Animator.SetFloat("MovX", movement.x);
        Animator.SetFloat("MovY", movement.y);
        Animator.SetFloat("Magnitude", movement.magnitude);
    }
}
