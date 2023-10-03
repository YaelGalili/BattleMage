using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] bool disableOnHit = false;
    [SerializeField] public Collider2D target;
    [SerializeField] bool attackOnStay = false;
    private bool _enabled = true;
    

    Collider2D attackCollider;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void Awake() {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (_enabled & attackOnStay) {
            if (target != null) {
                if (!GameObject.ReferenceEquals(collision, target))
                    return;
            }
            // check if can be damaged
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null) {
                if (disableOnHit)
                    _enabled = false;
                Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                // damage the target
                bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
                if (gotHit)
                    Debug.Log(collision.name + " hit for " + attackDamage);
            }
        }
    }


    protected void OnTriggerEnter2D(Collider2D collision) {
        if (_enabled & !attackOnStay) {
            if (target != null) {
                if (!GameObject.ReferenceEquals(collision, target))
                    return;
            }
            // check if can be damaged
            Damageable damageable = collision.GetComponent<Damageable>();



            if (damageable != null) {
                if (disableOnHit)
                    _enabled = false;
                Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                // damage the target
                bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
                if (gotHit)
                    Debug.Log(collision.name + " hit for " + attackDamage);
            }
        }
    }
}
