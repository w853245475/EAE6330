using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingState_Robot : IState_Robot
{
    int waitCounter;
    int waitforseconds = 600;

    public StandingState_Robot(PlayerMovement i_player) : base(i_player)
    {

    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        WaitToTransit();
    }

    void WaitToTransit()
    {
        if(waitCounter <= waitforseconds)
        {
            waitCounter++;
        }
        else
        {
            ownerPlayer.canMove = true;
            ownerPlayer.GetAnimator().SetTrigger("AfterStanding");
            ownerPlayer.SetState(new IdleState_Robot(ownerPlayer));
        }
    }
}
