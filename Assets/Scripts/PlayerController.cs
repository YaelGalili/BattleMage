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
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip magicballSound;

    private UIManager uiManager;

    private int _xp = 0;
    private int _level = 1;
    private List<int> _levelBrackets = new List<int> { 30, 80, 150 , 250, 400};
    private Vector2 moveInput;
    private float _coyoteTime = 0.15f;
    private float _coyoteTimeCounter = 0f;

    Rigidbody2D rb;
    Animator animator;
    Damageable damageable;
    TouchingDirections touchingDirections;
    SpellCaster spellCaster;

    public int Level {
        get { return _level; }
    }

    public int LevelBracket {
        get {
            return _levelBrackets[Unity.Mathematics.math.min(_level-1, 3)]; }
    }

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

    public void GainXP(int xpToGain) {
        if (xpToGain > 0) {
            _xp += xpToGain;
            while (_xp >= LevelBracket && Level < 5) {
                _level += 1;
                Debug.Log("Level Up!");
            }
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
        GainXP(400);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        spellCaster = GetComponent<SpellCaster>();
        touchingDirections = GetComponent<TouchingDirections>();
        uiManager = FindObjectOfType<UIManager>();
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

        if (JumpAttack && touchingDirections.IsGrounded)
            rb.velocity = new Vector2(0, rb.velocity.y);

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
        if (Level >= 2 && spellCaster.queuedSpell == SpellCaster.Spell.None) {
            Debug.Log("OnAbility2");
            if (context.started) {
                if (spellCaster.chosenSpells[0] == SpellCaster.Spell.LightningStorm) {
                    // LightningStorm
                    Debug.Log("Cast LightningStorm");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("blueAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.LightningStorm;

                }
                else {
                    // Fireball
                    Debug.Log("Cast Fireball");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("redAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.Fireball;
                }
            }
        }
    }

    public void OnAbility3(InputAction.CallbackContext context) {
        if (Level >= 3 && spellCaster.queuedSpell == SpellCaster.Spell.None) {
            Debug.Log("OnAbility3");
            if (context.started) {
                if (spellCaster.chosenSpells[1] == SpellCaster.Spell.EarthenSpike) {
                    // EarthenSpike
                    if (touchingDirections.IsGrounded) {
                        Debug.Log("Cast EarthenSpike");
                        animator.SetTrigger("attack");
                        animator.SetTrigger("brownAttack");
                        spellCaster.queuedSpell = SpellCaster.Spell.EarthenSpike;
                    }
                }
                else {
                    // Tornado
                    Debug.Log("Cast Tornado");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("greenAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.Tornado;
                }
            }
        }
    }

    public void OnAbility4(InputAction.CallbackContext context) {
        if (Level >= 4 && spellCaster.queuedSpell == SpellCaster.Spell.None) {
            Debug.Log("OnAbility4");
            if (context.started) {
                if (spellCaster.chosenSpells[2] == SpellCaster.Spell.ArcaneBlast) {
                    // ArcaneBlast
                    Debug.Log("Cast ArcaneBlast");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("blueAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.ArcaneBlast;
                }
                else {
                    // FlameStrike
                    Debug.Log("Cast FlameStrike");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("redAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.FlameStrike;
                }
            }
        }
    }

    public void OnAbility5(InputAction.CallbackContext context) {
        if (Level >= 5 && spellCaster.queuedSpell == SpellCaster.Spell.None) {
            Debug.Log("OnAbility5");
            if (context.started) {
                Debug.Log(spellCaster.chosenSpells[3]);
                if (spellCaster.chosenSpells[3] == SpellCaster.Spell.Phoenix) {
                    // Phoenix
                    Debug.Log("Cast Phoenix");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("blueAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.Phoenix;
                }
                else {
                    // Phoenix
                    Debug.Log("Cast FireBlade");
                    animator.SetTrigger("attack");
                    animator.SetTrigger("redAttack");
                    spellCaster.queuedSpell = SpellCaster.Spell.FireBlade;
                }
            }
        }
    }


    public void OnHit(int damage, Vector2 knockback) {
        spellCaster.queuedSpell = SpellCaster.Spell.None;
        damageable.LockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnDeath()
    {
        uiManager.GameOver();
    }
}
