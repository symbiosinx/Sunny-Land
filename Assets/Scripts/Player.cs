using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class Player : MonoBehaviour {

	public float gravity = -9.81f;
	public float moveSpeed = 10f;
	public float jumpSpeed = 10f;
	public float centerRadius = 0.1f;

	Animator anim;
	SpriteRenderer rend;
	CharacterController2D controller;

	bool jumped;
	bool climbing;
	Vector3 velocity;

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, centerRadius);
	}

	void Start() {

		anim = GetComponent<Animator>();
		rend = GetComponent<SpriteRenderer>();
		controller = GetComponent<CharacterController2D>();
	}

	void Update() {

		// Get horizontal input
		float inputH = Input.GetAxis("Horizontal");
		// Get vertical input
		float inputV = Input.GetAxis("Vertical");
		
		bool jumping = Input.GetButtonDown("Jump");


		if (controller.isGrounded) {
			//velocity.y = 0f;
			jumped = false;
			if (jumping) {
				jumped = true;
				Jump();
			}
		} else if (!climbing) {		
			velocity.y += gravity * (velocity.y < 0f && jumped ? 2f : 1f) * Time.deltaTime;
		}

		anim.SetBool("Grounded", controller.isGrounded);
		anim.SetFloat("JumpY", Mathf.Clamp(velocity.y, -1f, 1f));

		Move(inputH);
		Climb(inputH, inputV);

		if (!climbing) {
			// move the character left and right
			controller.move(velocity * Time.deltaTime);
		}

	}

	void Move(float inputH) {

		// set the horizontal component of velocity to the direction we are moving with moveSpeed
		velocity.x = inputH * moveSpeed;
		// set the running animation on if moving
		anim.SetBool("IsRunning", inputH != 0);
		// if moving
		if (inputH != 0) {
			// flip the spriterenderer to face the other direction
			rend.flipX = inputH < 0;
		}
	}

	void Climb(float inputH, float inputV) {
		// is overlapping ladder
		bool overLadder = false;		

		// list of all hit objects overlapping point
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centerRadius);
		// loop through each point
		foreach (Collider2D hit in hits) {
			// if player overlaps ladder
			if (hit.tag == "Ladder") {
				// player is overlapping ladder
				overLadder = true;
				break;
			}
		}

		// if vertical input is not 0 and player is over ladder
		if (inputV != 0f && overLadder) {
			// set climbing to true
			climbing = true;
		}
		if (!overLadder) {
			climbing = false;
		}
		// if climbing
		if (climbing) {
			// climbing logic
			// translate character
			Vector3 inputDir = new Vector3(inputH, inputV);
			transform.Translate(inputDir * moveSpeed * Time.deltaTime);

		}
		anim.SetBool("IsClimbing", climbing);
		anim.SetFloat("ClimbSpeed", inputV);

	}

	void Jump() {
		// Set the vertical component of the velocity to the jump speed
		velocity.y = jumpSpeed;
	}
}
