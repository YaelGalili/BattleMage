using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField] float amplitude = 0.2f;
    [SerializeField] float speed = 4f;
    private float y0;
    private Vector3 _tempPos;
    

    private void Awake() {
        y0 = transform.position.y;
        _tempPos = transform.position;
        speed += Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        _tempPos.y = y0 + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = _tempPos;
    }
}
