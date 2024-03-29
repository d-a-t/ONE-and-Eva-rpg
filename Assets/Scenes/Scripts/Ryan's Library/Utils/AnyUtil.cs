using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Little things I've added onto Unity's classes to make it easier to do some stuff.
/// </summary>
public static class Extensions {
	/// <summary>
	/// Function that finds a child with the defined name through a deep search. Don't use this often since it's intensive.
	/// </summary>
	/// <param name="aParent">The gameObject which you're trying to find the child in.</param>
	/// <param name="aName">The string name of the child.</param>
	/// <returns></returns>
	public static Transform FindDeepChild(this Transform aParent, string aName) {
		Queue<Transform> queue = new Queue<Transform>();
		queue.Enqueue(aParent);
		while (queue.Count > 0) {
			var c = queue.Dequeue();
			if (c.name == aName)
				return c;
			foreach (Transform t in c)
				queue.Enqueue(t);
		}
		return null;
	}

	/// <summary>
	/// Function returns CFrame from a transform.
	/// </summary>
	/// <param name="obj">The Transform object.</param>
	/// <returns></returns>
	public static CFrame GetCFrame(this Transform obj) {
		return new CFrame(obj);
	}

	/// <summary>
	/// Updates position and rotation of a Transform using a cframe.
	/// </summary>
	/// <param name="aParent">The gTransform being updated.</param>
	/// <returns></returns>
	public static void UpdateFromCFrame(this Transform aParent, CFrame cframe) {
		aParent.position = cframe.p;
		aParent.rotation = (cframe - cframe.p).ToQuaternion();
	}

	/// <summary>
	/// Changes the Vector3 representation as a Vector2 representation, with the z axis ignored. (x,y,z) -> (x,y).
	/// </summary>
	/// <param name="_v"></param>
	/// <returns></returns>
	public static Vector2 AsVector2(this Vector3 _v) {
		return new Vector2(_v.x, _v.y);
	}

	/// <summary>
	/// Changes the Vector2 representation as a Vector3 representation, with the z axis defaulted to 0. (x,y) -> (x,y,0).
	/// </summary>
	/// <param name="_v"></param>
	/// <returns></returns>
	public static Vector3 AsVector3(this Vector2 _v) {
		return new Vector3(_v.x, _v.y, 0);
	}

	/*
	//Depth-first search
	public static Transform FindDeepChild(this Transform aParent, string aName)
	{
		foreach(Transform child in aParent)
		{
			if(child.name == aName )
				return child;
			var result = child.FindDeepChild(aName);
			if (result != null)
				return result;
		}
		return null;
	}
	*/

	/// <summary>
	/// Creates an audio source gameObject at a point which will destroy itself after the audio clip finishes playing.
	/// </summary>
	/// <param name="audioSource"></param>
	/// <param name="pos"></param>
	/// <returns></returns>
	public static AudioSource PlayClipAtPoint(this AudioSource audioSource, Vector3 pos) {
		GameObject tempGO = new GameObject("TempAudio"); // create the temp object
		tempGO.transform.position = pos; // set its position
		AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
		tempASource.clip = audioSource.clip;
		tempASource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
		tempASource.mute = audioSource.mute;
		tempASource.bypassEffects = audioSource.bypassEffects;
		tempASource.bypassListenerEffects = audioSource.bypassListenerEffects;
		tempASource.bypassReverbZones = audioSource.bypassReverbZones;
		tempASource.playOnAwake = audioSource.playOnAwake;
		tempASource.loop = audioSource.loop;
		tempASource.priority = audioSource.priority;
		tempASource.volume = audioSource.volume;
		tempASource.pitch = audioSource.pitch;
		tempASource.panStereo = audioSource.panStereo;
		tempASource.spatialBlend = audioSource.spatialBlend;
		tempASource.reverbZoneMix = audioSource.reverbZoneMix;
		tempASource.dopplerLevel = audioSource.dopplerLevel;
		tempASource.rolloffMode = audioSource.rolloffMode;
		tempASource.minDistance = audioSource.minDistance;
		tempASource.spread = audioSource.spread;
		tempASource.maxDistance = audioSource.maxDistance;
		// set other aSource properties here, if desired
		tempASource.Play(); // start the sound
		MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
		return tempASource; // return the AudioSource reference
	}

	private const float GIZMO_DISK_THICKNESS = 0.05f;
	public static void DrawGizmoCircle(Transform transform, float radius, Color color) {
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.color = color;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation * Quaternion.Euler(90,0,0), new Vector3(1, GIZMO_DISK_THICKNESS, 1));
		Gizmos.DrawWireSphere(Vector3.zero, radius);
		Gizmos.matrix = oldMatrix;
	}

	public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
		Gizmos.DrawRay(pos, direction);

		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0 , 0) * new Vector3(0, 0, 1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
		Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
		Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
	}

	public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
		Gizmos.color = color;
		Gizmos.DrawRay(pos, direction);

		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0 , 0) * new Vector3(0, 0, 1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
		Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
		Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
	}
}