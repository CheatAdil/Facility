using UnityEngine;
public class VaultDoor : MonoBehaviour
{
    [SerializeField] private bool firstOpen;
    [SerializeField] private bool secondOpen;
    [SerializeField] private bool thirdOpen;
    [SerializeField] private bool codeEntered;
    private bool opened;
    [SerializeField] private int f;
    [SerializeField] private int s;
    private Vector3 newPos;
    private Transform t;
    [SerializeField] private float goDown;


    private bool Finished;
    private GameObject Victory;
    private void Start()
    {
        Victory = GameObject.Find("Victory");
        t = GetComponent<Transform>();
        goDown = t.position.y - 4;
        Finished = false;
    }
    private void Update()
    {
        if (Finished)
        {
            t.position = Vector3.MoveTowards(t.position, new Vector3(t.position.x, goDown, t.position.z), Time.deltaTime * 6f);
        }
    }
    private void FirstOpen(GameObject obj)
    {
        f++;
        if (f >= 2) firstOpen = true;
        CheckCompletion();
    }
    private void SecondOpen(GameObject obj)
    {
        s++;
        if (s>= 2) secondOpen = true;
        CheckCompletion();
    }
    private void ThirdOpen() 
    {
        thirdOpen = true;
        CheckCompletion();
    }
    private void CodeOpen()
    {
        codeEntered = true;
        CheckCompletion();
    }

    private void CheckCompletion()
    {
        if (firstOpen && secondOpen && thirdOpen && codeEntered)
        {
            Finished = true;
            Victory.SendMessage("EndGame");
        }
    }
}
