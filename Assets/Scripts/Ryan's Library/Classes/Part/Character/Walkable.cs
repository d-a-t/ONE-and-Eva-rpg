using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : Character {

	[Header("Movement Parameters")]
	public float WalkSpeed = 5f;
	public float RunSpeed = 10F;
	public float JumpPower = 10F;
	public Vector2 WalkDirection = new Vector2(0, 0);

	protected Variable<float> CurrentSpeed = new Variable<float>();

	public override void BindPlayerControls() {
		base.Start();
		
		//Binds the RIGHT key to make the character walk to the right.
		ControlMaid.GiveTask(
			InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("RIGHT"))].Connect(
				(bool val) => {
					if (val) {
						WalkDirection.x = 1;
					} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("LEFT"))].Value) {
						WalkDirection.x = -1;
					} else {
						WalkDirection.x = 0;
					}
					return true;
				}
			)
		);

		//Binds the LEFT key to make the character walk to the left.
		ControlMaid.GiveTask(
			InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("LEFT"))].Connect(
				(bool val) => {
					if (val) {
						WalkDirection.x = -1;
					} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("RIGHT"))].Value) {
						WalkDirection.x = 1;
					} else {
						WalkDirection.x = 0;
					}
					return true;
				}
			)
		);

		//Binds the UP key to make the character walk up.
		ControlMaid.GiveTask(
			InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("UP"))].Connect(
				(bool val) => {
					if (val) {
						WalkDirection.y = 1;
					} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("DOWN"))].Value) {
						WalkDirection.y = -1;
					} else {
						WalkDirection.y = 0;
					}
					return true;
				}
			)
		);
		
		//Binds the DOWN key to make the character walk down.
		ControlMaid.GiveTask(
			InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("DOWN"))].Connect(
				(bool val) => {
					if (val) {
						WalkDirection.y = -1;
					} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("UP"))].Value) {
						WalkDirection.y = 1;
					} else {
						WalkDirection.y = 0;
					}
					return true;
				}
			)
		);

		//This connects the RUN button to make the character run. This is a hold, not toggle.
		ControlMaid.GiveTask(
			InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("RUN"))].Connect(
				(bool val) => {
					if (val) {
						CurrentSpeed.Value = RunSpeed;
					} else {
						CurrentSpeed.Value = WalkSpeed;
					}
					return true;
				}
			)
		);
	}

	public virtual void FixedUpdate() {
		Rigidbody.MovePosition(Rigidbody.position + WalkDirection * CurrentSpeed.Value * Time.fixedDeltaTime);
	}

	public override void Awake() {
		base.Start();
		CurrentSpeed.Value = WalkSpeed;
	}

}