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


    public Ability[] abilities = new Ability[4];
    public List<Spell> chosenSpells = new List<Spell>();

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

    public void CastSpell() {

    }

    public void CastFireBall() {
        projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Fireball);
    }

    public void CastEarthenSPike() {
        CastAtClosestEnemy(earthenSpikePrefab, 0.7f, true, true);
    }


    public void CastLightningStorm() {
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
        spellDict.Add(Spell.LightningStorm, lightningStormPrefab);
        spellDict.Add(Spell.Fireball, fireballPrefab);
        spellDict.Add(Spell.EarthenSpike, earthenSpikePrefab);
        spellDict.Add(Spell.Tornado, tornadoPrefab);
        spellDict.Add(Spell.ArcaneBlast, arcaneBlastPrefab);
        spellDict.Add(Spell.FireBlade, fireBladePrefab);
        spellDict.Add(Spell.FlameStrike, flameStrikePrefab);
        spellDict.Add(Spell.Phoenix, phoenixPrefab);




        chosenSpells.Add(Spell.LightningStorm);
        //chosenSpells.Add(Spell.Fireball);
        
        //chosenSpells.Add(Spell.Tornado);
        chosenSpells.Add(Spell.EarthenSpike);
       
        chosenSpells.Add(Spell.FlameStrike);
        //chosenSpells.Add(Spell.ArcaneBlast);
        
        chosenSpells.Add(Spell.FireBlade);
        //chosenSpells.Add(Spell.Phoenix);
    }

    public void LoadSpellsFomData(int[] abilities) {
        if (abilities[0] == 0)
            chosenSpells.Add(Spell.LightningStorm);
        else
            chosenSpells.Add(Spell.Fireball);
        
        if (abilities[1] == 0)
            chosenSpells.Add(Spell.EarthenSpike);
        else
            chosenSpells.Add(Spell.Tornado);
        
        if (abilities[2] == 0)
            chosenSpells.Add(Spell.ArcaneBlast);
        else
            chosenSpells.Add(Spell.FlameStrike);
        
        if (abilities[3] == 0)
            chosenSpells.Add(Spell.Phoenix);
        else
            chosenSpells.Add(Spell.FireBlade);
    }
}
