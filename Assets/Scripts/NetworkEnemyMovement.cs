using SocketIO;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class NetworkEnemyMovement : MonoBehaviour
    {
        public GameObject player;

        private Transform enemyTransform;
        private NavMeshAgent nav;
        private MotherShip motherShip;

        void Awake()
        {
            enemyTransform = GetComponent<Transform>();
            nav = GetComponent<NavMeshAgent>();
            motherShip = GameObject.Find("MotherShip").GetComponent<MotherShip>();
        }

        public void OnMovement(SocketIOEvent obj)
        {
            var movement = new EntityMovement(obj);

            enemyTransform.SetPositionAndRotation(
                new Vector3(movement.X, 0f, movement.Z),
                Quaternion.Euler(0f, movement.RY, 0f));
        }

        void Update()
        {
            if (motherShip.collectedEnergy != motherShip.neededEnergy)
            {
                nav.SetDestination(player.transform.position);
            }
            else
            {
                nav.enabled = false;
            }
        }
    }
}
