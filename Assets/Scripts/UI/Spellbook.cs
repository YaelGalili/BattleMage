using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spellbook : MonoBehaviour
{
    public static Spellbook _instance;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text headerText;
    [SerializeField] private Button[] spellsButtons2;
    [SerializeField] private Button[] spellsButtons3;
    [SerializeField] private Button[] spellsButtons4;
    [SerializeField] private Button[] spellsButtons5;

    List<Button[]> buttons = new List<Button[]>();
    private const string descriptionDefault = "";
    private const string headerDefault = "Spellbook";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        //spellsButtons2[0].onClick = 
    }

    private void Start()
    {
        buttons.Add(spellsButtons2);
        buttons.Add(spellsButtons3);
        buttons.Add(spellsButtons4);
        buttons.Add(spellsButtons5);
        foreach (Button[] buttonsLevel in buttons)
        {
            DisableLevelButtons(buttonsLevel);
        }
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        descriptionText.text = descriptionDefault;
        headerText.text = headerDefault;
    }

    public void SetAndShowHeader(string headerStr)
    {
        gameObject.SetActive(true);
        headerText.text = headerStr;
    }

    public void SetAndShowDescription(string descriptionStr)
    {
        gameObject.SetActive(true);
        descriptionText.text = descriptionStr;
    }

    private void EnableLevelButton(Button[] spellsLevel)
    {
        foreach (Button spell in spellsLevel)
        {
            spell.interactable = true;
        }
    }

    private void DisableLevelButtons(Button[] spellsLevel)
    {
        foreach (Button spell in spellsLevel)
        {
            spell.interactable = false;
        }
    }

    public void LevelUp(int level)
    {
        EnableLevelButton(buttons[level - 2]);
    }
}
