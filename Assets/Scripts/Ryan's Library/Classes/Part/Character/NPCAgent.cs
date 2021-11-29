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
	void LateUpdate() {
		Agent.SetDestination(Target.Value.position);
		Character.Animator.SetFloat("Horizontal", Agent.desiredVelocity.x);
		Character.Animator.SetFloat("Vertical", -Agent.desiredVelocity.y);
		Character.Animator.SetFloat("Speed", Agent.speed);
		if (gameObject.GetComponent<PlayerForwardAttack>()) {
			PlayerForwardAttack bruh = gameObject.GetComponent<PlayerForwardAttack>();
			if ((Agent.transform.position - Target.Value.position).magnitude < 3) {
				bruh.Direction = (Agent.transform.position - Target.Value.position).AsVector2();
				bruh.Attack();
			}
		}
	}
}
