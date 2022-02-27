using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState_Robot : IState_Robot
{
    float jumpSpeed = 80.0f;
    public JumpState_Robot(PlayerMovement i_player) : base(i_player)
    {
        ownerPlayer.verticalVel = jumpSpeed;
        i_player.GetAnimator().SetTrigger("Jump");
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        if(ownerPlayer.GetController().isGrounded)
        {
            if (ownerPlayer.GetSpeed() == 0)
            {
                ownerPlayer.SetState(new IdleState_Robot(ownerPlayer));
            }
            else
            {
                ownerPlayer.SetState(new WalkState_Robot(ownerPlayer));
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                ownerPlayer.SetState(new StrikeState_Robot(ownerPlayer));
            }
        }
    }
}
