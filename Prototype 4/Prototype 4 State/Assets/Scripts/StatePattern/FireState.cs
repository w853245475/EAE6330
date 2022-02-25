using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : IState
{
    public FireState(PlayerController i_player) : base(i_player)
    {
        i_player.Fire();
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        if (ownerGameObject.GetIsJumping())
        {
            ownerGameObject.SetState(new JumpState(ownerGameObject));
        }

        if (Mathf.Abs(ownerGameObject.GetHorizontalAxis()) == 0)
        {
            ownerGameObject.SetState(new IdleState(ownerGameObject));
        }
        else
        {
            ownerGameObject.SetState(new WalkState(ownerGameObject));
        }
    }
}
