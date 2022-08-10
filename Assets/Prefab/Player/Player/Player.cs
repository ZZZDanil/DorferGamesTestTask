using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Joystick joystick;
    public CharacterController controller;
    public Animator animatorModel;
    public GameObject sickle;

    private Transform mCamera;
    private Transform playerTransform;
    private float speed = 1;

    private void Awake()
    {
        mCamera = Camera.main.transform;
        playerTransform = transform;
        StaticData.player = this;
        speed = StaticData.settings.playerSpeed;
    }

    private void Update()
    {
        if (animatorModel.GetInteger("State") != 2)
        {
            Vector3 move = MoveVector(joystick.Direction);

            if (move != Vector3.zero)
            {
                controller.Move(move * Time.deltaTime * speed);
                playerTransform.forward = move;
                animatorModel.SetInteger("State", 1);
            }
            else
            {
                animatorModel.SetInteger("State", 0);
            }
        } 

    }


    private Vector3 MoveVector(Vector2 joystickVector)
    {
        Quaternion cameraLookR = mCamera.rotation;
        cameraLookR.x = 0;
        cameraLookR.z = 0;
        Vector3 move = cameraLookR * new Vector3(joystickVector.x, 0, joystickVector.y);

        return move.normalized;
    }
}
