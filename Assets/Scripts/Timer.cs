using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] GameObject portalPrefab;
    [SerializeField] Vector3 portalPosition;
    [SerializeField] TextMeshPro timerText;
    [SerializeField] float remainingTime = 600;

    private int _minutes;
    private int _seconds;
    private bool _over = false;

    private void Awake() {
        timerText = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
    
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            _minutes = Mathf.FloorToInt(remainingTime / 60);
            _seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("Survive for: {0:00}:{1:00}", _minutes, _seconds);
            timerText.text = string.Format("Survive for: {0:00}:{1:00}", _minutes, _seconds);
        }
        else if (remainingTime <= 0 && !_over)
        {
            _over = true;
            timerText.color = Color.green;
            timerText.text = "Victory!";
            Instantiate(portalPrefab, portalPosition, portalPrefab.transform.rotation); ;
        }
    }
}
