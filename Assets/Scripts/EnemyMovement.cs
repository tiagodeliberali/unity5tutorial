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

    private MotherShip motherShip;
    private PlayerInventory playerInventory;

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

            socket.Emit("enemy", new JSONObject(
                string.Format(@"{{""s"":""{0}"",""x"":{1},""z"":{2},""ry"":{3}}}",
                sessionId,
                enemy.position.x,
                enemy.position.z,
                enemy.rotation.eulerAngles.y)));
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