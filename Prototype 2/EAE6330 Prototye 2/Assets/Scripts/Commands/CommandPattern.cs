using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected IActor m_Actor;

    public Command(IActor i_Actor)
    {
        m_Actor = i_Actor;
    }

    public abstract void Execute();

    public abstract void Undo();

    protected void GoTo(Vector2 i_dir)
    {
        m_Actor.transform.Translate(i_dir.x, i_dir.y , 0, Space.World);
    }
}