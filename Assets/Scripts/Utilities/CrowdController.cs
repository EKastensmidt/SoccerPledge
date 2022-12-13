using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrowdController : MonoBehaviourPun
{
    [SerializeField] private Animator animator;
    [SerializeField] private Team team;

    public void UpdateAnimation(bool isGoal, Team TeamWhoScored)
    {
        animator.SetBool("IsGoal", isGoal);

        if (TeamWhoScored == team)
        {
            animator.SetBool("Against", false);
        }
        else
        {
            animator.SetBool("Against", true);
        }
    }
}

public enum Team{ red, blue}
