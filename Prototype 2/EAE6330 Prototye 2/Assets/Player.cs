using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask detectLayer;


    Vector2 m_moveDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_moveDir = Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_moveDir = Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_moveDir = Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_moveDir = Vector2.down;
        }

        if(m_moveDir != Vector2.zero)
        {
            if(CanGoToDir(m_moveDir))
            {
                GoTo(m_moveDir);
            }
        }

        m_moveDir = Vector2.zero;
    }


    bool CanGoToDir(Vector2 i_dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, i_dir, 0.75f, detectLayer);
        if(!hit)
        {
            return true;
        }

        else
        {
            print("aaaa");
            if(hit.collider.GetComponent<Box>() != null)
            {
                return hit.collider.GetComponent<Box>().canMoveToDir(i_dir);
            }
        }

        return false;
    }

    void GoTo(Vector2 i_dir)
    {
        transform.Translate(i_dir);
    }


}
