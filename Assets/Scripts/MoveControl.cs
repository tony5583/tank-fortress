﻿using UnityEngine;
using System.Collections;

public class MoveControl : MonoBehaviour {
	public float turnSpeed = 100.0f;
	public float driveSpeed = 100.0f;
	public float nitroForce = 500000.0f;
	
	public AudioSource nitroAudio;
	public AudioSource moveAudio;
	public AudioSource turnAudio;
	public float moveAudioFadeSpeed = 3.0f;
	public float turnAudioFadeSpeed = 9.0f;
	public float moveAudioFade = 0.0f;
	public float turnAudioFade = 0.0f;
	
	float turn = 0;
	float drive = 0;
	CannonController cannon;
	
	void Start() {
		cannon = gameObject.GetComponentInChildren<CannonController>();
	}
	
	public void UseNitro() {
		rigidbody.AddForce(transform.forward * nitroForce * cannon.energySlider.value, ForceMode.Impulse);
		print(gameObject.name + " UseNitro!");
		nitroAudio.Play();
	}
	
	public void SetTurn(float direction) {
		if (cannon.IsAiming()) {
			turn = turnSpeed * direction * 0.5f;
		} else {
			turn = turnSpeed * direction;
		}
	}
	
	public void SetDrive(float direction) {
		drive = driveSpeed * direction;
	}
	
	void UpdateMoveAudio() {
		bool play = drive != 0;
		if (play) {
			moveAudioFade = Mathf.Lerp(moveAudioFade, 1.0f, moveAudioFadeSpeed * Time.deltaTime);
		} else {
			moveAudioFade = Mathf.Lerp(moveAudioFade, 0.0f, moveAudioFadeSpeed * Time.deltaTime);
		}
		
		if (moveAudio.isPlaying && !play && moveAudioFade < 0.05f) {
			moveAudio.Stop();
			print ("Move audio Stop");
		}
		if (!moveAudio.isPlaying && play) {
			moveAudio.Play();
			print ("Move audio Start");
		}
		
		if (moveAudio.isPlaying) {
			moveAudio.volume = moveAudioFade;
			moveAudio.pitch = moveAudioFade;
		}
	}
	
	void UpdateTurnAudio() {
		bool play = turn != 0 && drive == 0;
		if (play) {
			turnAudioFade = Mathf.Lerp(turnAudioFade, 1.0f, turnAudioFadeSpeed * Time.deltaTime);
		} else {
			turnAudioFade = Mathf.Lerp(turnAudioFade, 0.0f, turnAudioFadeSpeed * Time.deltaTime);
		}
		
		if (turnAudio.isPlaying && !play && turnAudioFade < 0.05f) {
			turnAudio.Stop();
			print ("Turn audio Stop");
		}
		if (!turnAudio.isPlaying && play) {
			turnAudio.Play();
			print ("Turn audio Start");
		}
		
		if (turnAudio.isPlaying) {
			turnAudio.volume = turnAudioFade;
			turnAudio.pitch = turnAudioFade;
		}
	}
	
	void Update() {
		UpdateMoveAudio();
		UpdateTurnAudio();
		
		if (turn != 0.0f) {
			transform.RotateAround(transform.position, transform.up, turn * Time.deltaTime);
		}
	}
	
	void FixedUpdate() {
		if (drive != 0.0f) {
			rigidbody.AddForce(transform.forward * drive * (turn != 0 ? 0.6f : 1), ForceMode.Force);
		}
	}
}




