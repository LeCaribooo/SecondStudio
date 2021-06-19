using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase2 : MonoBehaviour
{
    // Start is called before the first frame update
    public PhaseChanging pc;
    
    public void Start()
    {
        pc = GetComponentInParent<PhaseChanging>();
    }

    public void Changed()
    {
        pc.ended = true;
    }
}
