using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    public int level;
    public int experience;
    public int point = 0;
    [SerializeField] int expForNextLevel;
    [SerializeField] XPBar xpBar;
    [SerializeField] AptitudeManager aptitudeManager;

    // Start is called before the first frame update
    void Start()
    {
        xpBar.SetMaxXP(expForNextLevel,experience);
        xpBar.SetLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        xpBar.SetXP(experience);
        if(experience >= expForNextLevel)
        {
            level++;
            point++;
            experience -= expForNextLevel;
            expForNextLevel *= 2;
            xpBar.SetMaxXP(expForNextLevel, experience);
            xpBar.SetLevel(level);
            aptitudeManager.EvaluateSkillTree();
        }
    }
}
