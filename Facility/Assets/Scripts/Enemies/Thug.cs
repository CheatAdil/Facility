using UnityEngine;
using UnityEngine.AI;

public enum SelfState 
{
    idle,
    chasingPlayer,
    checkingSpot,
    attacking
};
public class Thug : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] float speed;
    [SerializeField] float chasingSpeed;
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
    [SerializeField] private float WaitBeforeIdle;
    [SerializeField] private Animator animator;
    private float attackTimer;
    [SerializeField] private LayerMask LayersToIgnore;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        t = GetComponent<Transform>();
        Player = GameObject.Find("Player");
        ss = SelfState.idle;
        agent.speed = speed;
        rayTime = Random.Range(0.7f, 1.5f);
        rayPoint = GameObject.Find("rayPoint").GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
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
        if (ss == SelfState.idle) 
        {
            if (waiting)
            {
                animator.SetBool("Walking", false);
                waitingTimer += Time.deltaTime;
                if (waitingTimer >= waitingTime)
                {
                    waitingTimer = 0;
                    waiting = false;
                    MoveEnemy(IdleWalkPoint());
                    animator.SetBool("Walking", true);
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
            if (!PlayerIsVisible) 
            {
                LoseTimer += Time.deltaTime;
                if (LoseTimer >= WaitBeforeIdle) 
                {
                    ss = SelfState.idle;
                    MoveEnemy(IdleWalkPoint());
                }
            }
        }
        else if (ss == SelfState.attacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= 3f)
            {
                ss = SelfState.chasingPlayer;
                attackTimer = 0f;
                agent.speed = speed;
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
        agent.speed = speed;
        ss = SelfState.idle;
        Vector3 p;
        Vector3 result = new Vector3 (0f,0f,0f);
        NavMeshHit hit;
        p = (Random.insideUnitSphere * idleRadius);
        p += t.position;
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
        animator.SetBool("Walking", true);
    }
    private void ChasePlayer() 
    {      
        ss = SelfState.chasingPlayer;
        agent.speed = chasingSpeed;
    }
    public void CheckSpot(Vector3 spot) 
    {
        ss = SelfState.checkingSpot;
        agent.speed = speed;
        MoveEnemy(spot);
    }
    private void DoRayCast() 
    {
        RaycastHit hit;
        if (Physics.Raycast(rayPoint.position, (Player.transform.position - rayPoint.position), out hit, Mathf.Infinity, ~LayersToIgnore))
        {
            Debug.DrawRay(rayPoint.position, (hit.point - t.position), Color.yellow, 2, false);
            if (hit.collider.tag == "Player")
            {
                if (Mathf.Abs(Vector3.Angle(transform.forward, (Player.transform.position - rayPoint.position))) <= (FieldOfView / 2f))
                {
                    ChasePlayer();
                    PlayerIsVisible = true;
                }
            }
            else PlayerIsVisible = false;
        }
    }
    private void KillPlayer() 
    {
        agent.speed = 0f;
        ss = SelfState.attacking;
        animator.SetTrigger("Attack");
        Player.SendMessage("Killed", t);
        Debug.Log("YOU KILLED");
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