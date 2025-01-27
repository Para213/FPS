using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State 
    {
        Chasing,
        Attacking,
        AttackCooldown
    }

    private NavMeshAgent agent; 
    private Transform playerTransform; 
    private EnemyHealth enemyHealth; 
    private PlayerHealth playerHealth;

    public float attackDistance = 2.0f;
    public float attackDamage = 12.0f; 
    public float attackDelay = 2.0f; 
    public bool isKnockedBack; 

    public State state; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        enemyHealth = GetComponent<EnemyHealth>(); 
        playerHealth = playerTransform.GetComponent<PlayerHealth>(); 

        state = State.Chasing; 
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position); 

        switch (state)
        {
            case State.Chasing: 
                if (distanceToPlayer <= attackDistance) 
                {
                    state = State.Attacking; 
                }
                else
                {
                    if (!isKnockedBack)
                    {
                        agent.SetDestination(playerTransform.position); 
                    }
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackDistance)
                {
                    state = State.Chasing;
                }
                else
                {
                    AttackPlayer();
                }
                break;

            case State.AttackCooldown: // In AttackCooldown state, do nothing
                break;
        }
    }

    private void AttackPlayer() 
    {
        playerHealth.DealDamage(Mathf.RoundToInt(attackDamage)); 
        state = State.AttackCooldown; 
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay() 
    {
        yield return new WaitForSeconds(attackDelay); 
        state = State.Chasing;
    }
}
