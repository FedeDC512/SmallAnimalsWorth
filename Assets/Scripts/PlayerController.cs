using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private BattleSystem battleSystemScript;
    //private GameObject battleSystemObject;

    public float flashDuration;
    public int numberOfFlashes;
    public Collider triggerCollider;
    public GameObject mySprite;
    private Animator playerAnimator;
    public float rotationSpeed;
    private AudioSource attackSound;

    void Start()
    {
        battleSystemScript = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        playerAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        attackSound = GameObject.Find("AttackSound").GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);

        if(transform.position.x < -5.9f) transform.position = new Vector3(-5.9f, transform.position.y, transform.position.z);
        if(transform.position.x > 0.4f) transform.position = new Vector3(0.4f, transform.position.y, transform.position.z);
        if(transform.position.z < -16.3f) transform.position = new Vector3(transform.position.x, transform.position.y, -16.3f);
        if(transform.position.z > -9.8f) transform.position = new Vector3(transform.position.x, transform.position.y, -9.8f);

        transform.Translate(movementDirection * Time.deltaTime * speed);

        if (movementDirection != Vector3.zero){
            playerAnimator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.GetChild(0).gameObject.transform.rotation = Quaternion.RotateTowards(transform.GetChild(0).gameObject.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else playerAnimator.SetBool("isMoving", false);

    }

    private void OnTriggerEnter(Collider other) {
        StartCoroutine(FlashCo());
        if (other.CompareTag("Animal")){
            attackSound.Play();
            battleSystemScript.HitByEntity();
        }
        else if (other.CompareTag("Food")){
            attackSound.Play();
            battleSystemScript.HitByProjectile();
        }
    }

    private IEnumerator FlashCo(){
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp<numberOfFlashes && battleSystemScript.state == BattleState.ENEMYTURN){
            //if(battleSystemScript.state == BattleState.PLAYERTURN){
                mySprite.gameObject.SetActive(false);
                yield return new WaitForSeconds(flashDuration);
                mySprite.gameObject.SetActive(true);
                yield return new WaitForSeconds(flashDuration);
                temp++;
            //}
        }
        triggerCollider.enabled = true;
    }
}