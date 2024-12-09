using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyDamage : MonoBehaviour
{
    public int damage = 200;
    private float timeBetweenAttacks = 3f;
    internal bool alreadyAttacked = false;
    internal bool handDamage = false; // Флаг для определения момента касания коллайдера руки с игроком
    internal int countHandTrigger = 0; // Нужен для подсчета количества срабатываний триггера кулака врага за один удар
    private MeleeEnemyController enemyController;
    private Animator anim;
    private List<PlayerHealth> _playersWeHit = new List<PlayerHealth>();

    private void Awake()
    {
        enemyController = GetComponent<MeleeEnemyController>();
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        transform.LookAt(enemyController.closestPlayer);

        if (!alreadyAttacked && enemyController.closestPlayer != null && enemyController.closestPlayer.GetComponent<PlayerHealth>().currentHealth > 0)
        {
            alreadyAttacked = true;
            anim.SetBool("Run", false);
            anim.SetBool("isMoving", false);
            anim.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Invoke(nameof(ResetCountHandTrigger), timeBetweenAttacks); // Сброс подсчета кол-ва срабатываний триггера кулака за один удар
            Invoke(nameof(ResetHandDamage), 1.5f); // Открытие окна нанесения урона
            Invoke(nameof(ResetHandDamage), 1.7f); // Закрытие окна нанесения урона
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetHandDamage()
    {
        handDamage = handDamage ? false : true;
    }
    
    private void ResetCountHandTrigger()
    {
        countHandTrigger = 0;
    }
}
