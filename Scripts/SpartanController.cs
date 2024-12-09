using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpartanController : PlayerController
{
    public GameObject spearPrefab;
    public GameObject swordPrefab;

    public override void Awake()
    {
        base.Awake();
        spearPrefab.SetActive(true);
        swordPrefab.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
        WeaponSwitch();
    }

    internal void WeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            canMove = false;
            Invoke(nameof(MovementCooldown), 1f);
            if (isSpearActive)
            {
                isSpearActive = false;
                spearPrefab.SetActive(false);
                swordPrefab.SetActive(true);
            }
            else
            {
                isSpearActive = true;
                spearPrefab.SetActive(true);
                swordPrefab.SetActive(false);
            }
            anim.SetTrigger("WeaponSwitch");
        }
    }
}
