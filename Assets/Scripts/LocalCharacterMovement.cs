using SocketIO;
using UnityEngine;

namespace Assets.Scripts
{
    public class LocalCharacterMovement : MonoBehaviour
    {
        public string sessionId;

        public float speed = 6f;
        public float turnSpeed = 10f;
        public float turnSmooth = 15f;
        public SocketIOComponent socket;

        private Vector3 movement;
        private Vector3 turning;
        private Animator anim;
        private Rigidbody playerRigidBody;

        EntityMovement lastMovement = new EntityMovement();

        void Start()
        {
            anim = GetComponent<Animator>();
            playerRigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float lh = Input.GetAxisRaw("Horizontal");
            float lv = Input.GetAxisRaw("Vertical");

            if (socket != null && playerRigidBody != null)
            {
                var currentMovement =
                    new EntityMovement(sessionId, playerRigidBody.position.x, playerRigidBody.position.z, playerRigidBody.rotation.eulerAngles.y, lh != 0f || lv != 0);

                if (!currentMovement.Equals(lastMovement))
                {
                    lastMovement = currentMovement;

                    socket.Emit("move", currentMovement.ToJSONObject());
                }
            }

            Move(lh, lv);
            Animating(lh, lv);
        }

        void Move(float lh, float lv)
        {
            movement.Set(lh, 0f, lv);

            movement = movement.normalized * speed * Time.deltaTime;

            if (playerRigidBody != null)
                playerRigidBody.MovePosition(transform.position + movement);

            if (lh != 0f || lv != 0f)
            {
                Rotating(lh, lv);
            }
        }

        void Rotating(float lh, float lv)
        {
            var targetDirection = new Vector3(lh, 0f, lv);

            var targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            if (playerRigidBody != null)
            {
                var newRotation = Quaternion.Lerp(playerRigidBody.rotation, targetRotation, turnSmooth * Time.deltaTime);

                playerRigidBody.MoveRotation(newRotation);
            }
        }

        void Animating(float lh, float lv)
        {
            bool running = lh != 0f || lv != 0f;

            if (anim != null)
                anim.SetBool("IsRunning", running);
        }
    }
}
