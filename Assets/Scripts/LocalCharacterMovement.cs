using UnityEngine;
using UnitySocketIO;

namespace Assets.Scripts
{
    public class LocalCharacterMovement : MonoBehaviour
    {
        public string sessionId;
        public SocketIOController socket;

        private Rigidbody playerRigidBody;

        CharacterMovement characterMovement;
        PlayerMovement lastMovement = new PlayerMovement();

        void Start()
        {
            characterMovement = new CharacterMovement(GetComponent<Animator>(), GetComponent<Rigidbody>());
            playerRigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float lh = Input.GetAxisRaw("Horizontal");
            float lv = Input.GetAxisRaw("Vertical");

            if (socket != null && playerRigidBody != null)
            {
                var currentMovement =
                    new PlayerMovement(sessionId, playerRigidBody.position.x, playerRigidBody.position.z, playerRigidBody.rotation.eulerAngles.y, lh != 0f || lv != 0, lh, lv);

                if (!currentMovement.MovementIsEqual(lastMovement))
                {
                    lastMovement = currentMovement;
                    socket.Emit("move", currentMovement.ToString());
                }
            }

            characterMovement.Move(lh, lv);
        }
    }
}
