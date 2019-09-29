using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterControl : MonoBehaviour
{
	#region Singleton
	public static CharacterControl instance;

	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one CharacterControl!");
			return;
		}
		instance = this;
	}
	#endregion

	[System.Serializable]
	public class MoveSettings
	{
		public float fowardVel = 12;
		public float strafeVel = 8;
		public float rotateVel = 0.03f;
		public float minCamRot, maxCamRot;
		public float jumpVel = 25;
		public float distToGrounded = 0.1f;
		public LayerMask ground; 
	}

	[System.Serializable]
	public class PhycsSettings
	{
		public float downAccel = 0.75f;
	}

	[System.Serializable]
	public class InputSettings
	{
		public float inputDelay = 0.1f;
		public bool lockCursor = true;
		public string FORWARD_AXIS = "Vertical";
		public string STRAFE_AXIS = "Horizontal";
		public string TURN_AXIS = "Mouse X";
		public string CAM_Y_AXIS = "Mouse Y";
		public string JUMP_AXIS = "Jump";
	}

	public MoveSettings moveSettings = new MoveSettings();
	public PhycsSettings phycsSettings = new PhycsSettings();
	public InputSettings inputSettings = new InputSettings();

	Vector3 velocity = Vector3.zero;
	Vector3 rotation = Vector3.zero;
	Vector3 cameraRotation = Vector3.zero;
	private float camXRot;
	private bool m_cursorIsLocked = true;
	public Rigidbody rb;
	public Camera cam;
	//public GameObject camHingeY;
    public Transform camTransform;

	float forwardInput, turnInput, camHorizontalInput, jumpInput, strafeInput;

	public bool Grounded()
	{
		return Physics.Raycast(transform.position, Vector3.down, moveSettings.distToGrounded, moveSettings.ground);
	}

	void Start()
	{
		forwardInput = turnInput = jumpInput = strafeInput = camHorizontalInput = 0;
	}

	void GetInput()
	{
		turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
		camHorizontalInput = Input.GetAxis(inputSettings.CAM_Y_AXIS);
		forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
		strafeInput = Input.GetAxis(inputSettings.STRAFE_AXIS);
		jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS); // non-interpolated.
	}

	void Update()
	{
		GetInput();
		UpdateCursorLock();
	}
	
	void FixedUpdate()
	{
		Run();
		Strafe();
		Jump();
		Turn();
		CamYrotation();
		rb.velocity = transform.TransformDirection(velocity);
	}

	void Run()
	{
		if (Mathf.Abs(forwardInput) > inputSettings.inputDelay)
		{
			// move
			velocity.z = moveSettings.fowardVel * forwardInput;
		}
		else
		{
			// zero vel
			velocity.z = 0;
		}
		
	}

	void Strafe ()
	{
		if (Mathf.Abs(strafeInput) > inputSettings.inputDelay)
		{
			// move
			velocity.x = moveSettings.strafeVel * strafeInput;
		}
		else
		{
			// zero vel
			velocity.x = 0;
		}

	}

	void Turn()
	{
		if (m_cursorIsLocked)
		{
			float toRotate = turnInput * moveSettings.rotateVel;
			rotation = new Vector3(0f, toRotate, 0f);
			rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		}
	}

	void CamYrotation()
	{
		if(m_cursorIsLocked)
		{
            cameraRotation.x -= camHorizontalInput * moveSettings.rotateVel;
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -50, 50);
            camTransform.localEulerAngles = cameraRotation;
            /*
            float toRotate = camHorizontalInput * moveSettings.rotateVel;
            cameraRotation = new Vector3(Mathf.Clamp(toRotate, -50, 50),0f, 0f);
                //toRotate, 0f, 0f);
			camHingeY.transform.Rotate(-cameraRotation);
            */
		}
	}

	void Jump()
	{
		if(jumpInput > 0 && Grounded())
		{
			//jump
			velocity.y = moveSettings.jumpVel;
		}
		else if(jumpInput == 0 && Grounded())
		{
			// zero out our velocity.y
			velocity.y = 0;
		}
		else
		{
			//decrease vel.y
			velocity.y -= phycsSettings.downAccel;
		}
	}

	public void SetCursorLock (bool value)
	{
		inputSettings.lockCursor = value;
		if (!inputSettings.lockCursor)
		{//we force unlock the cursor if the user disable the cursor locking helper
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void UpdateCursorLock ()
	{
		//if the user set "lockCursor" we check & properly lock the cursos
		if (inputSettings.lockCursor)
			InternalLockUpdate();
	}

	private void InternalLockUpdate ()
	{
		if (Input.GetKeyUp(KeyCode.Escape ) || Input.GetKeyUp(KeyCode.Tab))
		{
			m_cursorIsLocked = !m_cursorIsLocked;
		}	

		if (m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (!m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
