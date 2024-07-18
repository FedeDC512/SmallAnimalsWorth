using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject[ ] animalPrefabs;
    public int animalIndex;
    private float startDelay = 0;
    private float spawnInterval = 1f;

    public void StartFire()
    {
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
    }

    public void StopFire()
    {
        // Cancel all Invoke calls
        CancelInvoke();
    }

    void SpawnRandomAnimal() {
        //randomize the spawn position of the animal
        int randomSideSpawn = Random.Range(1, 4);
        switch (randomSideSpawn) 
        {
        case 1:
            SpawnUpAnimal();
            break;
        case 2:
            SpawnLeftAnimal();
            break;
        case 3:
            SpawnRightAnimal();
            break;
        }
    }

    void SpawnUpAnimal() {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-22, -13), -7.15f, 10);
        Instantiate(animalPrefabs[animalIndex], spawnPos , animalPrefabs[animalIndex].transform.rotation);
    }

    void SpawnLeftAnimal() {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(-17, -7.15f, Random.Range(-20, -14));
        Instantiate(animalPrefabs[animalIndex], spawnPos , Quaternion.Euler(new Vector3(0, -90, 0)));
    }

    void SpawnRightAnimal() {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnPos = new Vector3(0, -7.15f, Random.Range(0, 8));
        Instantiate(animalPrefabs[animalIndex], spawnPos , Quaternion.Euler(new Vector3(0, 90, 0)));
    }
}