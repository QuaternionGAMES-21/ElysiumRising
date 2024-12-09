using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : MonoBehaviour
{
    // [SerializeField] private float maxMoveSpeed = 5f;
    internal bool isAttacking = false;

    private Rigidbody rigid;
    private Animator anim;
    private NavMeshAgent agent;
    MeleeEnemyDamage dealDamage;
    public GameObject[] players;
    internal bool playerInSearchDistance;
    internal Transform closestPlayer;
    private string playerTag = "Player";
    private float attackRange = 2.2f;
    private float searchDistance = 7f;

    private PlayerController attackTarget;
    private PlayerController lastAttacker;

    internal float distanceToPlayer;
    public Transform[] points; // Точки для патрулирования
    private int destPoint = 0; // Вспомогательный счетчик для точек патрулирования
    private Vector3 startPosition; // Для возвращения обратно в стартовую позицию 
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        dealDamage = GetComponent<MeleeEnemyDamage>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        players = GameObject.FindGameObjectsWithTag(playerTag);
        startPosition = gameObject.transform.position;
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        playerInSearchDistance = Physics.CheckSphere(transform.position, searchDistance);
        if (playerInSearchDistance)
        {
            players = GameObject.FindGameObjectsWithTag(playerTag);
            if (players != null && players.Length > 0)
            {
                float minDistance = Mathf.Infinity;
                closestPlayer = null;

                foreach (GameObject player in players)
                {
                    float distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestPlayer = player.transform;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (rigid == null)
            return;

        isAttacking = dealDamage.alreadyAttacked;

        if (!isAttacking && enemyHealth.currentHealth > 0)
            UpdateBasicAI();

        if (distanceToPlayer <= attackRange && !isAttacking && enemyHealth.currentHealth > 0)
        {
            agent.SetDestination(transform.position); // Обновление места назначения до функции атаки, чтобы во время срабатывания функции атаки объект не передвигался
            anim.SetBool("isMoving", false);
            anim.speed = 1f;
            dealDamage.Attack();
        }
    }

    private void SetAttackTarget()
    {
        distanceToPlayer = Vector3.Distance(transform.position, GetClosestPlayerPosition());

        if (distanceToPlayer < searchDistance && distanceToPlayer > attackRange)
            attackTarget = GetClosestPlayer();
        else if (lastAttacker != null)
            attackTarget = lastAttacker;
    }

    private void UpdateBasicAI()
    {
        SetAttackTarget();

        if (attackTarget != null && distanceToPlayer < searchDistance)
            MoveTo(attackTarget.transform.position);
        else if (points.Length == 0)
            GotoStartPosition();
        
        if (points.Length > 0 && !agent.pathPending && agent.remainingDistance < 1.0f)
            GotoNextPoint();

        if (!agent.hasPath)
            anim.SetBool("isMoving", false);
        else
            anim.SetBool("isMoving", true);
    }

    private void MoveTo(Vector3 position)
    {
        // if (rigid.velocity.magnitude > maxMoveSpeed)
        //     return;

        agent.speed = 5f;
        anim.speed = 3.5f;
        agent.destination = position;
    }

    public void SetAttacker(PlayerController player)
    {
        lastAttacker = player;
    }

    private Vector3 GetClosestPlayerPosition()
    {
        Vector3 closestPlayerPosition = Vector3.zero;
        float closestDistance = searchDistance;
        foreach (var player in PlayerController.Players.Values)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(player.transform.position, transform.position);
                closestPlayerPosition = player.transform.position;
            }
        }
        return closestPlayerPosition;
    }

    private PlayerController GetClosestPlayer()
    {
        PlayerController closestPlayer = default;
        float closestDistance = searchDistance;
        foreach (var player in PlayerController.Players.Values)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(player.transform.position, transform.position);
                closestPlayer = player;
            }
        }
        return closestPlayer;
    }

    void GotoNextPoint()
    {
        agent.speed = 1f;
        anim.speed = 1f;
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    private void GotoStartPosition()
    {
        agent.speed = 1f;
        anim.speed = 1f;
        agent.destination = startPosition;
    }
}