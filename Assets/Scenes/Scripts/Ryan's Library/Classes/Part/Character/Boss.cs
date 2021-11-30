using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Character {
	public int CurrentStage = 0;

	[Header("Boss Parameters")]
	public Transform Target;

	[Header("Gun")]
	public Gun GunTurret;
	public float GunTurretSpeed = 1.5F;

	[Header("Gun Array")]
	public List<Gun> GunArray;
	public float GunArraySpeed = 60F;

	[Header("Turrets")]
	public List<Gun> Turrets;
	public float TurretSpeed = 40F;
	public float Interval = 2F;

	[Header("Cannons")]
	public List<Gun> Cannons;
	public float CannonSpeed = 60;

	[Header("Lasers")]
	public List<LaserGun> Lasers;
	public float LaserTurretSpeed = 15;

	[Header("WallLasers")]
	public List<LaserGun> WallLasers;
	private List<Vector3> orgWallLaserPos = new List<Vector3>();
	private List<bool> wallLaserGoUp = new List<bool>();
	public float WallLaserSpeed = 60F;

	public void ChangeMusic(string music) {
		if (SoundController.Singleton.BackgroundMusic) {
			SoundController.Singleton.BackgroundMusic.Stop();
		}
		SoundController.Singleton.BackgroundMusic = SoundController.Singleton.Musics[music];
		if (!SoundController.Singleton.BackgroundMusic.isPlaying) {
			SoundController.Singleton.BackgroundMusic.Play();
		}
	}

	public void SwitchStage(int index) {
		CurrentStage = index;
		if (CurrentStage == 3) {
			Maid.GiveTask(
				Runservice.RunEvery(Global.RunservicePriority.Heartbeat.Physics, Interval,
					delegate {
						foreach (Gun gun in Turrets) {
							gun.Direction = gun.transform.right;
							gun.Shoot();
						}
					}
				)
			);

			Maid.GiveTask(
				Runservice.BindToFixedUpdate(Global.RunservicePriority.Heartbeat.Physics,
					(float dt) => {
						foreach (Gun gun in Turrets) {
							gun.transform.Rotate(new Vector3(0, 0, TurretSpeed) * Time.deltaTime);
						}
						return true;
					}
				)
			);
		}
	}

	public void UpdateStage1() {
		if (!SoundController.Singleton.BackgroundMusic || SoundController.Singleton.BackgroundMusic != SoundController.Singleton.Musics["Control"]) {
			ChangeMusic("Control");
		}

		Vector3 targetDir = (GunTurret.transform.position - Target.position).normalized;
		GunTurret.transform.right = Vector3.RotateTowards(GunTurret.transform.right, targetDir, GunTurretSpeed * Time.deltaTime, 5);
		GunTurret.Direction = -GunTurret.transform.right;
		GunTurret.Shoot();

		GunArray[0].Model.Rotate(new Vector3(0, 0, -GunArraySpeed) * Time.deltaTime);
		foreach (Gun gun in GunArray) {
			gun.Direction = gun.transform.right;
			gun.Shoot();
		}
	}

	public void UpdateStage2() {
		if (!SoundController.Singleton.BackgroundMusic || SoundController.Singleton.BackgroundMusic != SoundController.Singleton.Musics["Anticipation"]) {
			ChangeMusic("Anticipation");
		}

		Vector3 targetDir = (GunTurret.transform.position - Target.position).normalized;
		GunTurret.transform.right = Vector3.RotateTowards(GunTurret.transform.right, targetDir, GunTurretSpeed * Time.deltaTime, 5);
		GunTurret.Direction = -GunTurret.transform.right;
		GunTurret.Shoot();

		GunArray[0].Model.Rotate(new Vector3(0, 0, -GunArraySpeed) * Time.deltaTime);
		foreach (Gun gun in GunArray) {
			gun.Direction = gun.transform.right;
			gun.Shoot();
		}

		foreach (LaserGun laser in Lasers) {
			laser.transform.Rotate(new Vector3(0, 0, LaserTurretSpeed) * Time.deltaTime);
			laser.Shoot();
		}
	}

	public void UpdateStage3() {
		if (!SoundController.Singleton.BackgroundMusic || SoundController.Singleton.BackgroundMusic != SoundController.Singleton.Musics["Assault"]) {
			ChangeMusic("Assault");
		}

		Vector3 targetDir = (GunTurret.transform.position - Target.position).normalized;
		GunTurret.transform.right = Vector3.RotateTowards(GunTurret.transform.right, targetDir, GunTurretSpeed * Time.deltaTime, 5);
		GunTurret.Direction = -GunTurret.transform.right;
		GunTurret.Shoot();

		GunArray[0].Model.Rotate(new Vector3(0, 0, -GunArraySpeed) * Time.deltaTime);
		foreach (Gun gun in GunArray) {
			gun.Direction = gun.transform.right;
			gun.Shoot();
		}

		foreach (LaserGun laser in Lasers) {
			laser.transform.Rotate(new Vector3(0, 0, LaserTurretSpeed) * Time.deltaTime);
			laser.Shoot();
		}

		for (int i = 0; i < WallLasers.Count; i++) {
			LaserGun laser = WallLasers[i];
			Vector3 orgPos = orgWallLaserPos[i];
			if (laser.transform.position.y >= Mathf.Abs(orgPos.y)) {
				wallLaserGoUp[i] = false;
			} else if (laser.transform.position.y <= -Mathf.Abs(orgPos.y)) {
				wallLaserGoUp[i] = true;
			}

			laser.transform.position = laser.transform.position + new Vector3(0, wallLaserGoUp[i] ? WallLaserSpeed * Time.deltaTime : -WallLaserSpeed * Time.deltaTime, 0);

			laser.Shoot();
		}
	}

	public void FixedUpdate() {
		if (Target) {
			if (CurrentStage == 1) {
				UpdateStage1();
			} else if (CurrentStage == 2) {
				UpdateStage2();
			} else if (CurrentStage == 3) {
				UpdateStage3();
			}
		}
	}

	public override void Start() {
		base.Start();

		for (int i = 0; i < WallLasers.Count; i++) {
			LaserGun laser = WallLasers[i];
			orgWallLaserPos.Add(laser.transform.position);
			wallLaserGoUp.Add(i % 2 == 0);
		}

		Health.Connect(
			(float val) => {
				SwitchStage(1);
				Target = PlayerController.Singleton.PlayerCharacter.transform;

				return false;
			}
		);

		Maid.GiveTask(
			Health.Connect(
				(float val) => {
					if (val <= MaxHealth * (2F / 3F)) {
						SwitchStage(2);
						return false;
					}
					return true;
				}
			)
		);

		Maid.GiveTask(
			Health.Connect(
				(float val) => {
					if (val <= MaxHealth * (1F / 3F)) {
						SwitchStage(3);
						return false;
					}
					return true;
				}
			)
		);

		Health.Connect((float val) => {
			PlayerPrefs.SetFloat("Score", Health.Value);

			if (val <= 0) {
				SceneManager.LoadScene("WinScreen");
			}

			return (val > 0);
		});

	}
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character {
	private int CurrentStage = 0;

	[Header("Boss Parameters")]
	public Transform Target;

	[Header("Gun")]
	public Gun GunTurret;
	public float GunTurretSpeed = 1.5F;

	[Header("Gun Array")]
	public List<Gun> GunArray;
	public float GunArraySpeed = 60F;

	[Header("Turrets")]
	public List<Gun> Turrets;
	public float TurretSpeed = 40F;
	public float Interval = 2F;

	[Header("Cannons")]
	public List<Gun> Cannons;
	public float CannonSpeed = 60;

	[Header("Lasers")]
	public List<LaserGun> Lasers;
	public float LaserTurretSpeed = 15;

	[Header("WallLasers")]
	public List<LaserGun> WallLasers;
	private List<Vector3> orgWallLaserPos = new List<Vector3>();
	private List<bool> wallLaserGoUp = new List<bool>();
	public float WallLaserSpeed = 60F;

	public void ChangeMusic(string music) {
		if (SoundController.Singleton.BackgroundMusic) {
			SoundController.Singleton.BackgroundMusic.Stop();
		}
		SoundController.Singleton.BackgroundMusic = SoundController.Singleton.Musics[music];
		if (!SoundController.Singleton.BackgroundMusic.isPlaying) {
			SoundController.Singleton.BackgroundMusic.Play();
		}
	}

	private Listener<float> StageUpdate;
	public void SwitchStage(int index) {
		CurrentStage = index;
		if (CurrentStage == 1) {

		} else if (CurrentStage == 2) {
			StageUpdate?.Dispose();

		} else if (CurrentStage == 3) {
			StageUpdate?.Dispose();

		}
	}

	public void FixedUpdate() {
		Debug.Log(CurrentStage);

		if (CurrentStage == 1) {
			if (!SoundController.Singleton.BackgroundMusic || SoundController.Singleton.BackgroundMusic != SoundController.Singleton.Musics["Control"]) {
				ChangeMusic("Control");
			}

			Vector3 targetDir = (GunTurret.transform.position - Target.position).normalized;
			GunTurret.transform.right = Vector3.RotateTowards(GunTurret.transform.right, targetDir, GunTurretSpeed * Time.deltaTime, 5);
			GunTurret.Direction = -GunTurret.transform.right;
			GunTurret.Shoot();

			GunArray[0].Model.Rotate(new Vector3(0, 0, -GunArraySpeed) * Time.deltaTime);
			foreach (Gun gun in GunArray) {
				gun.Direction = gun.transform.right;
				gun.Shoot();
			}

			foreach (LaserGun laser in Lasers) {
				laser.transform.Rotate(new Vector3(0, 0, LaserTurretSpeed) * Time.deltaTime);
				laser.Shoot();
			}

			for (int i = 0; i < WallLasers.Count; i++) {
				LaserGun laser = WallLasers[i];
				Vector3 orgPos = orgWallLaserPos[i];
				if (laser.transform.position.y >= Mathf.Abs(orgPos.y)) {
					wallLaserGoUp[i] = false;
				} else if (laser.transform.position.y <= -Mathf.Abs(orgPos.y)) {
					wallLaserGoUp[i] = true;
				}

				laser.transform.position = laser.transform.position + new Vector3(0, wallLaserGoUp[i] ? WallLaserSpeed * Time.deltaTime : -WallLaserSpeed * Time.deltaTime, 0);

				laser.Shoot();
			}
		}
	}

	public override void Start() {
		base.Start();

		for (int i = 0; i < WallLasers.Count; i++) {
			LaserGun laser = WallLasers[i];
			orgWallLaserPos.Add(laser.transform.position);
			wallLaserGoUp.Add(i % 2 == 0);
		}

		Health.Connect(
			(float val) => {
				if (!Target) {
					SwitchStage(1);
					Target = PlayerController.Singleton.PlayerCharacter.transform;
				}
				return false;
			}
		);

		Maid.GiveTask(StageUpdate);

		Maid.GiveTask(
			Health.Connect(
				(float val) => {
					if (val <= MaxHealth * (2 / 3)) {
						SwitchStage(2);
						return false;
					}
					return true;
				}
			)
		);

		Maid.GiveTask(
			Health.Connect(
				(float val) => {
					if (val <= MaxHealth * (1 / 3)) {
						SwitchStage(3);
						return false;
					}
					return true;
				}
			)
		);

	}
}
*/