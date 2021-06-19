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

    private bool hasReturned = false;

    public void Awake()
    {
        if (!hasReturned && Destroy_Door.created)
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
        actualObjectivesIndex++;
    }

    public void PickObjective(int i)
    {
        target = objectives[i];
    }
}
