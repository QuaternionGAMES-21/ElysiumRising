using UnityEngine;
using TMPro;
using DG.Tweening;

public class SpearHandler : Weapon
{
    private PlayerAttack playerObj;
    public GameObject popupObj;
    private GameObject mainCamera;
    private Vector3 popUpOriginalScale;
    private Vector3 popUpScaleTo;
    private GameObject popup;

    private void Awake()
    {
        minAttackDamage = 10;
        maxAttackDamage = 20;

        mainCamera = GameObject.Find("Camera");
        playerObj = GetComponentInParent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && playerObj.isAttacking)
        {
            attackDamage = (Random.Range(minAttackDamage, maxAttackDamage));
            var enemy = other.GetComponent<EnemyHealth>();
            enemy.timeTakeDamage = 1f;
            if (enemy.canTakeDamage && !enemy.enemyDead)
            {
                enemy.TakeDamage(attackDamage);
                enemy.canTakeDamage = false;
                Vector3 offset = new Vector3(1, Random.Range(2f, 3f), 0);
                if (enemy == true)
                    CreatePopUp(other.transform.position + offset, attackDamage.ToString());
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
