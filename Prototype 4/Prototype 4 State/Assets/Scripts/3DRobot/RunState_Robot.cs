using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState_Robot : IState_Robot
{
    public RunState_Robot(PlayerMovement i_player) :base(i_player)
    {
        ownerPlayer.GetAnimator().SetFloat("Velocity", 6.0f);
        ownerPlayer.velocity = 5.0f;
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ownerPlayer.SetState(new WalkState_Robot(ownerPlayer));
        }
    }

}
