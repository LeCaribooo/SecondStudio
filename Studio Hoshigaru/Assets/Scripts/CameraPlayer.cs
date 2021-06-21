using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{ 
    public Transform target;
    public float smoothSpeed;
    public Vector3 offset;
    public bool check = true;

    private void LateUpdate()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("BossF");
        if(boss != null && check)
        {
            gameObject.GetComponent<Camera>().orthographicSize = (8);
            check = false;
        }
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
