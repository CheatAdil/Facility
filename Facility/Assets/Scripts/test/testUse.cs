using UnityEngine;
public class testUse : MonoBehaviour
{
    [SerializeField] private GameObject UsedBy;
    private void Use(GameObject obj)
    {
        if (obj == UsedBy)
        {
            GetComponent<Transform>().localScale = new Vector3(1f,1f,1f);
            Debug.Log("Succesfull use");
        }
    }
}