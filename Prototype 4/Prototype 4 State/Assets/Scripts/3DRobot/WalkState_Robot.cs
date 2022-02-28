using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState_Robot : IState_Robot
{
    public WalkState_Robot(PlayerMovement i_player): base(i_player)
    {
        ownerPlayer.GetAnimator().SetFloat("Velocity", 2.0f);
        ownerPlayer.velocity = 2.0f;
    }
    public override void Handle(KeyCode input = KeyCode.None)
    {
        if(ownerPlayer.GetSpeed() == 0)
        {
            ownerPlayer.SetState(new IdleState_Robot(ownerPlayer));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ownerPlayer.SetState(new RunState_Robot(ownerPlayer));
        }

        if (ownerPlayer.GetController().isGrounded && Input.GetKeyDown(KeyCode.F))
        {
            ownerPlayer.SetState(new ThrowState_Robot(ownerPlayer));
        }
    }
   
}
