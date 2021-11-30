using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForwardAttack : MonoBehaviour {
	public Animator Animator;
	public LayerMask EnemyLayers;

	public Hitbox HitboxCollider;

	public float Damage = 10F;
	public float Cooldown = .5F;
	public float Knockback = 10F;
	public Vector2 Direction = new Vector2();
	public bool IsPlayer = true;
	private bool CanDash = true;
	
	private void Start() {
		if (IsPlayer) {
			InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("ATTACK", "Space"))].Connect(
				(bool val) => {
					if (this) {
						Direction = transform.position.AsVector2() - InputController.Mouse.Position;
						Attack();
					} else {
						return false;
					}
					return this;
				}
			);
		}
		HitboxCollider.Hit.Connect(
			(Collider2D val) => {
				if (val) {
					if (val.GetComponent<Character>()) {
						if (val.GetComponent<Rigidbody2D>()) {
							val.GetComponent<Rigidbody2D>().AddForce(	(val.transform.position - transform.position).normalized * Knockback	);
						}
						val.GetComponent<Character>().Health.Value -= Damage;
					}
				}
				return true;
			}
		);
		HitboxCollider.gameObject.SetActive(false);
	}

	public void Attack() {
		if (CanDash) {
			CanDash = false;
			//Cooldown, sets able to attack again after cooldown
			Runservice.RunAfter(0, Cooldown,
				delegate {
					CanDash = true;
				}
			);
			// Play an attack animation
			Animator.SetTrigger("Attack");

			HitboxCollider.transform.position = transform.position;
			HitboxCollider.transform.eulerAngles = new Vector3(HitboxCollider.transform.eulerAngles.x, HitboxCollider.transform.eulerAngles.y, (Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg) + 90);
			HitboxCollider.gameObject.SetActive(true);

			Runservice.RunAfter(0, .1F,
				delegate {
					if (HitboxCollider) {
						HitboxCollider.gameObject.SetActive(false);
					}
				}
			);
		}
	}
}
