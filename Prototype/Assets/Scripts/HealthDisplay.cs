using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

    public int playerId;

    Text _text;
    float currentHealthPercent;

    // Use this for initialization
    void Start()
    {
        _text = gameObject.GetComponent<Text>();
    }

    public void UpdateHealthDisplay(int currentHealth, int maxHealth)
    {
        currentHealthPercent = currentHealth * 100.0f / maxHealth;

        if (currentHealthPercent < 0)
            currentHealthPercent = 0;
        else if (currentHealthPercent > 100)
            currentHealthPercent = 100;

        _text.text = "Health: " + (int)currentHealthPercent;
    }
}
