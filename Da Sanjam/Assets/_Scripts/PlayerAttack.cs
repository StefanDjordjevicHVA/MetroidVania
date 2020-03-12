using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    float timeBtwAttack = 0f;

    [SerializeField]
    [Range(0, 1)]
    float startTimeBtwAttack = 0f;

    [SerializeField]
    [Range(0, 1)]
    float timeBtwCombo = 0f;

    [SerializeField]
    [Range(0, 1)]
    float startBtwCombo = 0f;

    [SerializeField]
    [Range(0, 1)]
    float attackRange = 0f;

    [SerializeField]
    [Range(0, 4)]
    int baseDamage = 0;

    int comboCount = 0;

    public Transform attackPos;
    private int enemies;

    /** TODO:
     *  add timer that checks if hit is in time for combo follow up
     *  add 2 more animations for 2nd hit and combo finisher
     *  combo finisher does 2x baseDamage
     *  reset comboCounter if combofinished or out of time
     *  change trigger in animation to int trigger
     * */

    public Animator anim;

    private void Start()
    {
        enemies = LayerMask.GetMask("Enemy");

        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(timeBtwCombo <= 0 || comboCount > 3)
            comboCount = 0;
        else timeBtwCombo -= Time.deltaTime;

        if (timeBtwAttack <= 0)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                    ComboAttacks();
            }
        } else timeBtwAttack -= Time.deltaTime;

        Animations(comboCount);
    }

    private void ComboAttacks()
    {

        //TODO: rework animation triggers to int for ez transitions
        
        Attack();
        
        timeBtwAttack = startTimeBtwAttack;
        timeBtwCombo = startBtwCombo;
    }

    private void Attack()
    {
        Debug.Log(comboCount);
        //TODO: add combo counter
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange, .5f), 0f, enemies);
        if (enemiesToDamage.Length >= 1)
        {
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i] != null && comboCount <= 1)
                {
                    enemiesToDamage[i].GetComponent<EnemyBehaviour>().TakeDamage(baseDamage);
                    Debug.Log("tik");
                }
                else if (enemiesToDamage[i] != null && comboCount >= 2)
                {
                    enemiesToDamage[i].GetComponent<EnemyBehaviour>().TakeDamage(baseDamage * 2);
                    Debug.Log("boom");
                }
            }
        }
        comboCount++;
    }

    private void Animations(int c)
    {
        anim.SetInteger("ComboCounter", c);
    }

    //gizmo to see attackrange and position
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireBox(attackPos.position, attackRange);
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRange, .5f, 1f));
    }
}
