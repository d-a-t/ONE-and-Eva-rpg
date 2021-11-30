using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // declare Animator
    public Animator animator;

    // use the attackPoint
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        if(Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ATTACK", "Space"))))
		{
            Attack();

            // can only attack every 2 sec
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        // delete later
        Debug.Log("Attack");

        // Play an attack animation
        animator.SetTrigger("Attack");

        // Deteck enemies in range of attack (will need to give enemies the enemy layer later)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);       // creates circle with attackPoint and collects all obj that is hit with circle

        // dmg enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.GetComponent<Boss>()) {
				// call damage methods
				enemy.gameObject.GetComponent<Boss>().Health.Value -= 10;
			}
        }
    }

    // draw the attackpt circle
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
