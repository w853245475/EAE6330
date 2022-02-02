using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IActor
{
    public Color arriveTargetColor;

    Color defaultColor;

    private Vector2 m_lastPosition;

    private CommandSystem m_commandSystem;

    private void Awake()
    {
        EventCenter.AddListener(EventType.PLAYERMOVENOCOLLISION, AddPaddingCommand);

        m_commandSystem = GetComponent<CommandSystem>();
    }

    private void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
        FindObjectOfType<GameManager>().totalBoxes++;

        var command = new BoxMoveCommand(this, Vector2.zero);
        m_commandSystem.ExecuteCommand(command);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.PLAYERMOVENOCOLLISION, AddPaddingCommand);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_commandSystem.Undo();
        }
    }

    public bool canMoveToDir(Vector2 i_dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)i_dir * 0.49f, i_dir, 0.5f);
        if(!hit)
        {
            var command = new BoxMoveCommand(this, i_dir);
            m_commandSystem.ExecuteCommand(command);

            EventCenter.RemoveListener(EventType.PLAYERMOVENOCOLLISION, AddPaddingCommand);
            EventCenter.Broadcast(EventType.PLAYERMOVENOCOLLISION);
            EventCenter.AddListener(EventType.PLAYERMOVENOCOLLISION, AddPaddingCommand);

            //transform.Translate(i_dir);

            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Target")
        {
            FindObjectOfType<GameManager>().arrivedBoxes++;
            FindObjectOfType<GameManager>().CheckFinish();
            GetComponent<SpriteRenderer>().color = arriveTargetColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Target")
        {
            FindObjectOfType<GameManager>().arrivedBoxes--;
            GetComponent<SpriteRenderer>().color = defaultColor;
        }
    }

    void AddPaddingCommand()
    {
        var command = new BoxMoveCommand(this, Vector2.zero);
        m_commandSystem.ExecuteCommand(command);
    }
}
