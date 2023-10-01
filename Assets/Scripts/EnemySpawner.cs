using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private GameObject swarmerEnemy;

    private float interval = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(interval, swarmerEnemy));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy) 
    { 
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(5,5),Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
