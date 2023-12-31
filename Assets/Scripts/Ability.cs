using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using TMPro;

public class Ability //: MonoBehaviour
{
    private Image imageCooldown;
    private TMP_Text textCooldown;

    private bool onCooldown;
    private float cooldownTimer = 0.0f;

    public SpellCaster.Spell spell;
    public float cooldown;

    public Ability(int buttonIndex, int cooldown, SpellCaster.Spell spell) {
        this.cooldown = cooldown;
        this.spell = spell;
        SetUp(buttonIndex);
    }

    public void SetUp(int buttonIndex) {
        Transform btn = GameObject.Find("UserInterface").transform.Find("ActionBar").transform.Find("ActionButton1");

        imageCooldown = btn.transform.Find("CooldownFade" + buttonIndex).GetComponent<Image>();
        textCooldown = btn.transform.Find("CooldownText" + buttonIndex.ToString()).GetComponent<TMP_Text>();

        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0f;

       
        textCooldown.gameObject.SetActive(false);
    }

    public void StartCooldown() {
        cooldownTimer = cooldown;
        textCooldown.gameObject.SetActive(true);
        imageCooldown.fillAmount = 1.0f;
        onCooldown = true;
    }

    public void UpdateCoolDown() {
        if (onCooldown) {
            if(cooldownTimer < 0.0f) {
                textCooldown.gameObject.SetActive(false);
                imageCooldown.fillAmount = 0.0f;
                onCooldown = false;
            }
            else {
                textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
                imageCooldown.fillAmount = cooldownTimer / cooldownTimer;
            }
            cooldownTimer -= Time.deltaTime;
        }
    }
    /*
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
    */
    /* 
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
    */
}
