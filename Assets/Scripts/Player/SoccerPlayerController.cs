using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoccerPlayerController : SoccerPlayer
{
    private GameObject chat;
    bool ischatHidden = false;

    public override void Start()
    {
        base.Start();
        chat = GameObject.Find("Chat");
    }

    public override void Update()
    {
        if (!photonView.IsMine) return;
        base.Update();
        Move();
        Shoot();
        HideShowChat();
    }

    private void Move() 
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Movement = new Vector3(x, y, 0);
        transform.position += Movement * Time.deltaTime * Speed;
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

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && HasBall)
        {
            MasterManager._instance.RPCMaster("RequestMoveBall", transform.position);
            Pv.RPC("HasReleasedBall", RpcTarget.All);
        }
    }

    private void HideShowChat()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (!ischatHidden)
            {
                chat.transform.position += Vector3.right * 1000f;
                ischatHidden = true;
            }
            else
            {
                chat.transform.position += Vector3.left * 1000f;
                ischatHidden = false;
            }
        }
    }

}
