﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerDisplay : MonoBehaviour
{
    public Text text;
    public fleche_directionnelle fleche;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetUp(GameObject target, int i)
    {
        Player_portal portal;
        if (TryGetComponent<Player_portal>(out portal))
            this.text.text = portal.NameLvl;
        else
            this.text.text = target.name;
        this.i = i;
    }

    public void OnClick_PickTarget()
    {
        fleche.gameObject.SetActive(true);
        fleche.objectiveManager.PickObjective(i);
    }
}
