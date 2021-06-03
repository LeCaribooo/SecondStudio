using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int level;
    public int experience;
    [SerializeField] int expForNextLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(experience >= expForNextLevel)
        {
            level++;
            experience = 0;
            expForNextLevel *= 2;
        }
    }
}
