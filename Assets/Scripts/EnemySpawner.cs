using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private GameObject swarmerEnemy;

    [SerializeField] private float interval = 6.5f;

    Rigidbody2D rb;
    Vector2 position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        position = gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(interval, swarmerEnemy));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy) 
    { 
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, position, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
