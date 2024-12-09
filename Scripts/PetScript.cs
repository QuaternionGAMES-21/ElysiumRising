using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PetScript : MonoBehaviour
{
    private GameObject[] enemies;
    public GameObject bulletPrefab;
    private GameObject bullet;
    public Transform firePoint;
    internal bool isBlocking = false;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private ParticleSystem Shooting;
    public Transform SFXpoint;
    internal bool canAttack = true;
    private float searchDistance = 5f;
    internal bool enemyInSphere;
    internal Transform closestEnemy;

    private void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        transform.position = GameObject.FindGameObjectsWithTag("Player")[0].transform.position + new Vector3(2, 1, 1);
        enemyInSphere = Physics.CheckSphere(transform.position, searchDistance, 1 << 3);
        if (enemyInSphere)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies != null && enemies.Length > 0)
            {
                float minDistance = Mathf.Infinity;
                closestEnemy = null;

                foreach (GameObject enemy in enemies)
                {
                     float distance = Vector3.Distance(transform.position, enemy.transform.position);
                     if (distance < minDistance)
                     {
                         minDistance = distance;
                         closestEnemy = enemy.transform;

                     }
                }
                if (Vector3.Distance(transform.position, closestEnemy.position) < 5f)
                {
                    if(canAttack)
                        Attack();
                }
                Vector3 enemyPosition = new Vector3(closestEnemy.transform.position.x, transform.position.y, closestEnemy.transform.position.z);
                transform.rotation = Quaternion.AngleAxis(10f, Vector3.right);
                transform.LookAt(enemyPosition);
            }
        }

        #region test
        /// <summary>
        ///if (GameObject.FindGameObjectsWithTag("Player").Length != 0)
        ///transform.position = GameObject.FindGameObjectsWithTag("Player")[0].transform.position + new Vector3(2, 1, 1);
        ///foreach (GameObject go in enemies)
        ///{
        ///if (!go.activeSelf)
        ///enemies.Remove(go);
        ///if(Vector3.Distance(transform.position, go.transform.position) < distance)
        ///{
        ///distance = Vector3.Distance(transform.position, go.transform.position);
        ///closest = go;
        ///}
        ///}
        ///if (Vector3.Distance(transform.position, closest.transform.position) < 10f)
        ///{
        ///Attack();
        ///}
        ///transform.LookAt(closest.transform, Vector3.up);
        //firePoint.Rotate(-14, 0, 0);
        /// </summary>
        #endregion

    }
    private void Attack()
    {
        canAttack = false;
        Invoke(nameof(ResetAttack), 0.5f);

        //RaycastHit hit;
       // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet != null)
            {
                bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * 50f;
                TrailRenderer trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, bullet.transform));
            }

        ShootingParticle();
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
            if(Trail != null)
                Trail.transform.position = Vector3.Lerp(startPosition, bullet.transform.position, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = bullet.transform.position;
        Destroy(Trail.gameObject, Trail.time);
    }
    private void ShootingParticle()
    {
        ParticleSystem instance = Instantiate(Shooting, SFXpoint.position, SFXpoint.rotation);
        instance.Play();
        Destroy(instance.gameObject, 0.5f);
    }

}
