using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soldierAttack : MonoBehaviour
{
    private Animator anim;
    public GameObject bulletPrefab;
    private GameObject bullet;
    public Transform firePoint;
    private PlayerController movement;
    internal bool isBlocking = false;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private ParticleSystem Shooting;
    public Transform SFXpoint;
    internal bool canAttack = true;
    internal bool canRotate = true;

    private void ShootingParticle()
    {
        ParticleSystem instance = Instantiate(Shooting, SFXpoint.position, SFXpoint.rotation);
        instance.Play();
        Destroy(instance.gameObject, 0.5f);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerController>();
        BulletTrail = Resources.Load<TrailRenderer>("Prefabs/HotTrail");
        Shooting = Resources.Load<ParticleSystem>("Prefabs/ShotVFX");
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(canRotate)
            {
                AttackDirection();
            }

            movement.canMove = false;

            if(canAttack)
            {
                Attack();
            }
        }
        else
        {
            anim.SetBool("Attack", false);
            movement.MoveSpeed = 4f;
            isBlocking = false;
            movement.canMove = true;
        }
    }

    private void AttackDirection()
    {
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
    }

    private void Attack()
    {
        canAttack = false;
        Invoke(nameof(ResetAttack), 0.15f);
        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if(bullet != null)
            {
                bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * 50f;
                TrailRenderer trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, bullet.transform));
            }
        }
        ShootingParticle();
        anim.SetBool("isMoving", false);
        anim.SetBool("Attack", true);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Transform bullet)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;
        while (time < 4)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, bullet.transform.position, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = bullet.transform.position;
        Destroy(Trail.gameObject, Trail.time);
    }
}
