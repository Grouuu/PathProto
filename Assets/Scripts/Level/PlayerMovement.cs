using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Gravity))]
[RequireComponent(typeof(PlayerPath))]
public class PlayerMovement : MonoBehaviour
{
	public Vector3 startVelocity;
	public float speedAcc = .5f;
	public float speedDec = .5f;
	public float minSpeed = .5f;
	public float maxSpeed = 50f;
	public float speedRotate = 1f;
	public float boostStrength = 1f;
	public float mass = 1f;
	public bool ignoreGravity = false;
	public bool ignorePath = false;

	private Rigidbody rb;
	private Gravity g;
	private PlayerPath path;

	private Vector3 velocity;

	[HideInInspector]
	public bool isBoosting = false;

	// SETUP ------------------------------------------------------------------

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		g = GetComponent<Gravity>();
		path = GetComponent<PlayerPath>();
	}

	void Start()
    {
		rb.mass = mass;
		rb.constraints =
			RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;

		velocity = startVelocity;
	}

	// PUBLIC -----------------------------------------------------------------

	public void AcceleratePlayer()
	{
		velocity += transform.forward * speedAcc;
		velocity = LimitPlayerSpeed(velocity);
	}

	public void DeceleratePlayer()
	{
		Vector3 force = -transform.forward * speedDec;

		if (force.magnitude + minSpeed > velocity.magnitude) // no backward + avoid stop (minSpeed)
			force = Vector3.zero;

		velocity += force;
		velocity = LimitPlayerSpeed(velocity);
	}

	public void RotatePlayer(int dir)
	{
		// calculate rotation, rotate player and velocity
		Quaternion rotate = Quaternion.Euler(new Vector3(0, 0, speedRotate * dir));
		rb.rotation = rotate * rb.rotation;
		velocity = rotate * velocity;
	}

	public void BoostPlayer()
	{
		if (!isBoosting)
		{
			velocity += velocity.normalized * boostStrength;

			Debug.Log("BOOST");
		}

		isBoosting = true;
	}

	public Vector3 LimitPlayerSpeed(Vector3 vel)
	{
		// limit velocity
		return Vector3.ClampMagnitude(vel, isBoosting ? maxSpeed + boostStrength : maxSpeed);
	}

	// UPDATE -----------------------------------------------------------------

	// Don't use AddForce because of the calculation to trace the path (need accurate current velocity)
	private void FixedUpdate()
	{
		float deltaTime = Time.fixedDeltaTime;

		if (!ignoreGravity)
		{
			velocity += g.GetGravityForce(rb.position, mass) * deltaTime; // add gravity to velocity
			velocity = LimitPlayerSpeed(velocity); // NOTE : can cause difference with path ? (path don't use this)

			if (velocity.normalized != Vector3.zero) // avoid warning with rotation == 0
				rb.rotation = Quaternion.LookRotation(velocity.normalized); // player face the new velocity direction
		}

		rb.position += velocity * deltaTime;

		Debug.Log(velocity);

		if (!ignorePath)
			path.DrawPath(velocity, Time.deltaTime);
	}

	private void Update()
	{
		// NOTE : cause shakings here, better in FixedUpdate but much expansive
		/*if (!ignorePath)
			path.DrawPath(velocity, Time.deltaTime);*/
	}

	// TOOLS ------------------------------------------------------------------

	public float GetPlayerSpeed()
	{
		return velocity.magnitude;
	}
}
