using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpImpulse = 6f;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isFacingRight = true;
    
    private Vector2 moveInput;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    TouchingDirections touchingDirections;


    public bool CanMove {
        get {
            return animator.GetBool("canMove");
        }
    }

    public float CurrentMoveSpeed {
        get {
            if (CanMove) {
                if (IsMoving && !touchingDirections.IsOnWall) {
                    return walkSpeed;
                }
                else {
                    return 0;
                }
            }
            else
                return 0;
        }
    }

    public bool IsMoving {
        get { return _isMoving; }
        private set {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    public bool IsFacingRight {
        get { return _isFacingRight; }
        private set {
            // Make the player face the opposite directon
            if (_isFacingRight != value) {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
 
    public bool IsAlive {
        get {
            return animator.GetBool("isAlive");
        }
    }


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {            
        
    }

    private void FixedUpdate() {
        // move
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        
        animator.SetFloat("yVelocity", rb.velocity.y);
        
        // only change direction if able to move
        if (moveInput != null && !damageable.LockVelocity && CanMove)
            SetFacingDirection(moveInput);
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive) {
            IsMoving = moveInput != Vector2.zero;

            if (!damageable.LockVelocity && CanMove)
                SetFacingDirection(moveInput);
        }
        else {
            IsMoving = false;
        }


    }

    private void SetFacingDirection(Vector2 moveInput) {
        if (moveInput.x > 0 && !IsFacingRight) {
            // Face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight) {
            // Face left
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        // TODO check if alive as well
        if (context.started && CanMove) {
            if (touchingDirections.IsGrounded) {
                animator.SetTrigger("jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            }
            else if(animator.GetBool("canDoubleJump")) {
                animator.SetTrigger("doubleJump");
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            }
            
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (context.started) {
            animator.SetTrigger("attack");
        }
    }

    public void OnHit(int damage, Vector2 knockback) {
        damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
