using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControl : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private Canvas UI;

    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;


    Vector3 moveDirection; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontalAxis, 0f, verticalAxis);
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 10.0f;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3.5f;
        }




        if(Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0.0f,  speed * Time.deltaTime, 0.0f);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            if (UI.enabled)
                UI.enabled = false;
            else
                UI.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
    }

}
