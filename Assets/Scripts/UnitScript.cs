using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{

    public string unitName;
    public int unitLevel;
    public int damage;
    public int foodPower;
    public int maxHP;
    public int currentHP;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool TakeDamage (int dmg){
        currentHP -= dmg;
        return DeathCheck();
    }

    public bool DeathCheck(){
        if(currentHP <= 0){
            currentHP = 0;
            return true;
        }
        else return false;
    }

    public void Heal (int h){
        currentHP += h;
        if(currentHP > maxHP) currentHP = maxHP;
    }

    public void activateMovement(){
        GetComponent<PlayerController>().enabled = true;
    }

    public void deactivateMovement(){
        GetComponent<PlayerController>().enabled = false;
    }

    public void resetPosition(){
        transform.position = startPosition;
        transform.GetChild(0).gameObject.transform.rotation = startRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.GetChild(0).gameObject.transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

