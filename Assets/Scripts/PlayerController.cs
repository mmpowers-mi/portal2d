using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject portalOrangeObj;
	public GameObject portalBlueObj;

	bool grounded = false;
	bool touchingPortalBlue = false;
	bool touchingPortalOrange = false;
	public Transform groundCheck;
	public Transform portalCheck;
	float groundRadius = 0.1f;
	float portalRadius = 0.1f;
	public LayerMask whatIsGround;
	public LayerMask portalBlue;
	public LayerMask portalOrange;
	public Vector2 jumpHeight;
	private Rigidbody2D rb2d;

	public int portalOrangeDir = 1;
	public int portalBlueDir = 1;

	public float portalExitOffset = .2f;

	public float maxSpeed = 5f;
	public float deceleration = .8f;
	public float acceleration = .5f;
	public float airAccelMod = .8f;

	void Start()
    {
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		Physics2D.IgnoreLayerCollision(8, 9, true);
		//portalOrangeObj = GameObject.FindGameObjectWithTag("PortalOrange");
		//portalBlueObj = GameObject.FindGameObjectWithTag("PortalBlue");
	}

	void FixedUpdate()
    {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		touchingPortalBlue = Physics2D.OverlapCircle (portalCheck.position, portalRadius, portalBlue);
		touchingPortalOrange = Physics2D.OverlapCircle (portalCheck.position, portalRadius, portalOrange);

		if (touchingPortalBlue) {
			this.transform.position = portalOrangeObj.transform.position;
			Vector2 tempPosition = this.transform.position;
			if (portalOrangeDir == 0) {
				tempPosition.y = tempPosition.y + portalExitOffset;
			}

			if (portalOrangeDir == 1) {
				tempPosition.x = tempPosition.x + portalExitOffset;
			}

			if (portalOrangeDir == 2) {
				tempPosition.y = tempPosition.y - portalExitOffset;
			}

			if (portalOrangeDir == 3) {
				tempPosition.x = tempPosition.x - portalExitOffset;
			}
			this.transform.position = tempPosition;
		}
		if (touchingPortalOrange) {
			this.transform.position = portalBlueObj.transform.position;
			Vector2 tempPosition = this.transform.position;
			if (portalBlueDir == 0) {
				tempPosition.y = tempPosition.y + portalExitOffset;
			}

			if (portalBlueDir == 1) {
				tempPosition.x = tempPosition.x + portalExitOffset;
			}

			if (portalBlueDir == 2) {
				tempPosition.y = tempPosition.y - portalExitOffset;
			}

			if (portalBlueDir == 3) {
				tempPosition.x = tempPosition.x - portalExitOffset;
			}
			this.transform.position = tempPosition;
		}

		// NOTE: KeyCode needs to be replaced with input manager in later version.

		float velocity = rb2d.velocity.x;

		if (Input.GetKey(KeyCode.D)) {
			if ((!grounded) && (velocity > 0)) {
				velocity = velocity + (acceleration * airAccelMod);
			}
			else {
				velocity = velocity + acceleration;
			}
		}

		if (Input.GetKey(KeyCode.A)) {
			if ((!grounded) && (velocity < 0)) {
				velocity = velocity - (acceleration * airAccelMod);
			}
			else {
				velocity = velocity - acceleration;
			}
		}

		if (Mathf.Abs (velocity) > maxSpeed) {
			// Ternary operator: Google it.
			velocity = velocity > 0 ? maxSpeed : -maxSpeed;
		}

		rb2d.velocity = new Vector2(velocity, rb2d.velocity.y);

		if (!Input.GetKey(KeyCode.D) && (!Input.GetKey(KeyCode.A))) {
			float newVelocity = rb2d.velocity.x * deceleration;
			rb2d.velocity = new Vector2(newVelocity, rb2d.velocity.y);
		}

		if (Mathf.Abs (rb2d.velocity.x) < .001) {
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
		}

		Vector2 tempVelocity = rb2d.velocity;
		if (portalBlueDir == portalOrangeDir) {
			if (portalBlueDir == 0 || portalBlueDir == 2) {
				tempVelocity.y = tempVelocity.y * -1;
			}
			if (portalBlueDir == 1 || portalBlueDir == 3) {
				tempVelocity.x = tempVelocity.x * -1;
			}
		}
	}

	void Update()
	{
		if (grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))) {
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
	}
}
