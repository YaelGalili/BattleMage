using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;
    //=======================================For GameObjects==================
    private void OnMouseEnter()
    {
        TooltipManager._instance.ShowTooltip(message);
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideTooltip();
    }
    //=======================================For Buttons=========================
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        TooltipManager._instance.ShowTooltip(message);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        TooltipManager._instance.HideTooltip();
    }
}

//public class Tooltip : MonoBehaviour
//{
//    public string message;

//    private void OnMouseEnter()
//    {
//        TooltipManager._instance.ShowTooltip(message);
//    }

//    private void OnMouseExit()
//    {
//        TooltipManager._instance.HideTooltip();
//    }
//}
