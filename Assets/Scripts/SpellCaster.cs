using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] public GameObject lightningStormPrefab;
    [SerializeField] public GameObject fireballPrefab;
    [SerializeField] public GameObject earthenSpikePrefab;
    [SerializeField] public GameObject tornadoPrefab;
    [SerializeField] public GameObject arcaneBlastPrefab;
    [SerializeField] public GameObject fireBladePrefab;
    [SerializeField] public GameObject flameStrikePrefab;
    [SerializeField] public GameObject phoenixPrefab;
    [SerializeField] DetectionZone enemyDetectionZone;
    [SerializeField] ProjectileLauncher projectileLauncher;


    public static Ability[] abilities = new Ability[4];
    public List<Spell> chosenSpells = new List<Spell>();

    public Ability[] Abilities {
        get { return abilities; }
        set { abilities = value; }
    }

    private IEnumerator coroutine;

    Animator animator;

    public enum Spell { 
        None, 
        LightningStorm, 
        Fireball,
        EarthenSpike,
        Tornado,
        ArcaneBlast,
        Phoenix,
        FlameStrike,
        FireBlade
    }
    private Dictionary<Spell, GameObject> spellDict = new Dictionary<Spell, GameObject>();
    

    private bool CastAtClosestEnemy(GameObject spell, float yOffset=0, bool onlyGrounded=false, bool singleTarget=false) {
        float minDistance = 1200f, currDistance;
        Collider2D closestCollider = null;

        Collider2D selfCollider = gameObject.GetComponent<Collider2D>();

        // find nearest enemy
        if (enemyDetectionZone.detectedColliders.Count > 0) {
            foreach (Collider2D collider in enemyDetectionZone.detectedColliders) {
                // only hit enemies infront of the player
                if (transform.localScale.x > 0 && collider.transform.position.x < transform.position.x)
                    continue;
                if (transform.localScale.x < 0 && collider.transform.position.x > transform.position.x)
                    continue;
                // check - only hit grounded enemies
                if (onlyGrounded) {
                    if (!collider.GetComponentInParent<TouchingDirections>().IsGrounded)
                        continue;
                }
                // Only hit living enemies
                if (collider.GetComponentInParent<Damageable>().IsAlive) {
                    currDistance = Physics2D.Distance(selfCollider, collider).distance;
                    if (currDistance <= minDistance) {
                        minDistance = currDistance;
                        closestCollider = collider;
                    }
                }
            }

            // cast the spell at the nearest enemy
            if (closestCollider != null) {
                Vector3 enemyPosition = closestCollider.transform.position;
                Vector3 castPosition = new Vector3(enemyPosition.x, enemyPosition.y + yOffset, enemyPosition.z);
                
                GameObject currSpell = CastAtPosition(spell, castPosition);

                Attack attack = currSpell.transform.GetChild(0).gameObject.GetComponent<Attack>();
                if (singleTarget)
                    attack.target = closestCollider;
                return true;
            }
        }

        // no enemy found
        Instantiate(spell, gameObject.transform.position, spell.transform.rotation);
        return false;
    }


    private bool CastAtEnemies(GameObject spell, int maxHit, int yOffset=0, bool onlyGrounded=false) {
        bool hitAny = false;

        if (enemyDetectionZone.detectedColliders.Count > 0) {
            foreach (Collider2D collider in enemyDetectionZone.detectedColliders) {
                // Only hit living enemies
                if (collider.GetComponentInParent<Damageable>().IsAlive) {
                    // check - only hit grounded enemies
                    if (onlyGrounded) {
                        if (!collider.GetComponentInParent<TouchingDirections>().IsGrounded)
                            continue;
                    }
                    Vector3 enemyPosition = collider.transform.position;
                    Vector3 castPosition = new Vector3(enemyPosition.x, enemyPosition.y + yOffset, enemyPosition.z);
                    GameObject currSpell = CastAtPosition(spell, castPosition);
                    Attack attack = currSpell.transform.GetChild(0).gameObject.GetComponent<Attack>();
                    attack.target = collider;
                    
                    hitAny = true;
                    //currSpell.transform.parent = collider.transform;
                    maxHit -= 1;
                    if (maxHit == 0)
                        break;
                }
            }
        }
        return hitAny;

    }


    private GameObject CastAtPosition(GameObject spell, Vector3 castPosition) {
        GameObject currSpell = Instantiate(spell, castPosition, spell.transform.rotation);

        Vector3 originalScale = currSpell.transform.localScale;
        currSpell.transform.localScale = new Vector3(
            originalScale.x * (transform.localScale.x > 0 ? 1 : -1),
            originalScale.y,
            originalScale.z
        );
        return currSpell;
    }

    public void CastFireBall() {
        projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Fireball);
    }

    public void CastEarthenSPike() {
        CastAtClosestEnemy(earthenSpikePrefab, 0.7f, true, true);
    }


    public void CastLightningStorm() {
        abilities[0].StartCooldown();
        CastAtEnemies(lightningStormPrefab, 7, 2);
    }

    public void CastTornado() {
        projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Tornado);
    }

    public void CastArcaneBlast() {
        CastAtClosestEnemy(arcaneBlastPrefab, 2f, false, false);
    }

    public void CastFlameStrike() {
        CastAtEnemies(flameStrikePrefab, 10, 0);
    }

    public void CastPhoenix() {
        projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Phoenix);
    }

    public void CastFireBLade() {
        projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.FireBlade);
    }


    private void Awake() {
        animator = GetComponent<Animator>();

        // manual loading
        /*
        chosenSpells.Add(Spell.LightningStorm);
        //chosenSpells.Add(Spell.Fireball);
        
        //chosenSpells.Add(Spell.Tornado);
        chosenSpells.Add(Spell.EarthenSpike);
       
        chosenSpells.Add(Spell.FlameStrike);
        //chosenSpells.Add(Spell.ArcaneBlast);
        
        //chosenSpells.Add(Spell.FireBlade);
        chosenSpells.Add(Spell.Phoenix);
        */
    }

    public static void AddSpell(SpellCaster.Spell spell) {
        SpellCaster spellCaster = GameObject.Find("Player").transform.GetComponent<SpellCaster>();
        switch (spell) {
            case Spell.LightningStorm:
                if (spellCaster.Abilities[0] == null) {
                    Ability LightningStorm = new Ability(1, 5, Spell.LightningStorm);
                    spellCaster.Abilities[0] = LightningStorm;
                }
                break;
            case Spell.Fireball:
                if (spellCaster.Abilities[0] == null) {
                    Ability Fireball = new Ability(1, 4, Spell.Fireball);
                    spellCaster.Abilities[0] = Fireball;
                    spellCaster.chosenSpells.Add(spell);
                }
                break;
            case Spell.EarthenSpike:
                if (spellCaster.Abilities[1] == null) {
                    Ability EarthenSpike = new Ability(2, 5, Spell.EarthenSpike);
                    spellCaster.Abilities[1] = EarthenSpike;
                }
                break;
            case Spell.Tornado:
                if (spellCaster.Abilities[1] == null) {
                    Ability Tornado = new Ability(2, 3, Spell.Tornado);
                    spellCaster.Abilities[1] = Tornado;
                }
                break;
            case Spell.ArcaneBlast:
                if (spellCaster.Abilities[2] == null) {
                    Ability ArcaneBlast = new Ability(9, 5, Spell.ArcaneBlast);
                    spellCaster.Abilities[2] = ArcaneBlast;
                }
                break;
            case Spell.FlameStrike:
                if (spellCaster.Abilities[2] == null) {
                    Ability FlameStrike = new Ability(20, 5, Spell.FlameStrike);
                    spellCaster.Abilities[2] = FlameStrike;
                }
                break;
            case Spell.Phoenix:
                if (spellCaster.Abilities[3] == null) {
                    Ability Phoenix = new Ability(30, 5, Spell.Phoenix);
                    spellCaster.Abilities[3] = Phoenix;
                }
                break;
            case Spell.FireBlade:
                if (spellCaster.Abilities[3] == null) {
                    Ability FireBlade = new Ability(7, 5, Spell.FireBlade);
                    spellCaster.Abilities[3] = FireBlade;
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate() {
        for (int i = 0; i < 4; i++) {
            if (abilities[i] != null)
                abilities[i].UpdateCoolDown();
        }
    }

    private void Start() {
        for (int i = 0; i < 4; i++) {
            if (abilities[i] != null)
                abilities[i].SetUp(i+1);
        }
    }
}
