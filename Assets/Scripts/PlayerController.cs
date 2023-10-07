using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour {
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpImpulse = 6f;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isFacingRight = true;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip magicballSound;
    [SerializeField] bool testMode = false;

    private UIManager uiManager;

    private Vector2 moveInput;

    // for jumping
    private float _coyoteTime = 0.15f;
    private float _coyoteTimeCounter = 0f;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    TouchingDirections touchingDirections;
    ExperienceSystem experienceSystem;
    SpellCaster spellCaster;

  

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

    public bool CanDoubleJump {
        get { return animator.GetBool("canDoubleJump"); }
        private set {
            animator.SetBool("canDoubleJump", value);
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

    public bool JumpAttack { 
        get { return animator.GetBool("jumpAttack"); }
        private set {
            animator.SetBool("jumpAttack", value);
        } 
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        spellCaster = GetComponent<SpellCaster>();
        touchingDirections = GetComponent<TouchingDirections>();
        experienceSystem = GetComponent<ExperienceSystem>();
        uiManager = FindObjectOfType<UIManager>();
    }


    private void FixedUpdate() {
        // move
        if (!damageable.LockVelocity) {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);               
            
        }
        if (JumpAttack && touchingDirections.IsGrounded )
            rb.velocity = new Vector2(0, rb.velocity.y);

        // -- Wall Slide, not working properly 
        //if (!touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        //    rb.velocity = new Vector2(rb.velocity.x, -1.2f);

        animator.SetFloat("yVelocity", rb.velocity.y);
        
        // only change direction if able to move
        if (moveInput != null && !damageable.LockVelocity && CanMove && !(JumpAttack && touchingDirections.IsGrounded))
            SetFacingDirection(moveInput);

        if (touchingDirections.IsGrounded)
            _coyoteTimeCounter = _coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive) {
            IsMoving = moveInput != Vector2.zero;

            if (!damageable.LockVelocity && CanMove && !(JumpAttack && touchingDirections.IsGrounded))
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
        if (context.started && CanMove && IsAlive) {
            if (_coyoteTimeCounter > 0) {
                animator.SetTrigger("jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                _coyoteTimeCounter = 0;
            }
            else if (CanDoubleJump) {
                animator.SetTrigger("doubleJump");
                animator.SetBool("canDoubleJump", false);
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (context.started) {
            SoundManager.instance.PlaySound(magicballSound);
            animator.SetTrigger("attack");
            animator.SetTrigger("basicAttack");
            if (!touchingDirections.IsGrounded)
                animator.SetBool("jumpAttack", true);
        }
    }

    public void OnAbility2(InputAction.CallbackContext context) {
        if (experienceSystem.Level >= 2) {
            Debug.Log("OnAbility2");
            if (context.started && spellCaster.Abilities[0] != null) {
                if (spellCaster.Abilities[0].spell == SpellCaster.Spell.LightningStorm) {
                    // Lightning Storm
                    Debug.Log("Cast LightningStorm");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("lightningStorm");
                    }
                else {
                    // Fireball
                    Debug.Log("Cast Fireball");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("fireball");
                }
            }
        }
    }

    public void OnAbility3(InputAction.CallbackContext context) {
        if (experienceSystem.Level >= 3) {
            Debug.Log("OnAbility3");
            if (context.started && spellCaster.Abilities[1] != null) {
                if (spellCaster.Abilities[1].spell == SpellCaster.Spell.EarthenSpike) {
                    // EarthenSpike
                    if (touchingDirections.IsGrounded) {
                        Debug.Log("Cast EarthenSpike");
                        animator.SetTrigger("attack");
                        animator.SetTrigger("earthenSpike");
                    }
                }
                else {
                    // Tornado
                    Debug.Log("Cast Tornado");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("tornado");
                }
            }
        }
    }

    public void OnAbility4(InputAction.CallbackContext context) {
        if (experienceSystem.Level >= 4) {
            Debug.Log("OnAbility4");
            if (context.started && spellCaster.Abilities[2] != null) {
                if (spellCaster.Abilities[2].spell == SpellCaster.Spell.ArcaneBlast) {
                    // ArcaneBlast
                    Debug.Log("Cast ArcaneBlast");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("arcaneBlast");
                }
                else {
                    // FlameStrike
                    Debug.Log("Cast FlameStrike");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("flameStrike");
                }
            }
        }
    }

    public void OnAbility5(InputAction.CallbackContext context) {
        if (experienceSystem.Level >= 5) {
            Debug.Log("OnAbility5");
            if (context.started && spellCaster.Abilities[3] != null) {
                if (spellCaster.Abilities[3].spell == SpellCaster.Spell.Phoenix) {
                    // Phoenix
                    Debug.Log("Cast Phoenix");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("phoenix");
                }
                else {
                    // Fire Blade
                    Debug.Log("Cast FireBlade");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("fireBlade");
                }
            }
        }
    }


    public void OnHit(int damage, Vector2 knockback) {
        //spellCaster.queuedSpell = SpellCaster.Spell.None;
        //if(touchingDirections.IsGrounded) damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, knockback.y != 0 ? knockback.y * 1.5f : rb.velocity.y);
    }


    public void OnDeath()
    {
        uiManager.GameOver();
    }

    public void OnSaveGame(InputAction.CallbackContext context) {
        Debug.Log("OnSaveGame");
        if (context.started) 
            SaveSystem.SaveGame(experienceSystem.XP, experienceSystem.Level, SceneManager.GetActiveScene().buildIndex, spellCaster.chosenSpells);
    }
}
