using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporary : MonoBehaviour
{
    [SerializeField] float timeToRemove;
    

    private float _timer = 0f;
    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (_timer >= timeToRemove)
            animator.SetTrigger("remove");
        _timer += Time.deltaTime;
        
    }
}
