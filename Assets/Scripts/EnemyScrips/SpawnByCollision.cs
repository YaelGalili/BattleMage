using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnByCollision : MonoBehaviour
{

    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] protected DetectionZone detectPlayerZone;
    private int _charges = 1;


    // Update is called once per frame
    void Update()
    {

        if (detectPlayerZone.detectedColliders.Count > 0 && _charges >= 1) {
            Instantiate(enemyPrefab, spawnPoint.position, enemyPrefab.transform.rotation);
            _charges -= 1;
        }
        if (_charges <= 0)
            Destroy(gameObject);
    }
}
