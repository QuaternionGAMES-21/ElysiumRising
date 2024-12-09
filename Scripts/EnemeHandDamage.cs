using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemeHandDamage : MonoBehaviour
{
    private GameObject player;
    private MeleeEnemyDamage enemy;
    public GameObject popupObj;
    private GameObject mainCamera;
    private Vector3 popUpOriginalScale;
    private Vector3 popUpScaleTo;
    private GameObject popup;

    void OnTriggerEnter(Collider other) {

        player = other.gameObject;
        enemy = gameObject.GetComponentInParent<MeleeEnemyDamage>();

        if (player.tag == "Player" && enemy.handDamage)
        {       
            enemy.countHandTrigger++; // подсчитывает количество срабатываний триггера за один удар, триггер иногда срабатывет 2 раза

            if (enemy.countHandTrigger == 1)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(enemy.damage);
                Vector3 offset = new Vector3(1, Random.Range(2f, 3f), 0);
                CreatePopUp(other.transform.position + offset, enemy.damage.ToString());
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
        temp.color = new Color(1f, 0f, 0f);
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
