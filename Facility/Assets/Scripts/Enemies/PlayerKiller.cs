using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    private GameObject monster;
    private Transform headpos;
    private Transform p;
    [SerializeField] private LayerMask LayersToIgnore;
    private float r;
    private void Start()
    {
        monster = transform.parent.gameObject;
        /// headpos = thug.GetComponent<Transform>().Find("Head");
        headpos = monster.GetComponent<Transform>();
        if (headpos == null) Debug.Log("SHIT");
        p = GameObject.Find("Player").GetComponent<Transform>();
        r = GetComponent<SphereCollider>().radius;
    }
    private void OnTriggerEnter(Collider other) => CheckForPlayer(other);
    private void OnTriggerStay(Collider other) => CheckForPlayer(other);
    private bool CheckForObstacles(Transform player) 
    {
        bool visible = false;
        RaycastHit hit;
        if (Physics.Raycast(headpos.position, (player.position - headpos.position), out hit, r, ~LayersToIgnore))
        {
            if (hit.collider.tag == "Player")
            {
                visible = true;
            }
        }
        return visible;
    }   
    private void CheckForPlayer(Collider other) 
    {
        if (other.gameObject.tag == "Player" && monster != null && CheckForObstacles(p))
        {
            monster.SendMessage("KillPlayer");
        }
    }
}

