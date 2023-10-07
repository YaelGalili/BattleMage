using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class GroundedEnemy : Enemy
{
    [SerializeField] protected DetectionZone cliffDetectionZone;


    private void FixedUpdate() {
        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall || 
            (touchingDirections.IsGrounded && cliffDetectionZone.detectedColliders.Count == 0 && cliffDetectionZone.firstCollision)) {
            FlipDirection();
        }

        if (CanMove && touchingDirections.IsGrounded && damageable.IsAlive)
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        else if (damageable.IsAlive) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        //else if (damageable.LockVelocity && touchingDirections.IsGrounded)
        //   rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
