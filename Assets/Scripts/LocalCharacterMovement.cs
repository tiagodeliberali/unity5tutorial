using SocketIO;
using UnityEngine;

namespace Assets.Scripts
{
    public class LocalCharacterMovement : MonoBehaviour
    {
        public string sessionId;
        public SocketIOComponent socket;

        private Rigidbody playerRigidBody;

        CharacterMovement characterMovement;
        EntityMovement lastMovement = new EntityMovement();

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
                    new EntityMovement(sessionId, playerRigidBody.position.x, playerRigidBody.position.z, playerRigidBody.rotation.eulerAngles.y, lh != 0f || lv != 0, lh, lv);

                if (!currentMovement.MovementIsEqual(lastMovement))
                {
                    lastMovement = currentMovement;

                    socket.Emit("move", currentMovement.ToJSONObject());
                }
            }

            characterMovement.Move(lh, lv);
        }
    }
}
