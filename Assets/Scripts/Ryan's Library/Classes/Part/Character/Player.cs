using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	[Header("Movement Parameters")]
	public float moveSpeed = 5f;
	Vector2 movement;


	[Header("Tool Parameters")]
	public Tool CurrentTool;
	public float ToolDistanceRadius = 1.5F;
	public Vector2 TargetPosition;
	private float org_ToolDistanceRadius;

	public override void BindPlayerControls() {
		base.BindPlayerControls();
		CurrentTool?.BindPlayerControls();
	}

	// Update is called once per frame
	public void Update() {
		// handles input
		if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("RIGHT"))].Value) {
			movement.x = 1;
		} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("LEFT"))].Value) {
			movement.x = -1;
		} else {
			movement.x = 0;
		}

		if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("UP"))].Value) {
			movement.y = 1;
		} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("DOWN"))].Value) {
			movement.y = -1;
		} else {
			movement.y = 0;
		}

		Animator.SetFloat("Horizontal", movement.x);
		Animator.SetFloat("Vertical", movement.y);
		Animator.SetFloat("Speed", movement.sqrMagnitude);
	}

	// Fixed update updates on a fixed timer (better for physics)
	public void FixedUpdate() {
		Health.Call();
		// handles movement
		Rigidbody.MovePosition(Rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
		//Adjusts tool to be a certain distance or less away from character.
		if (CurrentTool) {
			TargetPosition = InputController.Mouse.Position;
			
			Vector3 lookVec = TargetPosition.AsVector3() - transform.position;
			if (CurrentTool as Gun) {
				((Gun)CurrentTool).Direction = lookVec.normalized;
			}

			ToolDistanceRadius = Mathf.Clamp(lookVec.magnitude, .1F, org_ToolDistanceRadius);

			CurrentTool.transform.position = transform.position + (lookVec.normalized * ToolDistanceRadius);
			CurrentTool.transform.right = lookVec;
		}
	}

	public override void Awake() {
		base.Awake();

		org_ToolDistanceRadius = ToolDistanceRadius;
	}
	
	public override void Start() {
		base.Start();
	}
  
	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	public void OnDrawGizmos() {
		Extensions.DrawGizmoCircle(transform, ToolDistanceRadius, Color.blue);
		Extensions.DrawArrow(transform.position, (TargetPosition.AsVector3() - transform.position).normalized * ToolDistanceRadius * 1.1F);
	}
}
