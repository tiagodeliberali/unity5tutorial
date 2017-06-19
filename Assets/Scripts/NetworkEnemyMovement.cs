using SocketIO;
using UnityEngine;

namespace Assets.Scripts
{
    public class NetworkEnemyMovement : MonoBehaviour
    {
        private Transform enemyTransform;

        void Awake()
        {
            enemyTransform = GetComponent<Transform>();
        }

        public void OnMovement(SocketIOEvent obj)
        {
            var movement = new EntityMovement(obj);

            enemyTransform.SetPositionAndRotation(
                new Vector3(movement.X, 0f, movement.Z),
                Quaternion.Euler(0f, movement.RY, 0f));
        }
    }
}
