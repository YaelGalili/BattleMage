using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] Vector2 knockback = new Vector2(1f, 0);
    [SerializeField] Vector2 moveSpeed = new Vector2(7f, 0);
    [SerializeField] public Transform hitPosition;
    [SerializeField] bool pierceEnemies = false;


    Rigidbody2D rb;
    Animator animator;

    private bool _stop = false;

    public bool Stop {
        get { return _stop; }
        private set { 
            _stop = value;
            if (_stop == true)
                rb.velocity = new Vector2(0, 0);
        }
    }

    public void Hit() {
        animator.SetTrigger("hit");
        if (!pierceEnemies)
            Stop = true;
        gameObject.transform.position = hitPosition.position;
    }

    private void Awake() {        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Stop) {
            rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!Stop) {
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null) {
                if (damageable.IsAlive) {
                    Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                    // damage the target
                    bool gotHit = damageable.Hit(damage, deliveredKnockback);
                    if (gotHit)
                        Debug.Log(collision.name + " hit for " + damage);
                    Hit();
                }
            }
            else if (collision.gameObject.layer == 6) {
                Hit();
            }
        }
    }
}
