using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSystem : MonoBehaviour
{
    private List<Command> m_commands = new List<Command>();

    private int m_currentCommandIndex;

    public void ExecuteCommand(Command i_command)
    {
        m_commands.Add(i_command);
        i_command.Execute();
        m_currentCommandIndex = m_commands.Count - 1;
    }

    public void Undo()
    {
        if(!(m_currentCommandIndex == 0))
        {
            m_commands[m_currentCommandIndex].Undo();
            m_commands.RemoveAt(m_currentCommandIndex);
            m_currentCommandIndex--;
        }
    }

    public void Redo()
    {

    }
}
