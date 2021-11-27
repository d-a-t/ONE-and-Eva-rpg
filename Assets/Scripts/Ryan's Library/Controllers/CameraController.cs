using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// honestly this class may not be used. Originally it's used to switch between Cameras on the fly, but we don't really have that.
/// </summary>
public class CameraController : Singleton {
	public static CameraController Singleton;
	public Variable<Camera> Cam = new Variable<Camera>();
	public Canvas GUI;
	public Variable<Transform> Target = new Variable<Transform>();
	public float MovementFactor = 4F;
	public void Awake() {
		if (Singleton == null) {
			Singleton = this;
		}
	}

	public void Start() {
		Cam.Value.nearClipPlane = Config.Player.Camera.NearClipPlane;
		Cam.Value.fieldOfView = Config.Player.Camera.FOV;
	}

	void LateUpdate() {
		Vector2 midPoint = (InputController.Mouse.Position - Target.Value.position.AsVector2())/ MovementFactor;
		Vector3 midPoint_as_V3 = new Vector3(midPoint.x, midPoint.y, Camera.main.transform.position.z);

		Camera.main.transform.position = Target.Value.position + midPoint_as_V3;
	}

}
