using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] private Damageable damageable;
    [SerializeField] private Image healthContent;
    [SerializeField] private Image xpContent;

    private void Start()
    {
        //content = GetComponent<Image>();
        UpdateStatBar();
        damageable.OnHealthChanged += UpdateStatBar;
    }

    private void UpdateStatBar()
    {
        float healthPercentage = (float)damageable.Health / damageable.MaxHealth;
        healthContent.fillAmount = healthPercentage;
        Color fullHealthColor = Color.green;
        Color lowHealthColor = Color.red;
        healthContent.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
    }

    private void OnDestroy()
    {
        damageable.OnHealthChanged -= UpdateStatBar;
    }

    private void Update()
    {
        
    }
}
