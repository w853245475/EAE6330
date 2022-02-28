using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Robot : IState_Robot
{
    public IdleState_Robot(PlayerMovement i_player) : base(i_player)
    {
        ownerPlayer.GetAnimator().SetFloat("Velocity", 0.0f);
        ownerPlayer.velocity = 0.0f;
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        if(ownerPlayer.GetSpeed() > 0)
        {
            ownerPlayer.GetAnimator().SetFloat("Velocity", 3.0f);
            ownerPlayer.SetState(new WalkState_Robot(ownerPlayer));
        }

        if(ownerPlayer.GetController().isGrounded && Input.GetKeyDown(KeyCode.F))
        {
            ownerPlayer.SetState(new ThrowState_Robot(ownerPlayer));
        }
    }
}
