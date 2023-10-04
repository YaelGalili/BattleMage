using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spellbook : MonoBehaviour
{
    private Text headerText;

    private void Awake()
    {
        headerText = GetComponent<Text>();
    }

    public void SetAndShowHeader(string tooltipStr)
    {
        gameObject.SetActive(true);
        headerText.text = tooltipStr;
    }
}
