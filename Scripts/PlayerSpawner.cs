using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject chooseSpartanButton;
    public GameObject chooseSoldierButton;
    public GameObject spartanPrefab;
    public GameObject soldierPrefab;
    public GameObject petPrefab;
    public bool spartanChosen = false;
    public bool soldierChosen = false;

    public Transform spawnPoint;

    private void Awake()
    {
        chooseSpartanButton.SetActive(true);
        chooseSoldierButton.SetActive(true);
    }

    public void SpawnPet()
    {
        Instantiate(petPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnSpartan()
    {
        Instantiate(spartanPrefab, spawnPoint.position, Quaternion.identity);
        chooseSpartanButton.SetActive(false);
        chooseSoldierButton.SetActive(false);
        spartanChosen = true;
        //SpawnPet();
    }

    public void SpawnSoldier()
    {
        Instantiate(soldierPrefab, spawnPoint.position, Quaternion.identity);
        chooseSpartanButton.SetActive(false);
        chooseSoldierButton.SetActive(false);
        soldierChosen = true;
        //SpawnPet();
    }
}
