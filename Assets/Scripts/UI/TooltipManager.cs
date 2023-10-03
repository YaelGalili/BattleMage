using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;
    [SerializeField] private Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            backgroundRectTransform = GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ShowTooltip(string tooltipStr)
    {
        gameObject.SetActive(true);
        tooltipText.text = tooltipStr;
        float textPaddingSize = 4f * 2f;
        Vector2 backgoundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize, tooltipText.preferredHeight + textPaddingSize);
        backgroundRectTransform.sizeDelta = backgoundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltipText.text = string.Empty;
    }
}
