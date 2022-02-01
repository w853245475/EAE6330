using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Color arriveTargetColor;

    Color defaultColor;

    private void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
        FindObjectOfType<GameManager>().totalBoxes++;
    }
    public bool canMoveToDir(Vector2 i_dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)i_dir * 0.49f, i_dir, 0.5f);
        print(i_dir);
        if(!hit)
        {
            transform.Translate(i_dir);

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

}
