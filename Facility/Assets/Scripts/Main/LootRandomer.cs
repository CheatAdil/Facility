using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
public class LootRandomer : MonoBehaviour
{
   [SerializeField] private InteractableObject[] items;
   [SerializeField] private Transform[] positions;

    private GameManager gm;
    private List<GameObject> objectsToSpawn;
    private List<Transform> whereToSpawn;
    private List<GameObject> en;
    private bool AmountIsOkay;
    private void Start()
    {
        gm = GetComponentInParent<GameManager>();
    }
    public void StartSpawns(List<GameObject> e )
    {
        en = e;
        if (CheckId(items))
        {
            Debug.Log("IDs are Okay");
            if (items.Length > 0)
            {
                objectsToSpawn = new List<GameObject>();
                whereToSpawn = new List<Transform>();
                for (int i = 0; i < items.Length; i++)
                {
                    objectsToSpawn.Add(items[i].gameObject);
                }
                for (int i = 0; i < positions.Length; i++)
                {
                    whereToSpawn.Add(positions[i]);
                }
                if (objectsToSpawn.Count > whereToSpawn.Count)
                {
                    AmountIsOkay = false;
                    Debug.LogError("There are more objects than spawn positions!");
                }
                else AmountIsOkay = true;
                if (AmountIsOkay) ManageSpawns(objectsToSpawn, whereToSpawn);
            }
        }
        else 
        {
            Debug.LogError("ID Mismatch error");
        }
    }
    private void ManageSpawns(List<GameObject> ots, List<Transform> wts) 
    {
        int RandValue;
        do
        {
            RandValue = Random.Range(0, wts.Count);
            Spawn(ots[0], wts[RandValue]);
            ots.Remove(ots[0]); wts.Remove(wts[RandValue]);
        } while (ots.Count != 0);
        gm.AllSpawned(items.Length);
    }

    private void Spawn(GameObject what, Transform where) 
    {
       GameObject g = Instantiate(what, where.position, Quaternion.identity) as GameObject;
        g.SendMessage("FindEnemies", en);
       g.SendMessage("SetSpawnPoint", where.position);
    }

    private bool CheckId(InteractableObject[] objects) 
    {
        int[] ids = new int[objects.Length]; 
        bool result = false;
        for (int i = 0; i < objects.Length; i++) 
        {
            ids[i] = objects[i].id;
        }
        Sort(ids);
        if (CheckDuplicates(ids) && PostCheck(ids)) result = true;
        return result;
    }
    private void Sort(int[] arr) /// shell sort
    {
        int i, j, pos, temp;
        int n = arr.Length;
        pos = 3;
        while (pos > 0)
        {
            for (i = 0; i < n; i++)
            {
                j = i;
                temp = arr[i];
                while ((j >= pos) && (arr[j - pos] > temp))
                {
                    arr[j] = arr[j - pos];
                    j = j - pos;
                }
                arr[j] = temp;
            }
            if (pos / 2 != 0)
                pos = pos / 2;
            else if (pos == 1)
                pos = 0;
            else
                pos = 1;
        }
    }
    private bool CheckDuplicates(int[] arr) 
    {
        for (int i = 0; i < arr.Length - 1; i++) 
        {
            if (arr[i] == arr[i + 1]) 
            {
                return false;
            }
        }
        return true;
    }
    private bool PostCheck(int[] arr) 
    {
        if (arr[0] != 0) return false;
        bool result = false;
        for (int i = 0; i < arr.Length - 1; i++) 
        {
            if (arr[i] == arr[i + 1] - 1) result = true;
            else return false;
        }
        return result;
    }
    

}
