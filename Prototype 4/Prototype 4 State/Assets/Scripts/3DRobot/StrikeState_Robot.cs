using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeState_Robot : IState_Robot
{
    float strikeSpeed = -100.0f;
    public StrikeState_Robot(PlayerMovement i_player) : base(i_player)
    {
        ownerPlayer.verticalVel = strikeSpeed;
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        if(ownerPlayer.GetController().isGrounded)
        {
            ownerPlayer.SpawnFireBolt();
            if (ownerPlayer.GetSpeed() == 0)
            {
                ownerPlayer.SetState(new IdleState_Robot(ownerPlayer));
            }
            else
            {
                ownerPlayer.SetState(new WalkState_Robot(ownerPlayer));
            }
        }

    }
}
