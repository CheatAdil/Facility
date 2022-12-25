using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private statistics s;
    private EnemyManager EM;	
    private void Start()
    {
        Debug.Log("Starting level...");
        EM = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        InitializeGame(Random.Range(0, 2));
    }
    public void InitializeGame(int enemy) 
    {
        if (TryGetComponent(out s)) s.StartRecording(enemy + 1);       
        EM.SpawnEnemies(enemy + 1);
    }
    public void ItemUsed(InteractableObject intobj) => s.WriteItem(intobj, 1);
    public void ItemFound(InteractableObject intobj) => s.WriteItem(intobj, 0);
    public void AllSpawned(int count) => s.InitializeItemTable(count);
    public void EndGame(bool won = false) 
    {
        s.StopRecord(won);
    }
}
