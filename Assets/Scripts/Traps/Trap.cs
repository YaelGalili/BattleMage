using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float activeTime;
    private Animator animator;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + attackDamage);
            }
        }

        StartCoroutine(ActivateTrap());
    }
    private IEnumerator ActivateTrap()
    {
        animator.SetBool("triggered", true);
        yield return new WaitForSeconds(activeTime);
        animator.SetBool("triggered", false);
    }
}
