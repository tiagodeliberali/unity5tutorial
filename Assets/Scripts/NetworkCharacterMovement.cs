using SocketIO;
using UnityEngine;

namespace Assets.Scripts
{
    public class NetworkCharacterMovement : MonoBehaviour
    {
        private Transform playerTransform;
        private Animator anim;
        private EntityMovement movement;
        private CharacterMovement characterMovement;

        void Awake()
        {
            characterMovement = new CharacterMovement(GetComponent<Animator>(), GetComponent<Rigidbody>());
            playerTransform = GetComponent<Transform>();
        }

        void FixedUpdate()
        {
            if (movement != null)
            {
                characterMovement.Move(movement.LH.Value, movement.LV.Value);
            }
        }

        public void OnMovement(SocketIOEvent obj)
        {
            movement = new EntityMovement(obj);

            float rotation_y = float.Parse(obj.data["ry"].ToString());

            playerTransform.SetPositionAndRotation(
                new Vector3(movement.X, 0f, movement.Z), 
                Quaternion.Euler(0f, movement.RY, 0f));
            
            anim.SetBool("IsRunning", movement.IsRunning.Value);
        }
    }
}
