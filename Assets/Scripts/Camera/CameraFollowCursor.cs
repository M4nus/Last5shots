using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCursor : MonoBehaviour
{
    private GameObject James;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Start()
    {
        James = GameObject.FindGameObjectWithTag("James");
    }

    private void LateUpdate()
    {
        if(James.transform.position.x < 10f)
            offset.x = 8f;
        else
            offset.x = 5f;
        if(James.transform.position.z < 10f)
            offset.z = -1f;
        else
            offset.z = -8f;

        Vector3 desiredPosition = James.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
