using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Walkable {


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
			WalkDirection.x = 1;
		} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("LEFT"))].Value) {
			WalkDirection.x = -1;
		} else {
			WalkDirection.x = 0;
		}

		if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("UP"))].Value) {
			WalkDirection.y = 1;
		} else if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("DOWN"))].Value) {
			WalkDirection.y = -1;
		} else {
			WalkDirection.y = 0;
		}

		Animator.SetFloat("Horizontal", WalkDirection.x);
		Animator.SetFloat("Vertical", WalkDirection.y);
		Animator.SetFloat("Speed", WalkDirection.sqrMagnitude);
	}

	// Fixed update updates on a fixed timer (better for physics)
	public void FixedUpdate() {
		Health.Call();
		// handles WalkDirection
		Rigidbody.MovePosition(Rigidbody.position + WalkDirection * CurrentSpeed.Value * Time.fixedDeltaTime);
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
