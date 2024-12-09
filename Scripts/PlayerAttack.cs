using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    internal bool canAttack = true;
    [SerializeField] public Animator anim;
    private PlayerController movement;
    internal bool isAttacking = false;
    internal bool isBlocking = false;
    private GameObject spearPrefab;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && canAttack)
        {
            Attack();
        }
        else
        {
            movement.MoveSpeed = 4f;
            isBlocking = false;
        }
    }

    private void Attack()
    {
        movement.canMove = false;
        canAttack = false;
        isAttacking = true;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 attackDirection = hit.point - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(attackDirection, Vector3.up);
            float currentXRotation = transform.eulerAngles.x;

            Vector3 targetEulerAngles = targetRotation.eulerAngles;

            Vector3 fixedEulerAngles = new Vector3(currentXRotation, targetEulerAngles.y, targetEulerAngles.z);

            transform.eulerAngles = fixedEulerAngles;
        }

        if(movement.isSpearActive)
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetTrigger("SwordAttack");
        }

        anim.SetBool("isMoving", false);

        Invoke(nameof(AttackCooldown), 1f);
        Invoke(nameof(MovementCooldown), 0.7f);
        Invoke(nameof(AttackAnimationFlow), 0.3f);
    }

    private void ShieldBlock()
    {
        isBlocking = true;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 attackDirection = hit.point - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(attackDirection, Vector3.up);
            float currentXRotation = transform.eulerAngles.x;
            currentXRotation = 0;

            Vector3 targetEulerAngles = targetRotation.eulerAngles;

            Vector3 fixedEulerAngles = new Vector3(currentXRotation, targetEulerAngles.y, targetEulerAngles.z);

            transform.eulerAngles = fixedEulerAngles;
        }
    }

    private void AttackCooldown()
    {
        canAttack = true;
    }

    public void MovementCooldown()
    {
        movement.canMove = true;
    }

    public void AttackAnimationFlow()
    {
        isAttacking = false;
    }
}
