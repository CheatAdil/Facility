using UnityEngine;
using UnityEngine.AI;
public class Twins : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] float speed;
    [SerializeField] float chasingSpeed;
    [SerializeField] Twins secondTwin;
    [SerializeField] bool IsMainTwin;
    private NavMeshAgent agent;
    private Transform t;
    private GameObject Player;
    [SerializeField] private float idleRadius;
    SelfState ss;
    private float timer;
    private bool waiting;
    private float waitingTime;
    private float waitingTimer;
    [SerializeField] private float FieldOfView;
    private float checkTimer;
    private float rayTime;
    private float rayElapsedTime;
    private Transform rayPoint;
    private float LoseTimer;
    private bool PlayerIsVisible;
    private bool helping;
    [SerializeField] private float WaitBeforeIdle;
    [SerializeField] private LayerMask LayersToIgnore;
    [SerializeField] private Animator animator;
    private void Start()
    {
        speed = 3.5f;
        chasingSpeed = 3.75f;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        t = GetComponent<Transform>();
        Player = GameObject.Find("Player");
        ss = SelfState.idle;
        rayTime = Random.Range(0.7f, 1.5f);
        rayPoint = transform.Find("rayPoint");
    }
    private void Update()
    {
        if (rayElapsedTime <= rayTime)
        {
            rayElapsedTime += Time.deltaTime;
        }
        else
        {
            rayElapsedTime = 0;
            rayTime = Random.Range(0.7f, 1.5f);
            DoRayCast();
        }
        if (ss == SelfState.idle )
        {
            if (waiting)
            {
                animator.SetBool("moving", false);
                waitingTimer += Time.deltaTime;
                if (waitingTimer >= waitingTime)
                {
                    waitingTimer = 0;
                    waiting = false;
                    MoveEnemy(IdleWalkPoint());
                }
            }
            else if (agent.hasPath == true)
            {
                timer += Time.deltaTime;
                if (timer >= Random.Range(6.5f, 10f))
                {
                    timer = 0;
                    MoveEnemy(IdleWalkPoint());
                }
            }
            else
            {
                MoveEnemy(IdleWalkPoint());
            }
        }
        else if (ss == SelfState.chasingPlayer)
        {
            MoveEnemy(Player.transform.position);
            if (!helping)
            {
                if (!PlayerIsVisible)
                {
                    LoseTimer += Time.deltaTime;
                    if (LoseTimer >= WaitBeforeIdle)
                    {
                        ss = SelfState.idle;
                        MoveEnemy(IdleWalkPoint());
                        secondTwin.StopHelping();
                    }
                }
            }
        }
        else if (ss == SelfState.checkingSpot)
        {
            checkTimer += Time.deltaTime;
            Vector3 spot = agent.destination;
            if (Vector3.Distance(spot, t.position) < 3f)
            {
                ss = SelfState.idle;
            }
            else if (checkTimer >= 20f)
            {
                ss = SelfState.idle;
            }
        }
    }
    private Vector3 IdleWalkPoint()
    {
        ss = SelfState.idle;
        agent.speed = speed;
        Vector3 p = new Vector3(0f, 0f, 0f);
        Vector3 result = new Vector3(0f, 0f, 0f);
        NavMeshHit hit;
        if (IsMainTwin) 
        {
            p = (Random.insideUnitSphere * idleRadius);
            p += t.position;
        }
        else 
        {
            float r = idleRadius;
            Vector3 sb = secondTwin.transform.position;
            float angle = Random.Range(0f, 360f);
            float x, z;
            x = Mathf.Cos(angle) * r;
            z = Mathf.Sin(angle) * r;
            p = new Vector3 (x, 0f, z);
            p += sb;
          ///  Debug.DrawRay(p, Vector3.up, Color.red, 2, false);
        }
        if (NavMesh.SamplePosition(p, out hit, idleRadius, 1))
        {
            result = p;
        }
        return result;
    }
    private void MoveEnemy(Vector3 point)
    {
        agent.SetDestination(point);
        if (ss == SelfState.idle)
        {
            if (Random.Range(0f, 100f) >= 98f)
            {
                waiting = true;
                waitingTime = Random.Range(2.5f, 4.5f);
            }
        }
        animator.SetBool("moving", true);
    }
    private void ChasePlayer()
    {
        ss = SelfState.chasingPlayer;
        agent.speed = chasingSpeed;
    }
    public void CheckSpot(Vector3 spot)
    {
        agent.speed = speed;
        ss = SelfState.checkingSpot;
        MoveEnemy(spot);
    }
    private void DoRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPoint.position, (Player.transform.position - rayPoint.position), out hit, Mathf.Infinity, ~LayersToIgnore))
        {
           // Debug.DrawRay(rayPoint.position, transform.forward, Color.red, 2, false);
         //   Debug.DrawRay(rayPoint.position, (Player.transform.position - t.position), Color.yellow, 2, false);
            if (hit.collider.tag == "Player")
            {
                if (Mathf.Abs(Vector3.Angle(transform.forward, (Player.transform.position - rayPoint.position))) <= (FieldOfView / 2f))
                {
                    if (!helping) 
                    {
                        secondTwin.HelpMe();
                    }
                    ChasePlayer();
                    PlayerIsVisible = true;
                }
            }
            else PlayerIsVisible = false;
        }
    }
    private void KillPlayer()
    {
        Debug.Log("YOU KILLED");
        animator.SetTrigger("attacking");
        Player.SendMessage("Killed", head);
        speed = 0f;
        chasingSpeed = 0f;
        agent.speed = 0f;
    }
    public void HelpMe() 
    {
        helping = true;
        ss = SelfState.chasingPlayer;
    }
    public void StopHelping() 
    {
        helping = false;
    }
    private void Noise(string type) 
    {
        if (type == "sprint" && Vector3.Distance(t.position, Player.transform.position) <= 35f) 
        {
            CheckSpot(Player.transform.position);
            Debug.Log("Checking noise source");
        }
        else if (type == "jump" && Vector3.Distance(t.position, Player.transform.position) <= 40f)
        {
            CheckSpot(Player.transform.position);
            Debug.Log("Checking noise source");
        }
        else if (type == "itemDropped" && Vector3.Distance(t.position, Player.transform.position) <= 35f)
        {
            CheckSpot(Player.transform.position);
            Debug.Log("Checking noise source");
        }
    }
}
