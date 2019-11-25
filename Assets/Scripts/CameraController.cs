using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float turnSpeed;

    public Vector3 cameraVelocity;

	public bool isFrozen;

    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - player.transform.position;    
    }

    private void LateUpdate()
    {
        if (!isFrozen)
		{
			Vector3 targetPosition = player.transform.position + cameraOffset;
			Vector3 currentPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, turnSpeed);
			transform.position = currentPosition;
		}
    }
}
