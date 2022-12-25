using UnityEngine;
public class FirstControlPanel : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] InteractableObject key1;
    [SerializeField] InteractableObject key2;
    [SerializeField] private GameObject insertedkey1;
    [SerializeField] private GameObject insertedkey2;
    private void Start()
    {
        insertedkey1.SetActive(false);
        insertedkey2.SetActive(false);
    }
    private void Use(GameObject obj)
    {
        if (obj.GetComponent<InteractableObject>().name == key1.name) 
        {
            door.SendMessage("FirstOpen", obj);
            insertedkey1.SetActive(true);
            Destroy(obj); /// здесь нужно сделать анимацию - типа ключ вставлен
        }
        else if (obj.GetComponent<InteractableObject>().name == key2.name) 
        {
            door.SendMessage("SecondOpen", obj);
            insertedkey2.SetActive(true);
            Destroy(obj); /// здесь нужно сделать анимацию - типа ключ вставлен
        }
    }
}
