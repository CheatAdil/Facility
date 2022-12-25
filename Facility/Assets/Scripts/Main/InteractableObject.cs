using UnityEngine;
using System.Collections.Generic;
using System;

public class InteractableObject : MonoBehaviour
{
    public int id; /// starts from 0
    public string name;
    public string DisplayedName;
    public GameObject interactsWith;
    [SerializeField] private string objectName;
    private Vector3 spawnpoint;


    private GameObject EnemyManager;
    private List<GameObject> enemies;
    private bool EnemiesConnected;

    private bool FirstCollisionIsDone;

    private Inventory inv;

    [NonSerialized] public bool found;
    private void Awake()
    {
        FirstCollisionIsDone = false;
    }

    private void Start()
    {
        if (interactsWith == null) interactsWith = GameObject.Find(objectName);
        inv = Camera.main.GetComponent<Inventory>();
        if (inv == null) Debug.LogError("Inventory is not connected");
    }
    public void Use(GameObject UseOn = null) 
    {
        if (UseOn == null)
        {
            Debug.LogError("Nothing to use on");
        }
        else if (UseOn == interactsWith) 
        {
            UseOn.SendMessage("Use", this.gameObject);
            inv.DestroyObjInHand();
        }
        else 
        {
            Debug.LogWarning("It is impossible to use " + name + "  on  " + UseOn.name );
        }
    }
    private void SetSpawnPoint(Vector3 point) 
    {
        spawnpoint = point;
    }
    private void Respawn() 
    {
        if (spawnpoint != null)
        {
            transform.position = spawnpoint;
            FirstCollisionIsDone = false;
        }
    }
    private void FindEnemies(List<GameObject> enemiesList)
    {
        enemies = enemiesList;
        if (enemies.Count != 0)
        {
            EnemiesConnected = true;
        }
        else
        {
            Debug.LogError("JSON: Failled to connect enemies");
            Debug.LogError("If their presence was not intended");
            Debug.LogError("just ignore this message");
        }
    }
    private void MakeNoise(string type)
    {
        if (EnemiesConnected)
        {
            Debug.Log("item you dropped made noise");
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].SendMessage("Noise", type);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 8 && !FirstCollisionIsDone) 
        {
            FirstCollisionIsDone = true;
        }
        else if (collision.collider.gameObject.layer == 8)
        {
            MakeNoise("itemDropped");
        }
    }
}