using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public int playerId;
    public Image fill;

    private float healthBarFillAmount;
    
    void Start()
    {
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarFillAmount = (float)currentHealth / maxHealth;

        fill.fillAmount = healthBarFillAmount;
    }
}
