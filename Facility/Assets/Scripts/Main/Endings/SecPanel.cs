using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecPanel : MonoBehaviour
{
    private GameObject door;
    [SerializeField] InteractableObject key1;
    [SerializeField] InteractableObject key2;
    [SerializeField] InteractableObject key3;
    int interactionStage;

    private void Start()
    {
        interactionStage = 0;
        door = GameObject.Find("Vault Door");
    }

    private void Use(GameObject obj)
    {
        if (obj.GetComponent<InteractableObject>().name == key1.name)
        {
            interactionStage++;
            Check();
            Destroy(obj); 
        }
        else if (obj.GetComponent<InteractableObject>().name == key2.name) 
        {
            interactionStage++;
            Check();
            Destroy(obj);
        }
        else if (obj.GetComponent<InteractableObject>().name == key3.name) 
        {
            interactionStage++;
            Check();
            Destroy(obj);
        }
    }
    private void Check() 
    {
        if (interactionStage == 3) 
        {
            Debug.Log("Open");
            door.SendMessage("ThirdOpen");
        }
    }
}
