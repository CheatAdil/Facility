using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject twin1;
    private GameObject twin2;
    private GameObject thug;
    private PlayerController player;
    private LootRandomer LR;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        twin1 = GameObject.Find("Twins");
        twin2 = GameObject.Find("Twins (1)");
        thug = GameObject.Find("Thug");
        ToggleEnemies();
    }
    public void SpawnEnemies(int enemy) 
    {
        ToggleEnemies(enemy);
    }
    private void ToggleEnemies(int a = 0) 
    {
        List<GameObject> ens = new List<GameObject>();
        switch (a) 
        {
            case 0:
                twin1.SetActive(false);
                twin2.SetActive(false);
                thug.SetActive(false); 
                break;
            case 1:
                thug.SetActive(true);
                ens.Add(thug);
                player.FindEnemies(ens);
                if (GameObject.Find("SpawnManager").TryGetComponent(out LR)) LR.StartSpawns(ens);
                break;
            case 2:
                twin1.SetActive(true);
                twin2.SetActive(true);
                ens.Add(twin1);
                ens.Add(twin2);
                player.FindEnemies(ens);
                if (GameObject.Find("SpawnManager").TryGetComponent(out LR)) LR.StartSpawns(ens);
                break;
            default:
                Debug.LogError("Enemy amount mismatch");
                break;          
        }
    }

}
