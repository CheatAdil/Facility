using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float HP;
    [SerializeField] private int maxHealth;
    private Transform t;
    private float speed;
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float MaxStamina;
    private float stamina;
    private bool isSprinting;
    private bool ableToSprint;
    private GameManager gm;

    private CharacterController playerController;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public Transform GroundCheck;
    public float GrndChkRadius = 0.4f;
    public LayerMask GroundMask;
    public bool isGrounded;
    private Vector3 Fvelocity;
    public GameObject Camera;

    private List<GameObject> enemies;
    private bool EnemiesConnected;

    private bool alive = true;

    private float sprintingNoiseTimer;
    private void Awake()
    {
        EnemiesConnected = false;
    }
    private void Start()
    {
        HP = maxHealth;
        t = GetComponent<Transform>();
        playerController = GetComponent<CharacterController>();
        GroundCheck = t.GetChild(1);
        speed = PlayerSpeed;
        stamina = MaxStamina;
        isSprinting = false;
        ableToSprint = true;
        Camera = t.GetChild(0).gameObject;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        if (alive)
        {
            if (t.rotation.z != 0) t.rotation = Quaternion.Euler(new Vector3(t.rotation.x, t.rotation.y, 0f));
            isGrounded = Physics.CheckSphere(GroundCheck.position, GrndChkRadius, GroundMask);
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            if (x != 0 && z != 0) move /= Mathf.Sqrt(2);
            if (isGrounded && Fvelocity.y < 0)
            {
                Fvelocity.y = -2f;
            }
            playerController.Move(move * speed * Time.deltaTime);
            Fvelocity.y += gravity * Time.deltaTime;
            playerController.Move(Fvelocity * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.LeftShift) && ableToSprint && isGrounded)
            {
                StartSprint();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                StopSprint();
            }
            if (!isGrounded)
            {
                StopSprint();
            }
            SpendStamina(isSprinting);
            if (isSprinting)
            {
                sprintingNoiseTimer += Time.deltaTime;
                if (sprintingNoiseTimer >= 1f)
                {
                    sprintingNoiseTimer = 0;
                    MakeNoise("sprint");
                }
            }
        }
    }
    private void Killed(Transform enemy)
    {
        alive = false;
        t.LookAt(enemy);
        GetComponent<CharacterController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        Camera.GetComponent<CameraMove>().enabled = false;
        gm.EndGame();
    }
    private void StartSprint() 
    {
        speed = PlayerSpeed * 2.2f;
        isSprinting = true;
        MakeNoise("sprint");
    }
    private void StopSprint() 
    {
        speed = PlayerSpeed;
        isSprinting = false;
    }
    private void SpendStamina(bool issprinting) 
    {
        if (issprinting) 
        {
            stamina -= (Time.deltaTime * 2);
            if (stamina < 0) 
            {
                stamina = 0;
                ableToSprint = false;
                StopSprint();
            }
        }
        else 
        {
            if (stamina < MaxStamina) 
            {
                stamina += Time.deltaTime;
            }
            else 
            {
                stamina = MaxStamina;
                ableToSprint = true;
            }
        }
    }
    public void FindEnemies(List<GameObject> en) 
    {      
        int enCount = 0;
        enemies = new List<GameObject>();
        enemies = en;
        for (int i = 0; i < enemies.Count; i++) 
        {
            Debug.Log("Enemy: " + enemies[i]);
        }
        if (enemies.Count != 0) 
        {
            EnemiesConnected = true;
            Debug.Log("Enemies in this round: " + enemies.Count);
        }
        else 
        {
            Debug.LogError("JSON: Failled to connect enemies");
            Debug.LogError("If their presence was not intended");
            Debug.LogError("just ignore this message");
        }
    }
    private void MakeNoise(string type) 
    {
        if (EnemiesConnected) 
        {
            Debug.Log("You made noise");
            for (int i = 0; i < enemies.Count; i++) 
            {
                enemies[i].SendMessage("Noise", type);
            }
        }
    }
}