using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleHuds : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    
    public void UpdateHud(Monsters monster)
    {
        nameText.text = monster.monsterName;
        levelText.text = "Lvl" + monster.monsterLevel;
        hpSlider.maxValue = monster.monsterMaxHP;
        hpSlider.value = monster.monsterCurrentHP;
    }

    public void UpdateHP(int hp)
    {
        hpSlider.value = hp;
    }
}
