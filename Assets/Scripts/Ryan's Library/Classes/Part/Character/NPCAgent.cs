using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAgent : MonoBehaviour {
	public NavMeshAgent Agent;
	public Variable<Transform> Target = new Variable<Transform>();
	public Walkable Character;

	// Start is called before the first frame update
	void Start() {
		Agent.updateRotation = false;
		Agent.updateUpAxis = false;
		Agent.speed = Character.WalkSpeed;
	}

	// Update is called once per frame
	void Update() {
		Agent.SetDestination(Target.Value.position);
	}
}
