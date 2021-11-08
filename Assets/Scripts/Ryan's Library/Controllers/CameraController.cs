using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// honestly this class may not be used. Originally it's used to switch between Cameras on the fly, but we don't really have that.
/// </summary>
public class CameraController : Singleton {
	public static CameraController Singleton;

	public Variable<Camera> Cam = new Variable<Camera>();
	public Vector2 FollowOffset;
	public float FollowSpeed = 3f;

	public Canvas GUI;

	public Variable<Transform> Target = new Variable<Transform>();

	private Vector2 threshold;

	public void Awake() {
		if (Singleton == null) {
			Singleton = this;
		}
	}

	public void Start() {
		Cam.Value.nearClipPlane = Config.Player.Camera.NearClipPlane;
		Cam.Value.fieldOfView = Config.Player.Camera.FOV;

		threshold = CalculateThreshold();
	}

	void LateUpdate() {
		Vector2 follow = Target.Value.position.AsVector2();
		float xDifference = Vector2.Distance(Vector2.right * Cam.Value.transform.position.x, Vector2.right * follow.x);
		float yDifference = Vector2.Distance(Vector2.up * Cam.Value.transform.position.y, Vector2.up * follow.y);

		Vector3 newPosition = Cam.Value.transform.position;
		if (Mathf.Abs(xDifference) >= threshold.x) {
			newPosition.x = follow.x;
		}
		if (Mathf.Abs(yDifference) >= threshold.y) {
			newPosition.y = follow.y;
		}
		
		//float moveSpeed = Target.Value.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > FollowSpeed ? Target.Value.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude : FollowSpeed;
		Cam.Value.transform.position = Vector3.Lerp(Cam.Value.transform.position, newPosition, Time.deltaTime);
	}

	private Vector3 CalculateThreshold() {
		Rect aspect = Cam.Value.pixelRect;
		Vector2 t = new Vector2(Cam.Value.orthographicSize * aspect.width / aspect.height, Cam.Value.orthographicSize);
		t.x -= FollowOffset.x;
		t.y -= FollowOffset.y;
		return t;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Vector2 border = CalculateThreshold();
		Gizmos.DrawWireCube(Cam.Value.transform.position, new Vector3(border.x * 2, border.y * 2, 1));
	}
}
