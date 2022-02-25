using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpAmont;

    [SerializeField]
    private GameObject bullet;

    public Animator animator;
    private float horizontalAxis;
    private IState m_state;
    private bool isJumping = false;
    private bool isDashing = false;


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

                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up *  jumpAmont,
                ForceMode2D.Impulse);
            }


        }
        
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(isDashing == false)
            {
                //animator.SetBool("IsDashing", true);
                isDashing = true;
                if (GetComponent<SpriteRenderer>().flipX)
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 0.7f * jumpAmont,
                    ForceMode2D.Impulse);
                else
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 0.7f * jumpAmont,
                    ForceMode2D.Impulse);

                SetState(new DashState(this));
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(!isDashing)
                SetState(new FireState(this));
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

    public void ClearPreviousState()
    {
        isDashing = false;
    }

    public void Fire()
    {

        if (GetComponent<SpriteRenderer>().flipX)
        {
            GameObject currentBullet = Instantiate(bullet, transform.position + new Vector3(-3.0f, 0.0f, 0.0f), transform.rotation) as GameObject;
            currentBullet.GetComponent<Rigidbody>().AddForce(Vector2.left * 5000000);
        }

        else
        {
            GameObject currentBullet = Instantiate(bullet, transform.position + new Vector3(3.0f, 0.0f, 0.0f), transform.rotation) as GameObject;

            currentBullet.GetComponent<Rigidbody>().AddForce(Vector2.right * 5000000);
        }
    }
}
