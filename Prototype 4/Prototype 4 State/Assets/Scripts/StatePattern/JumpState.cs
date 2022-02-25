using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    public JumpState(PlayerController i_player) : base(i_player)
    {

    }


    public override void Handle(KeyCode input = KeyCode.None)
    {
        ownerGameObject.gameObject.transform.Translate(new Vector3(
        ownerGameObject.GetHorizontalAxis() * ownerGameObject.GetSpeed() * Time.deltaTime, 0.0f, 0.0f));


        if (ownerGameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            ownerGameObject.SetIsJumping(false);

            if(Mathf.Abs(ownerGameObject.GetHorizontalAxis()) > 0)
            {
                ownerGameObject.SetState(new WalkState(ownerGameObject));
            }
            else
            {
                ownerGameObject.SetState(new IdleState(ownerGameObject));
            }
        }

        ownerGameObject.animator.SetBool("IsJumping", ownerGameObject.GetIsJumping());
    }
}
