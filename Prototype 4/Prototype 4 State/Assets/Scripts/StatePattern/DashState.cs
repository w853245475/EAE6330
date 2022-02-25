using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IState
{
    public int counter = 0;

    public DashState(PlayerController i_gameObject) : base(i_gameObject)
    {
        ownerGameObject.animator.SetBool("IsDashing", true);
        ownerGameObject.ClearPreviousState();

        
        //StartCoroutine(WaitToExit());
    }

    public override void Handle(KeyCode input = KeyCode.None)
    {
        if(counter < 30)
        {
            counter++;
        }
        else
        {
            if (ownerGameObject.GetComponent<Rigidbody2D>().velocity.x == 0)
            {
                ownerGameObject.SetState(new IdleState(ownerGameObject));

            }
            else
            {
                ownerGameObject.SetState(new WalkState(ownerGameObject));

            }
        }
    }

    IEnumerator WaitToExit()
    {
        yield return new WaitForSecondsRealtime(0.7f);

        if (ownerGameObject.GetComponent<Rigidbody2D>().velocity.x == 0)
        {
            ownerGameObject.SetState(new IdleState(ownerGameObject));

        }
        else
        {
            ownerGameObject.SetState(new WalkState(ownerGameObject));

        }
    }
}
