using TMPro;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    private GameObject obj;
    private GameManager GM;
    [SerializeField] private Transform place;
    [SerializeField] private Transform placeDrop;
    private Transform t;
    [SerializeField] private LayerMask layerMask;
    private TextMeshProUGUI PlayerLog;
    private Vector3 objOldScale;
    private void Start()
    {
        t = GetComponent<Transform>();
        PlayerLog = GameObject.Find("PlayerLog").GetComponent<TextMeshProUGUI>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (obj == null)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
        if (Input.GetMouseButtonDown(0)) 
        {   
            if (obj != null) 
            {
                UseObject(obj);
            }
        }
    }
    private void PickUp() 
    {
        
        RaycastHit hit;
        if (Physics.Raycast(t.position, t.forward, out hit, 3f, layerMask))
        {
            if (hit.collider.gameObject.tag == "Pickable")
            {
            obj = hit.collider.gameObject;
            hit.collider.transform.parent = place;
            hit.collider.transform.localPosition *= 0f;
            hit.collider.transform.localRotation = new Quaternion(0f,0f,0f,0f);
            obj.GetComponent<Rigidbody>().isKinematic = true;
            objOldScale = obj.GetComponent<Transform>().localScale;
            obj.GetComponent<Transform>().localScale = objOldScale * 0.3f;
            DisplayText(obj.GetComponent<InteractableObject>().DisplayedName);
            Debug.DrawRay(t.position, t.forward * hit.distance, Color.red, 2, false);
            Debug.Log("Picked up");
            if (!obj.GetComponent<InteractableObject>().found)
            {
                obj.GetComponent<InteractableObject>().found = true;
                GM.ItemFound(obj.GetComponent<InteractableObject>());
            }
            }
            else if (hit.collider.gameObject.tag == "Touchable")
            {
                 hit.collider.gameObject.SendMessage("Press");
            }
        }
        else
        {
            Debug.DrawRay(t.position, t.forward * 3f, Color.red, 2, false);
            Debug.Log("Didn't find anything");
        }
    }
    private void Drop() 
    {
        obj.transform.parent = null;
        obj.GetComponent<Transform>().localScale = objOldScale;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1f, ForceMode.Impulse);
        obj = null;
        DisplayText("");
        Debug.Log("Dropped");
    }
    private void UseObject(GameObject objectToUse) 
    {
        InteractableObject io;
        if (objectToUse.TryGetComponent<InteractableObject>(out io)) 
        {
            RaycastHit hit;
            if (Physics.Raycast(t.position, t.forward, out hit, 3f, ~11))
            {
                io.Use(hit.transform.gameObject);
            }
        }
    }
    private void DisplayText(string text) 
    {
        PlayerLog.text = text;
    }
    public void DestroyObjInHand() 
    {
        GM.ItemUsed(obj.GetComponent<InteractableObject>());
        Destroy(obj.gameObject);
        DisplayText("");
    }
}