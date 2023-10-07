using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cooldown : MonoBehaviour
{
    [SerializeField] float cooldown = 3f;
    private float _timer;

    Animator animator;

    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (!animator.GetBool("offCooldown")) {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) {
                animator.SetBool("offCooldown", true);
            }
        }
    }

    public void StartCooldown() {
        animator.SetBool("offCooldown", false);
        _timer = cooldown;
    }

}
