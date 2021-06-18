using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fleche_directionnelle : MonoBehaviour
{
    public ObjectiveManager objectiveManager;
    public Transform target;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        objectiveManager = GameObject.Find("ObjectivesManager").GetComponent<ObjectiveManager>();
        target = objectiveManager.target.transform;
    }
    void Update()
    {
        objectiveManager = GameObject.Find("ObjectivesManager").GetComponent<ObjectiveManager>();
        if(objectiveManager.target == null)
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
