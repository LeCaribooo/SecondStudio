using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Bow : MonoBehaviour
{
    public Animator animator;
    public GameObject arrow;
    public float launchForce;
    public Transform shotPoint;

    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;

    public AnimatedArms animatedArms;
    public float shootCooldown;
    bool canShoot = true;

    public PhotonView PV;

    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(0) && animator.GetInteger("AttackStatus") == 0 && canShoot)
            {
                animator.SetInteger("AttackStatus",1);
                StartCoroutine(Wait());
            }
            for (int i = 0; i < numberOfPoints; i++)
            {
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
        }
    }

    IEnumerator Wait()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }



    public void Shoot()
    {
        if (PV.IsMine)
        {
            GameObject newArrow = PhotonNetwork.Instantiate(Path.Combine("Prefab", "Player", arrow.name), shotPoint.position, shotPoint.rotation);
            if (animatedArms.playerControler.facingRight)
                newArrow.GetComponent<Rigidbody2D>().velocity = -transform.right * launchForce;
            else
                newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        }
        
    }


    Vector2 PointPosition(float t)
    {
        Vector2 position;
        if (animatedArms.playerControler.facingRight)
        {
            position = (Vector2)shotPoint.position + (-1 * (Vector2)animatedArms.transform.right.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
        }
        else
        {
            position = (Vector2)shotPoint.position + ((Vector2)animatedArms.transform.right.normalized * launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
        }
        return position;
    }

    private void OnDisable()
    {
        foreach(GameObject point in points)
        {
            Destroy(point);
        }
    }

    private void OnEnable()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
        }
    }
}
