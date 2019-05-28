using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class Player : MonoBehaviour {

	public float moveSpeed = 10f;
	public float gravity = -9.81f;

	CharacterController2D controller;
	Animator anim;

    void Start() {

		controller = GetComponent<CharacterController2D>();
		anim = GetComponent<Animator>();
    }

    void Update() {

		float inputH = Input.GetAxis("Horizontal");
		float inputV = Input.GetAxis("Vertical");
		Move(inputH);
		Climb(inputV);
	}

	void Move(float inputH) {
		controller.move(transform.right * inputH * moveSpeed * Time.deltaTime);
		anim.SetBool("IsRunning", inputH != 0);
	}

	void Climb(float inputV) {

	}
}
