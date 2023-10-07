using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlipDirection : MonoBehaviour
{
    private int randomInt;    
    float _delay = 1.5f;
    private float _timer = 0f;

    [SerializeField] private float interval = 1f;
    [SerializeField] int range = 200;

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= interval) {
            _timer = 0f;
            Flip();
        }
        if (_timer >= _delay) {
            randomInt = Random.Range(0, range);
            if (randomInt == 1) {
                Flip();
            }
        }
    }

    void Flip() {
        gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
    }
}
