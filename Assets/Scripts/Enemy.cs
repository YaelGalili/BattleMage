using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Enemy : MonoBehaviour 
{ 
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] DetectionZone attackZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable damageable;
    Animator animator;

    public enum WalkableDirection { Right, Left }
    public bool _hasTarget = false;

    private WalkableDirection _walkDirection;
    [SerializeField] private Vector2 walkDirectionVector = Vector2.right;

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

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall) {
            FlipDirection();
        }

        if (CanMove && touchingDirections.IsGrounded)
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void FlipDirection() {
        if (WalkDirection == WalkableDirection.Right) {
            //Debug.Log("CHECK");
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left) {
            WalkDirection = WalkableDirection.Right;
        }
        else {
            Debug.LogError("Walk direction is not set to Right or Left");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    public void OnHit(int damage, Vector2 knockback) {
        damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
