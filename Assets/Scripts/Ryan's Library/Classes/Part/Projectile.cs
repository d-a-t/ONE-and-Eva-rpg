using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Constructs and spawns a Projectile that destroys either upon impact or after .5 seconds by default.<p/>
/// It's default speed is 60 units per second and does 30 damage on it, and with a yellow tracer.
/// </summary>
public class Projectile : Part {
	private LineRenderer lr;
	private Vector2 origin;

	[Header("Projectile Parameters")]
	public float Speed = 120F;
	public int Damage = 30;

	/// <summary>
	/// The direction from which the projectile shoots relative to itself.
	/// </summary>
	public Vector2 Direction;
	public float Duration = 3;

	[Header("Tracer Parameters")]
	public Color Color = Color.yellow;
	/// <summary>
	/// Length of the tracer. By default it's .5F.
	/// </summary>
	public float Length = 1.5F;
	public Tuple<float, float> Width = new Tuple<float, float>(.15F, .15F);

	public enum Types {
		Hitscan,
		Fastcast,
		RigidBody
	}

	[Header("Type")]
	public Types Type = Types.Fastcast;

	public Variable<Transform> Hit = new Variable<Transform>();

	public List<Collider2D> IgnoreColliders = new List<Collider2D>();

	protected internal Vector2 lastPos;

	/// <summary>
	/// Constructs a GameObject which the Projectile attaches onto. 
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="direction"></param>
	/// <returns></returns>
	public static Projectile Create(Types type, Vector2 pos, Vector2 direction) {
		GameObject empty = new GameObject();
		empty.transform.UpdateFromCFrame(new CFrame(pos) * CFrame.FromEulerAnglesXYZ(0, 0, Mathf.Atan2(direction.y, direction.x)));

		empty.name = "Projectile";

		Projectile self = empty.AddComponent<Projectile>();

		self.Type = type;

		self.Direction = direction.normalized;
		self.origin = pos;

		self.Maid.GiveTask(self.gameObject);

		self.lastPos = self.origin;
		float counter = 0;
		switch (self.Type) {
			case Types.RigidBody: {
					self.Collider = self.gameObject.AddComponent<PolygonCollider2D>();
					self.Maid.GiveTask(self.Collider);

					Listener<float> updateProjectileHitbox = Runservice.BindToFixedUpdate(Global.RunservicePriority.Heartbeat.Physics, (float dt) => {
						if (self.Collider == null) {
							return false;
						}

						if ((self.lastPos - empty.transform.position.AsVector2()).magnitude < self.Length * .75) {
							PolygonCollider2D asPolygon = self.Collider as PolygonCollider2D;

							self.Collider.enabled = false; //disables cause SetPath is really inefficient when enabled.
							Vector2[] pointList = new Vector2[] { (new Vector2(0, 0.025F)), new Vector2(0, -0.025F), new Vector2(-(self.lastPos - empty.transform.position.AsVector2()).magnitude, -0.025F), new Vector2(-(self.lastPos - empty.transform.position.AsVector2()).magnitude, 0.025F) };
							asPolygon.SetPath(0, pointList);
							self.Collider.enabled = true;
						} else {
							return false;
						}

						return true;
					});
					updateProjectileHitbox.Name = "updateProjectileHitbox";
					self.Maid.GiveTask(updateProjectileHitbox);

					break;
				}

			case Types.Fastcast: {
					self.lr = empty.AddComponent<LineRenderer>();
					self.lr.material = (Material)Resources.Load("Materials/Bullet", typeof(Material));
					self.lr.startColor = self.Color;
					self.lr.endColor = self.Color;
					self.lr.startWidth = self.Width.Item1;
					self.lr.endWidth = self.Width.Item2;

					self.lr.sortingLayerName = "Top";
					self.lr.sortingOrder = 90;

					Listener<float> updateProjectileHitbox = Runservice.BindToFixedUpdate(Global.RunservicePriority.Heartbeat.Physics,
						(float dt) => {
							if (empty == null) {
								return false;
							}

							RaycastHit2D hit = Physics2D.Raycast(self.lastPos, direction, (self.lastPos - empty.transform.position.AsVector2()).magnitude, ~LayerMask.GetMask("Projecties"));
							if (hit.collider && !self.IgnoreColliders.Contains(hit.collider)) {
								if (hit.collider != self.Hit.Value) {
									self.IgnoreColliders.Add(hit.collider);
									self.Hit.Value = hit.collider.gameObject.transform;
								}
							} else {
								self.Hit.Value = null;
							}
							self.lastPos = self.transform.position;
							return true;
						}
					);
					updateProjectileHitbox.Name = "updateProjectileHitbox";
					self.Maid.GiveTask(updateProjectileHitbox);

					break;
				}
		}

		Listener<float> drawProjectile = Runservice.BindToUpdate(Global.RunservicePriority.Heartbeat.Physics, (float dt) => {
			empty.transform.position += (self.Direction * self.Speed * dt).AsVector3();

			if ((self.lastPos - empty.transform.position.AsVector2()).magnitude > self.Length) {
				Vector2 diff = (self.lastPos - self.lr.transform.position.AsVector2());
				self.lastPos += (self.Direction * self.Speed * dt);
			}

			self.lr.SetPosition(0, self.lastPos);
			self.lr.SetPosition(1, self.lr.transform.position);

			if (counter > self.Duration) {
				self.Dispose();
				return false;
			}
			counter += dt;
			return true;
		});
		drawProjectile.Name = "drawProjectile";
		self.Maid.GiveTask(drawProjectile);

		return self;
	}

	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	public void OnDrawGizmos() {
		Extensions.DrawArrow(lastPos, Direction, (lastPos - transform.position.AsVector2()).magnitude);
	}

	public override void Dispose() {
		base.Dispose();
		GameObject.Destroy(lr);
	}
}
