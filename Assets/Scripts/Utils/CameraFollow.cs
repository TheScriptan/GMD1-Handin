using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public bool RotateAroundPlayer = true;
    public float RotationSpeed = 5.0f;

    //LateUpdate is used here to update camera movement AFTER player movement was updated in Update function
    private void LateUpdate()
    {
        if (target == null)
            return;

        if (RotateAroundPlayer && Input.GetMouseButton(2))
        {
            Quaternion turnAngleX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            offset = turnAngleX * offset;
        }


        Vector3 newPos = target.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, 0.5f);
        transform.LookAt(target);
    }
}