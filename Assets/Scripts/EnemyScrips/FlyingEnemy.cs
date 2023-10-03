using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : Enemy {

    private float prev_x, curr_x;
    private bool disabled_components = false;
    private ChaseTarget chaseTarget;

    private void Start() {
        chaseTarget = gameObject.GetComponent<ChaseTarget>();
        GameObject player = GameObject.FindWithTag("Player");
        chaseTarget.target = player.transform;
    }

    private void FixedUpdate() {
        if (!damageable.LockVelocity && damageable.IsAlive)
            rb.velocity = new Vector2(0, 0);
        if (!damageable.IsAlive) {
            if (!disabled_components) {
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
                disabled_components = true;
            }
            
            rb.gravityScale = 3;
            Debug.Log("Flying Enemy Dead");
        }
        else {
            curr_x = rb.transform.position.x;
            SetDirection();
            prev_x = curr_x;
        }
    }

    private void SetDirection() {
        if (!damageable.LockVelocity) {
            if (curr_x > prev_x) {
                WalkDirection = WalkableDirection.Right;
            }
            else if (curr_x < prev_x) {
                WalkDirection = WalkableDirection.Left;
            }
        }
    }
}
