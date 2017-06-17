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
            float position_x = float.Parse(obj.data["x"].ToString());
            float position_z = float.Parse(obj.data["z"].ToString());
            float rotation_y = float.Parse(obj.data["ry"].ToString());

            enemyTransform.SetPositionAndRotation(
                new Vector3(position_x, 0f, position_z),
                Quaternion.Euler(0f, rotation_y, 0f));
        }
    }
}
