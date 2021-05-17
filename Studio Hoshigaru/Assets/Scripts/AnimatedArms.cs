using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedArms : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [HideInInspector] public Vector2 direction;
    [SerializeField] private PlayerControler playerControler;

    void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        if(playerControler.facingRight)
            direction = new Vector2(bowPosition.x - mousePosition.x, bowPosition.y - mousePosition.y);
        else
            direction = new Vector2(mousePosition.x - bowPosition.x, mousePosition.y - bowPosition.y);
        transform.right = direction;
        if (transform.rotation.eulerAngles.z < 335 && transform.rotation.eulerAngles.z > 30)
        {
            if (transform.rotation.eulerAngles.z > 180)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 335));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30));
            }
        }
    }
}