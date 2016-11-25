using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.1f;
	public LayerMask whatIsGround;
	public Vector2 jumpHeight;
	private Rigidbody2D rb2d;

	public float maxSpeed = 5f;
	public float deceleration = .8f;
	public float acceleration = .5f;
	public float airAccelMod = .8f;

	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);

		//NOTE: KeyCode needs to be replaced with input manager in later version.

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
			//Ternary operator: Google it.
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
			
	}

	void Update()
	{
		if (grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))) {
            GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
        }
	}
}
