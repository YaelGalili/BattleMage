using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] public GameObject basicAttackPrefab;
    [SerializeField] public GameObject fireballPrefab;
    [SerializeField] public GameObject tornadoPrefab;
    [SerializeField] public GameObject phoenixPrefab;
    [SerializeField] public Transform launchPoint;

    public enum ProjectileType { 
        BasicAttack, 
        Fireball,
        Tornado,
        Phoenix
    }
    private Dictionary<ProjectileType, GameObject> projectileDict = new Dictionary<ProjectileType, GameObject> ();

    public void LaunchProjectile(ProjectileType projectileType) {
        GameObject projectile = Instantiate(projectileDict[projectileType], launchPoint.position, projectileDict[projectileType].transform.rotation);
    
        Vector3 originalScale = projectile.transform.localScale;

        projectile.transform.localScale = new Vector3(
            originalScale.x * (transform.localScale.x > 0 ? 1 : -1),
            originalScale.y,
            originalScale.z
            );
    }

    private void Awake() {
        LoadProjectileDict();
    }

    private void LoadProjectileDict() {
        projectileDict.Add(ProjectileType.BasicAttack, basicAttackPrefab);
        projectileDict.Add(ProjectileType.Fireball, fireballPrefab);
        projectileDict.Add(ProjectileType.Tornado, tornadoPrefab);
        projectileDict.Add(ProjectileType.Phoenix, phoenixPrefab);
    }
}
