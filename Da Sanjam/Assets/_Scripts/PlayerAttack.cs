using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    float timeBtwAttack;

    [SerializeField]
    [Range(0, 1)]
    float startTimeBtwAttack;

    [SerializeField]
    [Range(0, 1)]
    float timeBtwCombo;

    [SerializeField]
    [Range(0, 1)]
    float attackRange;

    int comboCount = 0;

    public Transform attackPos;
    public LayerMask enemies;

    //TODO: Add 3rd hit combo attack

    private void Update()
    {
        if ((timeBtwAttack <= 0) && (timeBtwCombo <= 0))
        {
            if (Input.GetButtonDown("Fire3"))
            {
                //TODO add combo counter
                Debug.Log("SLASH!");

                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //do something
                }

                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else timeBtwAttack -= Time.deltaTime;
    }

    //gizmo to see attackrange and position
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
