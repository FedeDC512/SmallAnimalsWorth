using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    public Image myFill;
    public Color maxHealthColor = Color.green;
    public Color minHealthColor = Color.red;

    public GameObject sliderFill;

    public void SetHUD (UnitScript unit){
        nameText.SetText(unit.unitName);
        levelText.SetText("Lv " + unit.unitLevel);
        hpSlider.maxValue = unit.maxHP;
        //hpSlider.value = unit.currentHP;
        SetHP(unit.currentHP);
    }

    public void SetHP(int hp){
        hpSlider.value = hp;
        myFill.color = Color.Lerp(minHealthColor, maxHealthColor, (float)hp / hpSlider.maxValue);
        hpText.SetText("HP: " + hp + "/" +hpSlider.maxValue);
        
        if(hp <= 0) sliderFill.SetActive(false);
    }
}
