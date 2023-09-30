using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _health;
    [SerializeField] private bool _isAlive = true;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private float invincibilityTime = 0.25f;


    Animator animator;
    private float timeSinceHit;
    

    public int MaxHealth {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    public int Health {
        get { return _health; }
        set { 
            _health = value;
            if (_health <= 0) {
                IsAlive = false;
                animator.SetBool("isAlive", _isAlive);
            }
        }
    }

    public bool IsAlive { get { return _isAlive; }
        set {
            _isAlive = value;
        }
    }

    public bool LockVelocity {
        get { return animator.GetBool("lockVelocity"); }
        set {
            animator.SetBool("lockVelocity", value);
        }
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Update() {
        if (isInvincible) {
            if (timeSinceHit > invincibilityTime) {
                // Remove Invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }


    // Applies damage and knockback to the object, returns whether the damagable object can be hit or not
    public bool Hit(int damage, Vector2 knockback) {
        if (IsAlive && !isInvincible) {
            Health -= damage;
            isInvincible = true;

            // Notify subscribed components that the object was hit
            Debug.Log("HIT");
            animator.SetTrigger("hit");
            damageableHit?.Invoke(damage, knockback);

            return true;
        }
        // Unable to be hit
        return false;
    }
}
