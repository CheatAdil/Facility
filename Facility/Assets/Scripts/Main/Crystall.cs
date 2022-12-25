using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Crystall : MonoBehaviour
{
    private GameObject levelManager;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MinLight, MaxLight;
    private Transform t;
    private Light l;
    private float timer;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager");
        t = GetComponent<Transform>();
        l = GetComponentInChildren<Light>();
        timer = 0;
    }

    private void Update()
    {
        IdleState();
        timer += Time.deltaTime;
    }

    private void IdleState() 
    {
        t.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);
        if (timer >= 0.05f)
        {
            l.intensity = Random.Range(MinLight, MaxLight);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        levelManager.SendMessage("Complete");
    }

}
