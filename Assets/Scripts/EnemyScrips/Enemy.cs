using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    [SerializeField] protected float walkSpeed = 3f;
    [SerializeField] protected DetectionZone attackZone;

    protected Rigidbody2D rb;
    protected TouchingDirections touchingDirections;
    protected Damageable damageable;
    protected Animator animator;

    public enum WalkableDirection { Right, Left }
    public bool _hasTarget = false;

    private WalkableDirection _walkDirection;
    [SerializeField] protected Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value) {
                // flip
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right) {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left) {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool HasTarget {
        get { return _hasTarget; }
        private set {
            _hasTarget = value;
            animator.SetBool("hasTarget", value);
        }
    }

    public bool CanMove { get { return animator.GetBool("canMove"); } }

    public void Awake() {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
    }


    protected void FlipDirection() {
        if (WalkDirection == WalkableDirection.Right) {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left) {
            WalkDirection = WalkableDirection.Right;
        }
        else {
            Debug.LogError("Walk direction is not set to Right or Left");
        }
    }


    // Update is called once per frame
    void Update() {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    public void OnHit(int damage, Vector2 knockback) {
        damageable.LockVelocity = true;
        rb.velocity = new Vector2(rb.velocity.x + knockback.x, rb.velocity.y + knockback.y);
    }
}
