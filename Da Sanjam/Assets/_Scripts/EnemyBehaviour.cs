using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    /**TODO:
     * movement towards player
     * stops following player if out of range
     * returns to startlocation if player out of range
     * flip enemy depending on walking direction
     * can take damage
     */

    [SerializeField]
    GameObject player;
    Transform startPos;

    [SerializeField]
    Rigidbody2D rb;

    float distance = 0f;
    Vector2 direction;

    [SerializeField]
    [Range(0, 5)]
    float activationDistance = 0f;

    [SerializeField]
    [Range(0, 1)]
    float timeBtwMovement, timeWhenHit = 0f;

    [SerializeField]
    [Range(0, 1)]
    float timeStartBtwMovement = 0f;

    [SerializeField]
    [Range(0, 2)]
    float timeStartWhenHit = 0f;

    [SerializeField]
    [Range(0, 5)]
    float crawlVelocity = 0f, hitVelocity = 0f;

    [SerializeField]
    [Range(0, 10)]
    int health = 0;

    bool hit = false;

    int prevHealt;
    int currentHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        startPos = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CalculateDistanceAndDirection(transform, player.transform);

        timeBtwMovement -= Time.deltaTime;
        //timeWhenHit -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        EnemyMovement(crawlVelocity, hitVelocity);
    }

    private void CalculateDistanceAndDirection(Transform e, Transform p)
    {
        distance = Vector3.Distance(e.position, p.position);

        direction = (Vector2)p.position - (Vector2)e.position;

        direction.Normalize();

    }

    private void EnemyMovement(float skipVel, float hitVel)
    {
        if (timeBtwMovement <= 0 && !hit && distance < activationDistance)
        {
            rb.velocity = new Vector2(skipVel * direction.x, skipVel);
            timeBtwMovement = timeStartBtwMovement;
        } else if (hit)
        {
            if((Mathf.Abs(prevHealt - currentHealth) == 1))
                rb.velocity = new Vector2(hitVel * -direction.x, rb.velocity.y);
            else if ((Mathf.Abs(prevHealt - currentHealth) == 2))
                rb.velocity = new Vector2((hitVel * 4) * -direction.x, rb.velocity.y);
            hit = false;
        }
            
    }

    public void TakeDamage(int damage)
    {
        prevHealt = health;

        health -= damage;

        currentHealth = health;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        hit = true;
        //timeWhenHit = timeStartWhenHit;
    }

    
        
}
