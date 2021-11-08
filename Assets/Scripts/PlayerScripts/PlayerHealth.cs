using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Transform pfHealthBar;   // refrence to prefab
    private HealthSystem healthSystem;
    public GameObject thing;
    public Vector3 healthBarPosition = new Vector3();

    private void Start()
    {
        healthSystem = new HealthSystem(100);      // player has 100 health

        Transform healthBarTransform = Instantiate(pfHealthBar, healthBarPosition, Quaternion.identity);
        healthBarTransform.transform.parent = thing.transform;
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);
    }

    public void Update()
    {
        // print the health
        if (Input.GetKeyDown("j"))
        {
            Debug.Log("Health: " + healthSystem.GetHealthPercent());
        }
        if (Input.GetKeyDown("k"))
        {
            healthSystem.Damage(10);
            Debug.Log("Damaged: " + healthSystem.GetHealth());
        }
        if (Input.GetKeyDown("l"))
        {
            healthSystem.Heal(10);
            Debug.Log("Healed: " + healthSystem.GetHealth());
        }
    }
}
