using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float launchOffest = 0f;
    [SerializeField] Vector2 knockback = new Vector2(1f, 0);
    [SerializeField] Vector2 moveSpeed = new Vector2(7f, 0);

    Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null) {
            if (damageable.IsAlive) {
                Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                // damage the target
                bool gotHit = damageable.Hit(damage, deliveredKnockback);
                if (gotHit)
                    Debug.Log(collision.name + " hit for " + damage);
                Destroy(gameObject);
            }
        }


    }
}
