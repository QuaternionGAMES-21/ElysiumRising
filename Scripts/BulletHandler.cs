using UnityEngine;
using TMPro;
using DG.Tweening;

public class BulletHandler: Weapon
{
    private GameObject playerObj;
    public GameObject popupObj;
    private GameObject mainCamera;
    private Vector3 popUpOriginalScale;
    private Vector3 popUpScaleTo;
    private GameObject popup;

    private void Awake()
    {
        minAttackDamage = 3;
        maxAttackDamage = 7;
        Destroy(gameObject, 0.2f);
        mainCamera = GameObject.Find("Camera");
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Enemy"))
            {
                attackDamage = (Random.Range(minAttackDamage, maxAttackDamage));
                var enemy = other.GetComponent<EnemyHealth>();
                if (enemy.canTakeDamage && !enemy.enemyDead)
                {
                    enemy.timeTakeDamage = 0.1f;
                    enemy.TakeDamage(attackDamage);
                    enemy.canTakeDamage = false;
                    Vector3 offset = new Vector3(1, Random.Range(2f, 3f), 0);
                    CreatePopUp(other.transform.position + offset, attackDamage.ToString());

                    var controller = other.GetComponent<MeleeEnemyController>();
                    var playerController = playerObj.GetComponent<PlayerController>();

                    controller.SetAttacker(playerController);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void CreatePopUp(Vector3 popUpPosition, string text)
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Camera");
        }

        popup = Instantiate(popupObj, popUpPosition, Quaternion.identity);
        TMP_Text temp = popup.transform.GetChild(0).GetComponent<TMP_Text>();
        temp.text = text;
        popup.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);
        popUpOriginalScale = popup.transform.localScale;
        popUpScaleTo = popUpOriginalScale * 2;
        OnScale();
        Destroy(popup, 0.4f);
    }

    private void OnScale()
    {
        popup.transform.DOScale(popUpScaleTo, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            popup.transform.DOScale(popUpOriginalScale, 0.2f)
            .SetEase(Ease.InOutSine).OnComplete(OnScale);
        });
    }
}