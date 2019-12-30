using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rbRocketBody;
    AudioSource thrusterSound;

    Vector3 cameraOffset;
    [SerializeField] Camera playerCamera;
    [SerializeField] float thrustPower;
    [SerializeField] float rotatePower;

    [SerializeField] float velocity;

    float MinimumVelocityToRotate = 0.1f;

    Vector3 rocketStartPosition;
    Quaternion rocketStartRotation;
    void Start()
    {
        rbRocketBody = gameObject.GetComponent<Rigidbody>();
        thrusterSound = gameObject.GetComponent<AudioSource>();

        rocketStartPosition = rbRocketBody.position;
        rocketStartRotation = transform.rotation;

        cameraOffset = playerCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Thrust();
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        playerCamera.transform.position = gameObject.transform.position + cameraOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.tag.Equals("Friendly"))
        {
            StopShip();
            rbRocketBody.position = rocketStartPosition;
        }
    }

    private void StopShip()
    {
        rbRocketBody.velocity = Vector3.zero;
        rbRocketBody.rotation = rocketStartRotation;
    }

    private void Rotate()
    {
        rbRocketBody.freezeRotation = true;

        velocity = rbRocketBody.velocity.magnitude;

        if (velocity<MinimumVelocityToRotate)
        {
            return;
        }

        float rotationThisFrame = rotatePower * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rbRocketBody.freezeRotation = false;

    }

    private void Thrust()
    {
        float thrustThisFrame = thrustPower * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rbRocketBody.AddRelativeForce(Vector3.up* thrustThisFrame);

            if (!thrusterSound.isPlaying)
            {
                thrusterSound.Play();
            }
        }
        else
        {
            thrusterSound.Stop();
        }
    }
}
