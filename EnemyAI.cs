using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundMask, playerMask;
    public GunSystem gunSystem;
    private EnemyCounter enemyCounter;
    private float originX;
    private float originZ;

    //enemy patrolling variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //enemy attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float health;
    
    //States
    public float sightRange, attackRange;
    public bool isPlayerInSightRange, isPlayerInAttackRange;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent =  GetComponent<NavMeshAgent>();
        gunSystem = GetComponent<GunSystem>();
    }

    void Start()
    {
        enemyCounter = GameObject.FindAnyObjectByType<EnemyCounter>();

        originX = transform.position.x;
        originZ = transform.position.z;
    }

    void Update()
    {
        //check if player is in sight and attack range (ignoring walls)
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        Ray ray = new Ray(transform.position, directionToPlayer.normalized);
        RaycastHit hit;

        //only checks if player is hit by raycast, not walls so cannot see through walls
        if (Physics.Raycast(ray, out hit, distanceToPlayer, playerMask))
        {
            isPlayerInSightRange = distanceToPlayer <= sightRange;
            isPlayerInAttackRange = distanceToPlayer <= attackRange;
        }
        else
        {
            isPlayerInSightRange = false;
            isPlayerInAttackRange = false;
        }

        if(!isPlayerInSightRange && !isPlayerInAttackRange){
            EnemyPatrolling();               
        } 
        if(isPlayerInSightRange && !isPlayerInAttackRange){
            ChasePlayer();
        }
        if(isPlayerInSightRange && isPlayerInAttackRange){
            AttackPlayer();
        }
    }

    //Function to handle enemy patrolling state
    private void EnemyPatrolling()
    {
        // Debug.Log("Patrolling...");
        if(!walkPointSet){
            SearchWalkPoint();
        }
        if(walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if walkPoint is reached
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }
    
    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(originX + randomX, transform.position.y, originZ + randomZ);
        
        if(!isPlayerInSightRange && !isPlayerInAttackRange && !walkPointSet){
            //only sets walk point if it's not too close to the wall
            if (!Physics.CheckSphere(walkPoint, 2f, LayerMask.GetMask("wall"))){
                walkPointSet = true;
            }
        }
    }

    //Function to handle enemy chasing player state
    private void ChasePlayer()
    {
        Debug.Log("Chasing player...");
        //agent moves towards player position
        agent.SetDestination(player.position);
    }

    //Function to handle enemy attacking player state
    private void AttackPlayer()
    {
        //Debug.Log("Attacking player...");
        //stops enemy from moving and makes sure enemy looking at player when attacking
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked){
            gunSystem.TriggerShot(); //enemy shoots
            //Notifies the player that they've been shot at (even if missed)
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null){
                playerHealth.NotifyUnderAttack();
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack(){
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage){
        health -= damage;
        //gets rid of enemy when they die
        if(health <=0){
            //alerts enemy counter if an enemy has died
            if (enemyCounter != null)
            {
                enemyCounter.OnEnemyKilled(transform.position);
            } 
            Invoke(nameof(DestroyEnemy), 0.1f);
        }
    }

    private void DestroyEnemy(){
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //alerts user how close they are to enemy detection/attacking them
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
