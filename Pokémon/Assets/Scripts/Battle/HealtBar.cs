using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    [SerializeField] private Image healtBar;
    [SerializeField] private TextMeshProUGUI healtNumber;


    [SerializeField]private int MaxHP;
    [SerializeField]private int currentHP;

    public void SetHP(int hp, int maxHP)
    {
        currentHP = hp;
        MaxHP = maxHP;

        healtBar.fillAmount = currentHP / (float)MaxHP;
        if(healtNumber != null) healtNumber.text = $"{currentHP}/{MaxHP}";
    }

    public IEnumerator UpdateHealt(int hp)
    {
        while(currentHP > hp)
        {
            currentHP--;
            SetHP(currentHP, MaxHP);
            yield return null;
        }
        
        while(currentHP < hp)
        {
            currentHP++;
            SetHP(currentHP, MaxHP);
            yield return null;
        }
    }
}
