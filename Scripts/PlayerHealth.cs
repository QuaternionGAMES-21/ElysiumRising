using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static Dictionary<float, PlayerHealth> Players = new Dictionary<float, PlayerHealth>();

    private float maxHealth = 400;

    internal float currentHealth;

    public Image healthBarBG;
    public Image healthBarDisplay;
    private PlayerAttack playerAttack;

    private PlayerController movement;

    private Animator anim;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        healthBarBG = GameObject.Find("HealthBarBG").GetComponent<Image>();
        healthBarDisplay = GameObject.Find("HealthBar").GetComponent<Image>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            movement.canMove = false;
            playerAttack.canAttack = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBarDisplay.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            anim.SetBool("isMoving", false);
            anim.SetTrigger("Death");
        }
    }
}