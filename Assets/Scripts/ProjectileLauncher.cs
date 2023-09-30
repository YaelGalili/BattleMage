using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Transform launchPoint;

    public void LaunchProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
    
        Vector3 originalScale = projectile.transform.localScale;

        projectile.transform.localScale = new Vector3(
            originalScale.x * (transform.localScale.x > 0 ? 1 : -1),
            originalScale.y,
            originalScale.z
            );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
