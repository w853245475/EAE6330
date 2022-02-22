using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpAmont;

    public Animator animator;
    private float horizontalAxis;
    private IState m_state;
    private bool isJumping = true;

    // Start is called before the first frame update
    void Start()
    {
        m_state = new IdleState(this);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");

        if(horizontalAxis > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (horizontalAxis < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumping == false)
            {
                
                isJumping = true;
                
            }
        }
        

        m_state.Handle();

        
    }

    public void SetState(IState i_state)
    {
        m_state = i_state;
    }

    public float GetHorizontalAxis()
    {
        return horizontalAxis;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }

    public void SetIsJumping(bool i)
    {
        isJumping = i;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetJumpAmount()
    {
        return jumpAmont;
    }

    public void SetAnimatorFloat(string i_name, float i_value)
    {
        animator.SetFloat(i_name, i_value);
    }
}
