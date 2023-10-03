using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public bool firstCollision = false;
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider col;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        detectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        firstCollision = true;
        detectedColliders.Remove(collision);
    }
}

