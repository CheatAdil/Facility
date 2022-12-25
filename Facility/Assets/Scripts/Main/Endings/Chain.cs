using UnityEngine;
public class Chain : MonoBehaviour
{
    [SerializeField] private InteractableObject scisors;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private float[] angle;
    [SerializeField] private BoxCollider controlPanelBox;
    [SerializeField] private MeshCollider controlPanelMesh;

    private float timer;
    [SerializeField] private bool opened;
    private void Update()
    {
        if (opened)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].transform.Rotate(new Vector3(0f, angle[i] * Time.deltaTime, 0f));
            }
            if (timer >= 1f) Destroy(this.gameObject);
        }
    }
    private void Use(GameObject obj)
    {
        if (obj.GetComponent<InteractableObject>().name == scisors.name)
        {
            opened = true;
            GetComponent<MeshRenderer>().enabled = false;
            controlPanelBox.enabled = true;
            controlPanelMesh.enabled = true;
        }
    }
}
