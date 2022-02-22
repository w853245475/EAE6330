using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public IdleState(PlayerController i_gameObject) : base(i_gameObject)
    {
        Debug.Log("Player In Idle State");
    }

    public override void Handle(KeyCode input)
    {
        ownerGameObject.SetAnimatorFloat("HorizontalAxis", 0.0f);

        if (ownerGameObject.GetIsJumping())
        {
            ownerGameObject.SetState(new JumpState(ownerGameObject));
        }

        if (Mathf.Abs(ownerGameObject.GetHorizontalAxis()) >0)
        {
            ownerGameObject.SetState(new WalkState(ownerGameObject));
        }

        //else if (input == KeyCode.Space)
        //{
        //    ownerGameObject.SetState(new JumpState(ownerGameObject));
        //}

    }
}
