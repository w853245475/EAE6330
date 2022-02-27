using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState_Robot
{
    protected PlayerMovement ownerPlayer;

    public IState_Robot(PlayerMovement i_player)
    {
        ownerPlayer = i_player;
    }

    public abstract void Handle(KeyCode input = KeyCode.None);
}
