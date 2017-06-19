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

            var currentMovement = 
                new EntityMovement(sessionId, enemy.position.x, enemy.position.z, enemy.rotation.eulerAngles.y, false);

            if (!currentMovement.Equals(lastMovement))
            {
                lastMovement = currentMovement;

                socket.Emit("enemy", currentMovement.ToJSONObject());
            }
        }
        else
        {
            nav.enabled = false;
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