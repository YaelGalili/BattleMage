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
    [SerializeField] private int maxSpellQueueSize = 2;

    public List<Spell> chosenSpells = new List<Spell>();

    private Queue<Spell> spellQueue = new Queue<Spell>();
    private Spell queuedSpell;

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
    

    public void CastSpell() {
        queuedSpell = spellQueue.Dequeue();
        switch (queuedSpell) {
            case Spell.LightningStorm:
                CastAtEnemies(7, 2);
                break;
            case Spell.Fireball:
                projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Fireball);
                break;
            case Spell.EarthenSpike:
                CastAtClosestEnemy(0.7f, true, true);
                break;
            case Spell.Tornado:
                projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Tornado);
                break;
            case Spell.ArcaneBlast:
                CastAtClosestEnemy(2f, false, false);
                break;
            case Spell.FlameStrike:
                CastAtEnemies(10, 0);
                break;
            case Spell.FireBlade:
                projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.FireBlade);
                break;
            case Spell.Phoenix:
                projectileLauncher.LaunchProjectile(ProjectileLauncher.ProjectileType.Phoenix);
                break;
            default:
                break;

        }
        queuedSpell = Spell.None;
    }


    private bool CastAtClosestEnemy(float yOffset=0, bool onlyGrounded=false, bool singleTarget=false) {
        float minDistance = 1200f, currDistance;
        Collider2D closestCollider = null;
        Spell spell = queuedSpell;

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
        Instantiate(spellDict[spell], gameObject.transform.position, spellDict[spell].transform.rotation);
        return false;
    }


    private bool CastAtEnemies(int maxHit, int yOffset=0, bool onlyGrounded=false) {
        Spell spell = queuedSpell;
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


    private GameObject CastAtPosition(Spell spell, Vector3 castPosition) {
        GameObject currSpell = Instantiate(spellDict[spell], castPosition, spellDict[spell].transform.rotation);

        Vector3 originalScale = currSpell.transform.localScale;
        currSpell.transform.localScale = new Vector3(
            originalScale.x * (transform.localScale.x > 0 ? 1 : -1),
            originalScale.y,
            originalScale.z
        );
        return currSpell;
    }


    public bool QueueSpell(Spell spell) { 
        spellQueue.Enqueue(spell);
        return true;
        /*if (spellQueue.Count < maxSpellQueueSize) {
            Debug.Log("meow");
            spellQueue.Enqueue(spell);
            return true;
        }
        return false;*/
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
       
        //chosenSpells.Add(Spell.FlameStrike);
        chosenSpells.Add(Spell.ArcaneBlast);
        
        chosenSpells.Add(Spell.FireBlade);
        //chosenSpells.Add(Spell.Phoenix);
    }
}
