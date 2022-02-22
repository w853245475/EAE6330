using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState
{

    protected PlayerController ownerGameObject;

    public IState(PlayerController i_gameObject = null)
    {
        ownerGameObject = i_gameObject;
    }

    public abstract void Handle(KeyCode input = KeyCode.None);
}
