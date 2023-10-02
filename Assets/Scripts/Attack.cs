using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Collider2D attackCollider;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void Awake() {
        attackCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        // check if can be damaged
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null) {

            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // damage the target
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit)
                Debug.Log(collision.name + " hit for " + attackDamage);
        }
    }
}
