using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState_Robot : IState_Robot
{
    int waitCounter;
    int waitforseconds = 60;
    public ThrowState_Robot(PlayerMovement i_player) : base(i_player)
    {
        i_player.GetAnimator().SetTrigger("Throw");
        i_player.SpawnFireBullet();
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        WaitToTransit();
    }
    void WaitToTransit()
    {
        if (waitCounter <= waitforseconds)
        {
            waitCounter++;
        }
        else
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
    }
}
