using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class fleche_directionnelle : MonoBehaviourPun
{
    public ObjectiveManager objectiveManager;
    public Transform target;
    public PhotonView PV;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        if (PV.IsMine)
        {
            objectiveManager = GameObject.Find("ObjectivesManager").GetComponent<ObjectiveManager>();
            target = objectiveManager.target.transform;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }
    void Update()
    {
        if (PV.IsMine)
        {
            objectiveManager = GameObject.Find("ObjectivesManager").GetComponent<ObjectiveManager>();
            if (objectiveManager.target == null)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                target = objectiveManager.target.transform;
                transform.gameObject.SetActive(true);
                Vector3 targetPosition = target.transform.position;
                targetPosition.z = transform.position.z;
                transform.LookAt(targetPosition);
            }
        }
    }
}
