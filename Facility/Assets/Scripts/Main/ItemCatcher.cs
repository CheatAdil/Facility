using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatcher : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) 
        {
            collision.gameObject.SendMessage("Respawn");
        }
    }
}
