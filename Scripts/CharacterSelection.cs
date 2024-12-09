using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CharacterSelection : MonoBehaviour
{
    public bool spartanChosen = false;
    public bool soldierChosen = false;
    public GameObject spartanPrefab;
    public GameObject soldierPrefab;
    private GameObject spartanDisplay;
    private GameObject soldierDisplay;
    public Transform SpawnPoint;

    private void Awake()
    {
        spartanDisplay = GameObject.Find("SpartanImage");
        soldierDisplay = GameObject.Find("SoldierImage");
    }

    public void SpawnSpartan()
    {
        spartanChosen = true;
        soldierChosen = false;
    }

    public void SpawnSoldier()
    {
        soldierChosen = true;
        spartanChosen = false;
        SpawnHero();
    }

    private void SpawnHero()
    {
        if(spartanChosen)
        {
            GameObject player = Instantiate(spartanPrefab, SpawnPoint.position, quaternion.identity);
        }
        else
        {
            GameObject player = Instantiate(soldierPrefab, SpawnPoint.position, quaternion.identity);
        }
    }
}
