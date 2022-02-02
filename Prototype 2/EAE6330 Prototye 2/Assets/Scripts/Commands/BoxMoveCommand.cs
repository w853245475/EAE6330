using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMoveCommand : Command
{

    private Vector2 moveDir;

    private Vector2 lastDir;

    public BoxMoveCommand(IActor i_Actor, Vector2 i_dir) : base(i_Actor)
    {
        moveDir = i_dir;

        if (moveDir == Vector2.up)
        {
            lastDir = Vector2.down;
        }
        else if (moveDir == Vector2.down)
        {
            lastDir = Vector2.up;
        }
        else if (moveDir == Vector2.left)
        {
            lastDir = Vector2.right;
        }
        else if (moveDir == Vector2.right)
        {
            lastDir = Vector2.left;
        }
        else
        {
            lastDir = Vector2.zero;
        }
    }

    public override void Execute()
    {
        GoTo(moveDir);
    }

    public override void Undo()
    {
        GoTo(lastDir);
    }


}
