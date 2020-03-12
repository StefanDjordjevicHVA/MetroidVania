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
    float attackRange = 0f;

    [SerializeField]
    [Range(0, 4)]
    int damage = 0;

    int comboCount = 0;

    public Transform attackPos;
    private int enemies;

    //TODO: Add 3rd hit combo attack

    public Animator anim;

    private void Start()
    {
        enemies = LayerMask.GetMask("Enemy");

        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if ((timeBtwAttack <= 0) && (timeBtwCombo <= 0))
        {
            if (Input.GetButtonDown("Fire3"))
            {
                anim.SetTrigger("TriggerHit");
                //TODO add combo counter
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange ,.5f), 0f, enemies);
                if (enemiesToDamage.Length >= 1)
                {
                    Debug.Log(enemiesToDamage.Length);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i] != null) ;
                            enemiesToDamage[i].GetComponent<EnemyBehaviour>().TakeDamage(damage);
                    }
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
        //Gizmos.DrawWireBox(attackPos.position, attackRange);
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRange, .5f, 1f));
    }
}
