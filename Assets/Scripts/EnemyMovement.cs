using SocketIO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public SocketIOComponent socket;
    public string sessionId;

    Transform player;
    Transform enemy;
    NavMeshAgent nav;

    MotherShip motherShip;
    PlayerInventory playerInventory;

    EntityMovement lastMovement = new EntityMovement();

    float lastEmitTime;
    float timeToEmit = 0.25f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<Transform>();

        nav = GetComponent<NavMeshAgent>();
        motherShip = GameObject.Find("MotherShip").GetComponent<MotherShip>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (motherShip.collectedEnergy != motherShip.neededEnergy)
        {
            nav.SetDestination(player.position);

            EmitNewPostion();
        }
        else
        {
            nav.enabled = false;
        }
    }

    private void EmitNewPostion()
    {
        if (Time.time - lastEmitTime > timeToEmit)
        {
            Debug.Log("EmitNewPostion");

            lastEmitTime = Time.time;

            var currentMovement =
                            new EntityMovement(sessionId, enemy.position.x, enemy.position.z, enemy.rotation.eulerAngles.y);

            if (!currentMovement.Equals(lastMovement))
            {
                Debug.Log("New Enemy position: " + currentMovement);

                lastMovement = currentMovement;

                socket.Emit("enemy", currentMovement.ToJSONObject());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            motherShip.totalEnergy -= playerInventory.collectedEnergy;
            playerInventory.collectedEnergy = 0;
        }
    }
}