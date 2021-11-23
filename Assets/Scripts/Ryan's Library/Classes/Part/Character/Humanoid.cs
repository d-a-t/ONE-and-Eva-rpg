using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : Walkable {

	// Update is called once per frame
	public void Update() {
		Animator.SetFloat("Horizontal", WalkDirection.x);
		Animator.SetFloat("Vertical", WalkDirection.y);
		Animator.SetFloat("Speed", WalkDirection.sqrMagnitude);
	}

	public override void Awake() {
		base.Awake();
	}
	
	public override void Start() {
		base.Start();
	}
}
