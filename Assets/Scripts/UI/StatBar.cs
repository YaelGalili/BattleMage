using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] private Damageable damageable;
    private Image content;

    private void Start()
    {
        content = GetComponent<Image>();
        UpdateStatBar();
        damageable.OnHealthChanged += UpdateStatBar;
    }

    private void UpdateStatBar()
    {
        float healthPercentage = (float)damageable.Health / damageable.MaxHealth;
        content.fillAmount = healthPercentage;
        Color fullHealthColor = Color.green;
        Color lowHealthColor = Color.red;
        content.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }

    private void OnDestroy()
    {
        damageable.OnHealthChanged -= UpdateStatBar;
    }

    private void Update()
    {
        
    }
}
