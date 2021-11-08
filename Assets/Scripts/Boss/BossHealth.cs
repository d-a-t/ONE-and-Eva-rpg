using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public Transform pfHealthBar;   // refrence to prefab
    private HealthSystem healthSystem;
    public GameObject thing;
    public Vector3 healthBarPosition = new Vector3();

    private void Start()
    {
        healthSystem = new HealthSystem(1000);      // player has 100 health

        Transform healthBarTransform = Instantiate(pfHealthBar, healthBarPosition, Quaternion.identity);
        healthBarTransform.transform.parent = thing.transform;
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);
    }

    public void takeDamage(int x)
    {
        healthSystem.Damage(x);
    }
}
