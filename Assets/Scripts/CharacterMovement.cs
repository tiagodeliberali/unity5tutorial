using UnityEngine;

public class CharacterMovement
{
    public float speed = 6f;
    public float turnSpeed = 10f;
    public float turnSmooth = 15f;

    private Vector3 movement;
    private Vector3 turning;
    private Animator anim;
    private Rigidbody playerRigidBody;

    public CharacterMovement(Animator anim, Rigidbody playerRigidBody)
    {
        this.playerRigidBody = playerRigidBody;
        this.anim = anim;
    }

    public void Move(float lh, float lv)
    {
        movement.Set(lh, 0f, lv);

        movement = movement.normalized * speed * Time.deltaTime;

        if (playerRigidBody != null)
            playerRigidBody.MovePosition(playerRigidBody.position + movement);

        if (lh != 0f || lv != 0f)
        {
            Rotating(lh, lv);
        }

        Animating(lh, lv);
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
