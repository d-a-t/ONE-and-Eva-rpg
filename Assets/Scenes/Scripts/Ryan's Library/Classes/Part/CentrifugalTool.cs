using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentrifugalTool : Tool {
	public float Radius = 1.5F;
	public Vector2 LookAt;
	public Vector3 upVector = new Vector3(1, 0, 0);

	private float orgRadius;
	public override void Awake() {
		base.Awake();

		orgRadius = Radius;
	}

	public override void Start() {
		//Updates tool position to be at a distance away from center.
		Vector3 center = new Vector3(0, 0, 0);
		Maid.GiveTask(
			Runservice.BindToUpdate(Global.RunservicePriority.RenderStep.Last, 
				(float dt) => {
					LookAt = InputController.Mouse.Position;
					Vector3 lookVec = LookAt.AsVector3() - Controller.position;

					Radius = Mathf.Clamp(lookVec.magnitude, .1F, orgRadius);

					transform.position = Controller.position + (lookVec.normalized * Radius);
					transform.right = lookVec;
					return true;
				}
			)
		);
	}
	
	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	public void OnDrawGizmos() {
		if (Controller) {
			Extensions.DrawGizmoCircle(Controller, Radius, Color.blue);
			Extensions.DrawArrow(Controller.position, (LookAt.AsVector3() - Controller.position).normalized * Radius * 1.1F);
		}
	}

}
