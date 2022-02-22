using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    public WalkState(PlayerController i_player) : base(i_player)
    {
        Debug.Log("Player Walk State");
    }

    public override void Handle(KeyCode input)
    {
        ownerGameObject.SetAnimatorFloat("HorizontalAxis", Mathf.Abs(ownerGameObject.GetHorizontalAxis()));

        ownerGameObject.gameObject.transform.Translate(new Vector3(
            ownerGameObject.GetHorizontalAxis() * ownerGameObject.GetSpeed() * Time.deltaTime, 0.0f, 0.0f));

        if (ownerGameObject.GetIsJumping())
        {
            ownerGameObject.SetState(new JumpState(ownerGameObject));
        }

        if (Mathf.Abs(ownerGameObject.GetHorizontalAxis()) == 0)
        {
            ownerGameObject.SetState(new IdleState(ownerGameObject));
        }



    }
}
