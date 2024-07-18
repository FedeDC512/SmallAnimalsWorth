using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackManager : MonoBehaviour
{
    public GameObject[] foodPrefab;
    private float startDelay = 1;
    private float spawnInterval = 1f;
    public AudioSource shootSound;

    public void StartFire()
    {
        InvokeRepeating("SpawnBananaBullets", startDelay, spawnInterval);
    }

    public void StopFire()
    {
        // Cancel all Invoke calls
        CancelInvoke();
    }

    void SpawnBananaBullets() {
        //randomize the spawn position of the animal
        StartCoroutine(FireBanana());
    }

    
    private IEnumerator FireBanana(){
        //int rotationDone = 0;
        int customRotation = 60;
        int randomSpawn = Random.Range(0, foodPrefab.Length);
        int randomRotation = Random.Range(25, 35);
        while (customRotation > -60){
            Instantiate(foodPrefab[randomSpawn], foodPrefab[randomSpawn].transform.position, Quaternion.Euler(new Vector3(0, customRotation, 0)));
            shootSound.Play();

            //spawn roated number each time
            yield return new WaitForSeconds(0.05f);
            //rotationDone += randomRotation;
            customRotation -= randomRotation;

            randomRotation = Random.Range(5, 15);
        }
    }

//Quaternion.Euler(new Vector3(0, -90, 0)));
//Quaternion.Euler(new Vector3(0, 90, 0)));
}
