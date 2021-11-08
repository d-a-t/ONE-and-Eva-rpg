using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Tool {
	public bool CanShoot = true;
	public Vector2 Direction = new Vector2();

	[Header("Laser")]
	public Hitbox LaserHitbox;
	public SpriteRenderer WarningSprite;
	public LineRenderer LaserRenderer;

	[Header("Laser Parameters")]
	public float WarningTimer = 1F;
	public float LaserDuration = 1F;
	public float Cooldown = 4F;
	public float Damage = 30;

	private float CooldownTimer = 0;

	private Maid LaserMaid = new Maid();

	public virtual void Shoot() {
		if (CanShoot) {
			LaserMaid.Dispose();

			CanShoot = false;
			CooldownTimer = 0;

			WarningSprite.enabled = true;

			SoundController.PlayClipAtPoint("Lasercharge", transform.position);

			Listener<float> flashingRedWarning = Runservice.RunEvery(Global.RunservicePriority.Heartbeat.Physics, .1F,
				(float dt) => {
					WarningSprite.color = Color.red;
					Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, .05F,
						delegate {
							WarningSprite.color = Color.white;
						}
					);
					return true;
				}
			);
			LaserMaid.GiveTask(flashingRedWarning);

			LaserMaid.GiveTask(
				Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, WarningTimer, 
					delegate {
						SoundController.PlayClipAtPoint("Lasershoot", transform.position);

						flashingRedWarning.Destroy();
						WarningSprite.enabled = false;

						LaserRenderer.enabled = true;
						LaserHitbox.Enabled = true;

						LaserMaid.GiveTask(
							LaserHitbox.Hit.Connect(
								(Collider2D otherhit) => {
									var other = otherhit?.transform;
									if (other?.GetComponent<Character>()) {
										other.GetComponent<Character>().Health.Value -= Damage;
										return false;
									}
									return true;
								}
							)
						);


						LaserMaid.GiveTask(
							Runservice.RunAfter(Global.RunservicePriority.Heartbeat.Physics, LaserDuration, 
								delegate {
									LaserRenderer.enabled = false;
									LaserHitbox.Enabled = false;
									LaserHitbox.Hit.Value = null;
								}
							)
						);
					}
				)
			);
		}
	}

	public void FixedUpdate() {
		if (CooldownTimer < Cooldown) {
			CooldownTimer += Time.deltaTime;
		}
		if (CooldownTimer > Cooldown && !CanShoot) {
			CanShoot = true;
		}
	}

	public override void Awake() {
		base.Awake();
		LaserRenderer.enabled = false;
		LaserHitbox.Enabled = false;
	}

	public override void Start() {
		LaserRenderer.enabled = false;
		LaserHitbox.Enabled = false;

		Maid.GiveTask(LaserMaid);
	}
}
