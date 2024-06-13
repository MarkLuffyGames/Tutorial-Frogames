using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SugarPoints : MonoBehaviour
{
    private TextMeshProUGUI pointsText;
    private int points;

    void Start()
    {
        pointsText = GetComponent<TextMeshProUGUI>();
        UpdatePoints();
    }

    public void AddPoints(int points)
    {
        this.points += points;
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        pointsText.text = points.ToString();
    }
}
