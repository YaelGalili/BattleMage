using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnNoMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    private Vector2 _prevPosition;
    private Vector2 _currPosition;
    private bool _firstIteration = true;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate() {
        _currPosition = rb.transform.position;
        if (!_firstIteration) {
            if (Unity.Mathematics.math.abs(Vector2.Distance(_prevPosition, _currPosition)) < 0.0001)
                animator.SetTrigger("remove");
        }
        _prevPosition = _currPosition;
        _firstIteration = false;


    }
}
