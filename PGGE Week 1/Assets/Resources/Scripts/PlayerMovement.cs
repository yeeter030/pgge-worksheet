using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Animator mAnimator;

    public float walkspeed = 1f;
    public float rotationSpeed = 50f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        float speed = walkspeed;

        //increase speed when player is running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = walkspeed * 2.0f;
        }


        if (mAnimator == null) return;

        //allows player to rotate the character
        transform.Rotate(0.0f, hInput * rotationSpeed * Time.deltaTime, 0.0f);

        //find the forward direction of the player as well as making sure it is looking forward
        Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
        forward.y = 0.0f;

        //move character forward
        characterController.Move(forward * vInput * speed * Time.deltaTime);

        //change the values of PosX and PosZ so as to trigger the animation
        mAnimator.SetFloat("PosX", 0);
        mAnimator.SetFloat("PosZ", vInput * speed / 2.0f * walkspeed);
    }
}
