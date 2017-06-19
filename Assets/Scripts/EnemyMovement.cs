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
    float timeToEmit = 0.1f;

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
            lastEmitTime = Time.time;

            var currentMovement =
                            new EntityMovement(sessionId, enemy.position.x, enemy.position.z, enemy.rotation.eulerAngles.y);

            if (!currentMovement.Equals(lastMovement))
            {
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