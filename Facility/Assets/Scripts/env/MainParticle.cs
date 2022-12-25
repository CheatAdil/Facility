using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainParticle : MonoBehaviour
{
    private Transform crystal;
    private Transform t;
    private void Start()
    {
        crystal = GameObject.Find("crystalPoint").GetComponent<Transform>();
        t = GetComponent<Transform>();
    }
    private void Update()
    {
        if (crystal != null) t.LookAt(crystal); 
    }
}
