using UnityEngine;
using UnityEngine.UI;

public class Spellbook : MonoBehaviour
{
    private const string descriptionDefault = "";
    private const string headerDefault = "Spellbook";


    [SerializeField] private Text descriptionText;
    [SerializeField] private Text headerText;

    private void Start()
    {
        //gameObject.SetActive(true);
        //descriptionText.text = descriptionDefault;
        //headerText.text = headerDefault;
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
}
