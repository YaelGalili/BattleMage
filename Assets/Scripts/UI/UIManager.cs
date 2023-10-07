using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Spellbook")]
    [SerializeField] private GameObject spellbookScreen;

    [SerializeField] GameObject FireballButton;
    [SerializeField] GameObject LightningStormButton;
    [SerializeField] GameObject TornadoButton;
    [SerializeField] GameObject EarthenSpikeButton;
    [SerializeField] GameObject ArcaneBlastButton;
    [SerializeField] GameObject FlameStrikeButton;
    [SerializeField] GameObject PhoenixButton;
    [SerializeField] GameObject FireBladeButton;


    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        spellbookScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (spellbookScreen.activeInHierarchy)
            {
                ShowSpellbook(false);
            }
            else
            {
                PauseGame(!pauseScreen.activeInHierarchy);
            }
        }
    }

    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //Quit game/exit play mode if in Editor
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        Pause(status);
    }

    private void Pause(bool status)
    {
        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion

    #region Spellbook
    public void ShowSpellbook(bool status)
    {
        ExperienceSystem expSys = GameObject.Find("Player").GetComponent<ExperienceSystem>();
        SpellCaster spellCaster = GameObject.Find("Player").GetComponent<SpellCaster>();
        spellbookScreen.SetActive(status);

        PhoenixButton.GetComponent<Button>().interactable = expSys.Level >= 5;
        FireBladeButton.GetComponent<Button>().interactable = expSys.Level >= 5;
        

        ArcaneBlastButton.GetComponent<Button>().interactable = expSys.Level >= 4;
        FlameStrikeButton.GetComponent<Button>().interactable = expSys.Level >= 4;

        EarthenSpikeButton.GetComponent<Button>().interactable = expSys.Level >= 3;
        TornadoButton.GetComponent<Button>().interactable = expSys.Level >= 3;
        

        FireballButton.GetComponent<Button>().interactable = expSys.Level >= 2;
        LightningStormButton.GetComponent<Button>().interactable = expSys.Level >= 2;
        


        if (spellCaster.Abilities[0] != null) {
            if (spellCaster.Abilities[0].spell == SpellCaster.Spell.LightningStorm) {
                FireballButton.GetComponent<Button>().interactable = false;
            }
            if (spellCaster.Abilities[0].spell == SpellCaster.Spell.Fireball)
                LightningStormButton.GetComponent<Button>().interactable = false;
        }
        if (spellCaster.Abilities[1] != null) {
            if (spellCaster.Abilities[1].spell == SpellCaster.Spell.Tornado) {
                EarthenSpikeButton.GetComponent<Button>().interactable = false;
            }
            if (spellCaster.Abilities[1].spell == SpellCaster.Spell.EarthenSpike)
                TornadoButton.GetComponent<Button>().interactable = false;
        }
        if (spellCaster.Abilities[2] != null) {
            if (spellCaster.Abilities[2].spell == SpellCaster.Spell.FlameStrike) {
                ArcaneBlastButton.GetComponent<Button>().interactable = false;
            }
            if (spellCaster.Abilities[2].spell == SpellCaster.Spell.ArcaneBlast)
                FlameStrikeButton.GetComponent<Button>().interactable = false;
        }
        if (spellCaster.Abilities[3] != null) {
            if (spellCaster.Abilities[3].spell == SpellCaster.Spell.FireBlade) {
                PhoenixButton.GetComponent<Button>().interactable = false;
            }
            if (spellCaster.Abilities[3].spell == SpellCaster.Spell.Phoenix)
                FireBladeButton.GetComponent<Button>().interactable = false;
        }

        Pause(status);
    }
    #endregion
}