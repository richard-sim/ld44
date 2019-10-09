using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDCameraController : MonoBehaviour
{
    public Camera TargetCamera;
    public PlayerController Player;
    public float CameraDistace = 4.0f;
    public float CameraHeight = 3.0f;
    public float CameraLookAtY = 1.5f;

    public float lerpAmount = 0.9f;

    private Vector3 currentTarget;
    private Vector3 desiredTarget;
    private Vector3 desiredPosition;
        
    // Start is called before the first frame update
    void Start()
    {
        desiredTarget = Player.transform.position;
        desiredTarget.y += CameraLookAtY;

        desiredPosition = desiredTarget - Player.transform.forward * CameraDistace;
        desiredPosition.y += CameraHeight;

        currentTarget = desiredTarget;
        transform.position = desiredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
/*
        Vector3 currentLookDir = currentTarget - currentPosition;
*/
        
        desiredTarget = Player.transform.position;
        desiredTarget.y += CameraLookAtY;
        desiredPosition = desiredTarget - Player.transform.forward * CameraDistace;
        desiredPosition.y += CameraHeight;
/*
        Vector3 desiredLookDir = desiredTarget - desiredPosition;
*/

        Vector3 newPosition = Vector3.Lerp(currentPosition, desiredPosition, lerpAmount * Time.deltaTime);
        Vector3 newTarget = Vector3.Lerp(currentTarget, desiredTarget, lerpAmount * Time.deltaTime);

        transform.position = newPosition;
        transform.LookAt(newTarget);

        currentTarget = newTarget;
    }
}
