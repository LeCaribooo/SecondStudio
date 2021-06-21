using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode
{
    QUEST,
    TRACKER,
}


public class ObjectiveManager : MonoBehaviour
{
    public Mode mode;
    public List<GameObject> objectives;
    public int actualObjectivesIndex;
    public GameObject target;
    public Destroy_Door destroy_;
    public bool InRoom = false;

    private bool hasReturned = false;

    public void Awake()
    {
        if (!hasReturned && Destroy_Door.created && InRoom)
            NextObjective();
    }

    private void Start()
    {
        target = objectives[0];
    }

    private void Update()
    {
        if(mode == Mode.QUEST)
        {
            target = objectives[actualObjectivesIndex];
        }
    }

    public void NextObjective()
    {
        Debug.Log("On m'a appelé");
        actualObjectivesIndex += 1;
    }

    public void PickObjective(int i)
    {
        target = objectives[i];
    }
}
