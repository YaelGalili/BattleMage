using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{

    [SerializeField] private ExperienceSystem experienceSystem;
    private Image content;

    private void Start()
    {
        content = GetComponent<Image>();
        UpdateXpBar();

    }


    private void UpdateXpBar() {
        float expPercentage = (float)experienceSystem.XP / experienceSystem.LevelBracket;
        content.fillAmount = expPercentage;
    }


    private void FixedUpdate()
    {
        UpdateXpBar();
    }
}
