using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	float inputX;
	float inputZ;

	float speed;

	float allowPlayerRotation = 0.1f;
	float jumpSpeed = 60.0f;
	public float velocity { get; set; } = 2.0f;
	public float verticalVel { get; set; }
	float rotationSpeed = 0.1f;

	public bool canMove { get; set; } = false;
	public bool isRunning { get; set; } = false;

	Animator anim;
	Camera mainCam;

	[SerializeField]
	private GameObject fireBolt;

	[SerializeField]
	private GameObject fireBullet;

	[SerializeField]
	private GameObject fireWhenSpawned;

	// Finite State Machine
	IState_Robot state;

	// Start is called before the first frame update
	void Start()
    {
		state = new StandingState_Robot(this);
		mainCam = Camera.main;
		anim = GetComponent<Animator>();

		Instantiate(fireWhenSpawned, transform);
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		inputX = Input.GetAxis("Horizontal");
		inputZ = Input.GetAxis("Vertical");

		InputMagnitude();

		if (GetComponent<CharacterController>().isGrounded)
		{
			if (Input.GetKeyDown(KeyCode.Space) && canMove)
			{
				SetState(new JumpState_Robot(this));
			}

			verticalVel -= 0;
		}
		else
		{
			verticalVel -= 1;
		}
		Vector3 moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
		GetComponent<CharacterController>().Move(moveVector);


		mainCam.transform.position = new Vector3(transform.position.x + 8.0f,
			transform.position.y + 8.0f,
			transform.position.z + 3.0f);


		state.Handle();
	}

	void InputMagnitude()
	{
		speed = new Vector2(inputX, inputZ).sqrMagnitude;

		if(canMove)
		{
			if(speed > allowPlayerRotation)
			{
				PlayerMoveAndRotation();
			}
		}
	}

	void PlayerMoveAndRotation()
	{
		var forward = mainCam.transform.forward;
		var right = mainCam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize();
		right.Normalize();

		Vector3 desiredMoveDirection = forward * inputZ + right * inputX;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
		GetComponent<CharacterController>().Move(desiredMoveDirection * Time.deltaTime * velocity);

	}

	public void SetState(IState_Robot i_state)
	{
		state = i_state;
	}

	public Animator GetAnimator()
	{
		return anim;
	}

	public float GetSpeed()
	{
		return speed;
	}

	public CharacterController GetController()
	{
		return GetComponent<CharacterController>();
	}


	public void SpawnFireBolt()
	{
		Instantiate(fireBolt, transform.localPosition, transform.rotation);
	}

	public void SpawnFireBullet()
	{
		Instantiate(fireBullet, transform.localPosition, transform.rotation);
	}
}
