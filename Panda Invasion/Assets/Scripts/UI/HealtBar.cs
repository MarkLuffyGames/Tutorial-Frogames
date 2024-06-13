using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    private float maxHealt = 100;
    private float currentHealt;
    private Image fillingImage;

    private void Start()
    {
        fillingImage = GetComponent<Image>();
        currentHealt = maxHealt;
        UpdateHealtBar();
    }

    private void UpdateHealtBar()
    {
        float percentage = currentHealt / maxHealt;
        fillingImage.fillAmount = percentage;
    }

    public bool ApplyDamage(int damage)
    {
        currentHealt -= damage;
        if(currentHealt > 0) 
        {
            UpdateHealtBar();
            return false;
        }

        currentHealt = 0;
        UpdateHealtBar();
        return true;
    }
}
