using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// A Gun is a tool derived class. It has 3 functions which can be binded to controls: Shooting, Reloading, and Equipping.
/// Whenever it fires, it spawns a Projectile object which shoots towards wherever the mouse is at.
/// Note: There must be a gameObject in the Gun named Firepoint, which defines the muzzle of the gun from which the Projectiles spawn.
/// </summary>
public class Gun : Tool {
	public bool CanShoot = true;

	public Vector2 Direction = new Vector2();

	[Header("Stats")]
	/// <summary>
	/// How fast the gun is shooting, as defined in Rounds Per Minutes (RPM)
	/// </summary>
	public int RPM = 900;
	public float Speed = 120;
	public Projectile.Types BulletType = Projectile.Types.Fastcast;


	/// <summary>
	/// How much damage the gun does per shot.
	/// </summary>	
	public int Damage = 30;

	public Transform Firepoint;

	public virtual void Shoot() {
		if (CanShoot) {
			CanShoot = false;
			
			Projectile bullet = Projectile.Create(BulletType, Firepoint.position, Direction);
			bullet.Damage = Damage;
			bullet.Speed = Speed;

			SoundController.PlayClipAtPoint("Gunshot", transform.position).volume = .5F;

			bullet.Hit.Connect(
				(Transform other) => {
					if (other?.GetComponent<Character>()) {
						other.GetComponent<Character>().Health.Value -= bullet.Damage;
						bullet.Dispose();

						return false;
					}
					return true;
				}
			);
		}
	}

	public override void BindPlayerControls() {
		base.BindPlayerControls();

		Runservice.BindToFixedUpdate(Global.RunservicePriority.Heartbeat.Physics - 1, (float dt) => {
			if (InputController.Keyboard[InputController.GetKeyCode(PlayerPrefs.GetString("SHOOT"))].Value) {
				Shoot();
			}
			return true;
		});


	}


	public override void UnBindControls() {
		base.UnBindControls();
	}

	//Repeats shooting of the gun as long as binded key to shoot is held, and also makes the recoiling effect of the gun.
	private float dtCounter = 0;
	public void FixedUpdate() {
		dtCounter += Time.deltaTime;
		if (!CanShoot) {
			if (dtCounter > (double)(1 / ((double)RPM / 60))) {
				CanShoot = true;
				if (dtCounter != 0) {
					dtCounter = 0;
				}
			}
		}
	}

	public override void Awake() {
		base.Awake();
	}

	// Start is called before the first frame update
	public override void Start() {
		base.Start();

		Maid.GiveTask(delegate () {
			UnBindControls();
		});
	}
}
