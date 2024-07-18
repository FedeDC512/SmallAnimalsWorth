using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private UnitScript playerUnitScript;
    private UnitScript enemyUnitScript;
    public GameObject camera;
    public GameObject camera2;
    public GameObject fences;
    public GameObject attackManager;
    public GameObject singleAttackManager;

    private Animator playerAnimator;
    private Animator enemyAnimator;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public GameObject buttons;
    public GameObject menuButton;
    public GameObject nextButton;
    public GameObject retryButton;
    public GameObject maxHPbuttons;

    public AudioSource attackSound;
    public AudioSource healSound;
    public GameObject multiAttackSound;

    /*Vector3 startCameraPosition = new Vector3 (-5.22f, 1.89f, -4.32f);
    Vector3 startCameraRotation = new Vector3 (30, -60f, 0);
    Vector3 battleCameraPosition = new Vector3 (-7.55f, 3.9f, -11.34f);
    Vector3 battleCameraRotation = new Vector3 (40, 0, 0);
    float cameraSpeed = 2f;*/


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    /*void Update(){
        if (camera.gameObject.active) playerUnitScript.resetPosition();
    }*/

    IEnumerator SetupBattle(){
        GameObject player = Instantiate(playerPrefab);
        playerUnitScript = player.GetComponent<UnitScript>();
        playerAnimator = player.transform.GetChild(0).gameObject.GetComponent<Animator>();
        
        GameObject enemy = Instantiate(enemyPrefab);
        enemyUnitScript = enemy.GetComponent<UnitScript>();
        enemyAnimator = enemy.transform.GetChild(0).gameObject.GetComponent<Animator>();

        dialogueText.SetText("A wild " + enemyUnitScript.unitName + " approaches!");

        playerHUD.SetHUD(playerUnitScript);
        enemyHUD.SetHUD(enemyUnitScript);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    void PlayerTurn(){
        dialogueText.SetText("Chose an action:");
        if(playerUnitScript.currentHP >= playerUnitScript.maxHP){
            maxHPbuttons.gameObject.SetActive(true);
        }
        else buttons.gameObject.SetActive(true);
    }

    public void MaxHPButton(){
        dialogueText.SetText(playerUnitScript.unitName + "'s HP are already full!");
    }

    public void OnAttackButton(){
        if (state != BattleState.PLAYERTURN) return;
        buttons.gameObject.SetActive(false);
        maxHPbuttons.gameObject.SetActive(false);
        
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton(){
        if (state != BattleState.PLAYERTURN) return;
        buttons.gameObject.SetActive(false);
        maxHPbuttons.gameObject.SetActive(false);

        playerAnimator.SetTrigger("isEating");
        
        StartCoroutine(PlayerHeal());
    }
    
    IEnumerator PlayerAttack(){
        dialogueText.SetText(playerUnitScript.unitName + " attacked " + enemyUnitScript.unitName + "! Dealt " + playerUnitScript.damage +" damage!");

        yield return new WaitForSeconds(0.5f);

        bool isDead = enemyUnitScript.TakeDamage(playerUnitScript.damage);
        enemyHUD.SetHP(enemyUnitScript.currentHP);

        playerAnimator.SetTrigger("isAttack");
        enemyAnimator.SetTrigger("getHit");
        attackSound.Play();

        yield return new WaitForSeconds(1.5f);

        if(isDead){
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal(){
        dialogueText.SetText(playerUnitScript.unitName + " ate something! Healed " + playerUnitScript.foodPower + " HP!");
        healSound.Play();

        yield return new WaitForSeconds(0.5f);

        playerUnitScript.Heal(playerUnitScript.foodPower);
        playerHUD.SetHP(playerUnitScript.currentHP);

        yield return new WaitForSeconds(1.5f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    
    IEnumerator EnemyTurn(){
        //randomize the attack of the enemy
        int random = Random.Range(1, 4);
        switch (random) 
        {
        case 1:
            StartCoroutine(EnemyMultipleAttack());
            yield return new WaitForSeconds(13f);
            break;
        case 2:
            StartCoroutine(EnemySingleAttack());
            yield return new WaitForSeconds(2f);
            break;
        case 3:
            StartCoroutine(EnemyBananaAttack());
            yield return new WaitForSeconds(10f);
            break;
        }

        bool isDead = playerUnitScript.DeathCheck();
        playerHUD.SetHP(playerUnitScript.currentHP);
        Debug.Log("End enemy turn");

        if(isDead){
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else{
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyMultipleAttack(){
        dialogueText.SetText(enemyUnitScript.unitName + " calls for help!");

        yield return new WaitForSeconds(1f);
        multiAttackSound.gameObject.SetActive(true);

        //camera.transform.position = Vector3.Lerp(transform.position, battleCameraPosition, cameraSpeed*Time.deltaTime);
        //camera.transform.rotation.eulerAngles = Vector3.Lerp(transform.rotation, battleCameraRotation, cameraSpeed);
        playerUnitScript.activateMovement();
        attackManager.gameObject.GetComponent<AttackManager>().StartFire();
        
        enemyAnimator.SetBool("multipleAttack", true);
        camera.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
        fences.gameObject.SetActive(true);
        //attacks from the enemy

        yield return new WaitForSeconds(7f);
        
        attackManager.gameObject.GetComponent<AttackManager>().StopFire();

        yield return new WaitForSeconds(3f);
        enemyAnimator.SetBool("multipleAttack", false);
        yield return new WaitForSeconds(2f);
        
        multiAttackSound.gameObject.SetActive(false);


        playerUnitScript.deactivateMovement();
        camera2.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
        fences.gameObject.SetActive(false);
        playerUnitScript.resetPosition();

        playerAnimator.SetBool("isMoving", false);
        //camera.transform.position = Vector3.Lerp(transform.position, startCameraPosition, cameraSpeed);
        //camera.transform.rotation = Vector3.Lerp(transform.rotation, startCameraRotation, cameraSpeed);
    }

    IEnumerator EnemySingleAttack(){
        dialogueText.SetText(enemyUnitScript.unitName + " attacks! Dealt " + enemyUnitScript.damage * 2 +" damage!");
        
        yield return new WaitForSeconds(0.5f);

        playerUnitScript.TakeDamage(enemyUnitScript.damage * 2);
        playerHUD.SetHP(playerUnitScript.currentHP);

        playerAnimator.SetTrigger("getHit");
        enemyAnimator.SetTrigger("isAttack");
        attackSound.Play();

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator EnemyBananaAttack(){
        dialogueText.SetText(enemyUnitScript.unitName + " is throwing food at " + playerUnitScript.unitName + "!");

        yield return new WaitForSeconds(1f);

        playerUnitScript.activateMovement();
        singleAttackManager.gameObject.GetComponent<SingleAttackManager>().StartFire();
        camera.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
        fences.gameObject.SetActive(true);
        //attacks from the enemy

        yield return new WaitForSeconds(0.5f);
        enemyAnimator.SetBool("bananaAttack", true);
        yield return new WaitForSeconds(6.5f);
        
        singleAttackManager.gameObject.GetComponent<SingleAttackManager>().StopFire();
        enemyAnimator.SetBool("bananaAttack", false);

        yield return new WaitForSeconds(2f);

        playerUnitScript.deactivateMovement();
        camera2.gameObject.SetActive(false);
        camera.gameObject.SetActive(true);
        fences.gameObject.SetActive(false);
        playerUnitScript.resetPosition();
        
        playerAnimator.SetBool("isMoving", false);
    }

    public void HitByEntity(){
        Debug.Log("Hit by enemy");
        dialogueText.SetText(playerUnitScript.unitName + " got hit by " + enemyUnitScript.unitName + "'s friends! Dealt " + enemyUnitScript.damage +" damage!");
        playerUnitScript.TakeDamage(enemyUnitScript.damage);
        playerHUD.SetHP(playerUnitScript.currentHP);
    }

    public void HitByProjectile(){
        Debug.Log("Hit by enemy");
        dialogueText.SetText(playerUnitScript.unitName + " got hit by " + enemyUnitScript.unitName + "'s shot! Dealt " + enemyUnitScript.damage +" damage!");
        playerUnitScript.TakeDamage(enemyUnitScript.damage);
        playerHUD.SetHP(playerUnitScript.currentHP);
    }
    

    public IEnumerator EndBattle(){
        if (state == BattleState.WON){
            dialogueText.SetText("You won!");
            playerAnimator.SetBool("battleWon", true);
            enemyAnimator.SetBool("battleWon", true);
            
            nextButton.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);
            playerAnimator.SetBool("isDead", true);
            }
        else {
            dialogueText.SetText("You lost!");
            playerAnimator.SetBool("isDead", true);
            enemyAnimator.SetBool("isDead", true);

            retryButton.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);
            playerAnimator.SetBool("isDead", true);
        }
        menuButton.gameObject.SetActive(true);
    }
}
