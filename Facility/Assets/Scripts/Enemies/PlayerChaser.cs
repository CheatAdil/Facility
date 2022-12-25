using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChaser : MonoBehaviour
{
    private GameObject Enemy;
    private void Start()
    {
        Enemy = transform.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Enemy!= null) 
        {
            Enemy.SendMessage("ChasePlayer");
        }
    }
}
