using SocketIO;
using UnityEngine;

namespace Assets.Scripts
{
    public class NetworkCharacterMovement : MonoBehaviour
    {
        private Transform playerTransform;
        private Animator anim;

        void Awake()
        {
            anim = GetComponent<Animator>();
            playerTransform = GetComponent<Transform>();
        }

        public void OnMovement(SocketIOEvent obj)
        {
            float position_x = float.Parse(obj.data["x"].ToString());
            float position_z = float.Parse(obj.data["z"].ToString());
            bool isRunning = bool.Parse(obj.data["a"].ToString());

            float rotation_y = float.Parse(obj.data["ry"].ToString());

            playerTransform.SetPositionAndRotation(
                new Vector3(position_x, 0f, position_z), 
                Quaternion.Euler(0f, rotation_y, 0f));
            
            anim.SetBool("IsRunning", isRunning);
        }
    }
}
