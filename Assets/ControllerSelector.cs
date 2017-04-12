﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ControllerSelector : MonoBehaviour {
	public int controller_num;
	bool controller_init;
	InputDevice controller;
	float move_speed = 500f;
	Vector3 pos;
	Vector3 lock_pos;
	Vector3 quadrant;
	Vector2 x_border;
	Vector2 y_border;
	float width, height;
	bool lock_quad; 
	bool center = true;
	int choice = -1;
	// Use this for initialization
	void Start () {
		pos = transform.localPosition;
		x_border = new Vector2 (-400f, 400f);
		y_border = new Vector2 (-250f, 220f);
		width = x_border.y - x_border.x;
		height = y_border.y - y_border.x;
	}
	
	// Update is called once per frame
	void Update () {
		TryInitController ();
		TakeInput ();
		MoveToCenter ();
		LockPos ();
	}
	void TryInitController(){
		controller_init = false;
		if (InputManager.Devices.Count > controller_num) {
			controller = InputManager.Devices [controller_num];
			controller_init = true;
		}
	}
	void TakeInput(){
		if (controller_init && controller.Action2) {
			if (choice != -1) {
				center = true;
				lock_quad = false;
				Controllers.instance.choices [choice] = false;
				Controllers.instance.choice_nums [controller_num] = -1;
				choice = -1;
			}

		}
		if (controller_init && controller.MenuWasPressed) {
			Controllers.instance.LockInChoices ();
		}
		if (!center)
			return;
		if(controller_init && transform.localPosition.x + controller.LeftStickX * move_speed * Time.deltaTime > x_border.x && transform.localPosition.x + controller.LeftStickX * move_speed * Time.deltaTime < x_border.y) transform.localPosition = new Vector3 (transform.localPosition.x + controller.LeftStickX * move_speed * Time.deltaTime, transform.localPosition.y, 0f);
		if(controller_init && transform.localPosition.y + controller.LeftStickY * move_speed * Time.deltaTime > y_border.x && transform.localPosition.y + controller.LeftStickY * move_speed * Time.deltaTime < y_border.y)  transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + controller.LeftStickY *  move_speed * Time.deltaTime, 0f);
		if (controller_init && controller.Action1) {
			GetQuadrant ();
		}
	}
	void GetQuadrant(){
		if (choice != -1)
			return;
		if (transform.localPosition.x - pos.x < 0.1f && transform.localPosition.x - pos.x > -0.1f && transform.localPosition.y - pos.y < 0.1f && transform.localPosition.y - pos.y > -0.1f)
			return;
		if (transform.localPosition.x >= x_border.x && transform.localPosition.x <= x_border.x + width / 2) {
			if(transform.localPosition.y >= y_border.x + height / 2 && transform.localPosition.y <= y_border.y){

				if (Controllers.instance.choices [0])
					return;
				Controllers.instance.choices [0] = true;
				Controllers.instance.choice_nums [0] = controller_num;
				center = false;
				lock_quad = true;
				lock_pos = new Vector3 (x_border.x + width / 4, y_border.y - height / 3);
				choice = 0;
			}

			if(transform.localPosition.y >= y_border.x && transform.localPosition.y <= y_border.y - height / 2){

				if (Controllers.instance.choices [1])
					return;
				Controllers.instance.choices [1] = true;
				Controllers.instance.choice_nums [1] = controller_num;

				center = false;
				lock_quad = true;
				lock_pos = new Vector3 (x_border.x + width / 4, y_border.x + height / 3);
				choice = 1;
			}
		}
			
		else if (transform.localPosition.x >= x_border.x + width / 2 && transform.localPosition.x <= x_border.y) {
			if(transform.localPosition.y >= y_border.x && transform.localPosition.y <= y_border.y - height / 2){

				if (Controllers.instance.choices [3])
					return;
				Controllers.instance.choices [3] = true;
				Controllers.instance.choice_nums [3] = controller_num;

				center = false;
				lock_quad = true;
				lock_pos = new Vector3 (x_border.y - width / 4, y_border.x + height / 3);
				choice = 3;
			}
			if(transform.localPosition.y >= y_border.x + height / 2 && transform.localPosition.y <= y_border.y){

				if (Controllers.instance.choices [2])
					return;
				Controllers.instance.choices [2] = true;
				Controllers.instance.choice_nums [2] = controller_num;

				center = false;
				lock_quad = true;
				lock_pos = new Vector3 (x_border.y - width / 4, y_border.y - height / 3);
				choice = 2;
			}
		}


	}
	void MoveToCenter(){
		if (!controller_init || !center)
			return;
		if (controller.LeftStickX == 0) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(pos.x, transform.localPosition.y, transform.localPosition.z), 0.075f);
		}
		if (controller.LeftStickY == 0) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(transform.localPosition.x, pos.y, transform.localPosition.z), 0.075f);
		}

	}
	void LockPos(){
		if (!lock_quad)
			return;
		transform.localPosition = Vector3.Lerp (transform.localPosition, lock_pos, 0.15f);
		if (transform.localPosition.x - lock_pos.x < 0.15f && transform.localPosition.x - lock_pos.x > -0.15f) {
			if (transform.localPosition.y - lock_pos.y < 0.15f && transform.localPosition.y - lock_pos.y > -0.15f) {
				lock_quad = false;
				transform.localPosition = lock_pos;
			}
		}

	}
}
