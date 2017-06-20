using UnityEngine;
using UnitySocketIO.Events;

namespace Assets.Scripts
{
    public class NetworkCharacterMovement : MonoBehaviour
    {
        private Transform playerTransform;
        private Animator anim;
        private PlayerMovement movement;
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
                characterMovement.Move(movement.LH, movement.LV);
            }
        }

        public void OnMovement(SocketIOEvent obj)
        {
            movement = new PlayerMovement(obj);

            playerTransform.SetPositionAndRotation(
                new Vector3(movement.X, 0f, movement.Z), 
                Quaternion.Euler(0f, movement.RY, 0f));
        }
    }
}
