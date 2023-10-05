using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public Image abilityIcon;
    public Image abilityBackground;
    public float cooldown;
    bool isCooldown = false;
    bool isValid = false;
    public KeyCode abilityKey;

    private void Start()
    {
        abilityBackground.fillAmount = 0;
    }

    private void Update()
    {
        if(Input.GetKeyUp(abilityKey) && !isCooldown)
        {
            isCooldown = true;
            abilityBackground.fillAmount = 1;
        }

        if (isCooldown)
        {
            abilityBackground.fillAmount -= 1/cooldown * Time.deltaTime;

            if (abilityBackground.fillAmount <= 0)
            {
                abilityBackground.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
}
