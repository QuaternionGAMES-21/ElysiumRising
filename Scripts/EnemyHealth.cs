using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 200f;
    internal float currentHealth;
    internal bool canTakeDamage = true;
    public Image healthBarBG;
    public Image healthBarDisplay;
    private QuestSystem questSystem;
    private Animator anim;
    private MeleeEnemyController enemyController;
    internal bool enemyDead = false;
    internal float timeTakeDamage;

    private void Awake()
    {
        
        healthBarBG.gameObject.SetActive(false);
        currentHealth = maxHealth;
        questSystem = GameObject.Find("QuestCanvas").GetComponent<QuestSystem>();
        anim = GetComponent<Animator>();
    }

    // ��� ��������� ������ �����, ����� ���������� ����������� ������ � �������� SwordHandler � SpearHandler
    public void TakeDamage(float damage)
    {
        healthBarBG.gameObject.SetActive(true);

        if (canTakeDamage && currentHealth > 0f)
        {
            canTakeDamage = false;
            Invoke(nameof(TakeDamageReset), timeTakeDamage);
            currentHealth -= damage;
            healthBarDisplay.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0f && !enemyDead)
        {
            enemyDead = true;
            Die();
            QuestSystemCount();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Death");
        Invoke(nameof(DestroyEnemy), 6);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void TakeDamageReset()
    {
        canTakeDamage = true;
    }

    private void QuestSystemCount()
    {
        if(questSystem.currentProgress < questSystem.requiredProgress)
        {
            questSystem.currentProgress += 1;
            questSystem.currentProgressDisplay.text = questSystem.currentProgress.ToString();
            if (questSystem.currentProgress == (questSystem.requiredProgress - 1))
            {
                questSystem.isQuestCompleted = true;
            }
        }
    }
}