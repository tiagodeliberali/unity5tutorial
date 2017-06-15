using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 10f;
    public float turnSmooth = 15f;

    private Vector3 movement;
    private Vector3 turning;
    private Animator anim;
    private Rigidbody playerRigidBody;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        float lh = Input.GetAxisRaw("Horizontal");
        float lv = Input.GetAxisRaw("Vertical");

        Move(lh, lv);
        Animating(lh, lv);
    }

    void Move(float lh, float lv)
    {
        movement.Set(lh, 0f, lv);

        movement = movement.normalized * speed * Time.deltaTime;

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

        var newRotation = Quaternion.Lerp(playerRigidBody.rotation, targetRotation, turnSmooth * Time.deltaTime);

        playerRigidBody.MoveRotation(newRotation);
    }

    void Animating(float lh, float lv)
    {
        bool running = lh != 0f || lv != 0f;
        
        anim.SetBool("IsRunning", running);
    }
}
