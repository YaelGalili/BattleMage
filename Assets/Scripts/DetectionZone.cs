using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public bool firstCollision = false;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider col;

    private void OnTriggerEnter2D(Collider2D collision) {
        detectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        firstCollision = true;
        detectedColliders.Remove(collision);
    }
}

