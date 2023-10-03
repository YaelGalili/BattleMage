using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlipDirection : MonoBehaviour
{
    private int randomInt;
    private float _interval = 1f;
    private float _delay = 1.5f;
    private float _timer = 0f;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _interval) {
            _timer = 0f;
            Flip();
        }
        if (_timer >= _delay) {
            randomInt = Random.RandomRange(0, 200);
            if (randomInt == 1) {
                Flip();
            }
        }
    }

    void Flip() {
        gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
    }
}
