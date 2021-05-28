using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar_Boss : MonoBehaviour
{
    public Slider slider;

    public GameObject Boss;

    public int Health;

    public void Start()
    {
        Health = Boss.GetComponent<EnemyHealth>().health;
        SetMaxHealth(Health);
    }

    public void Update()
    {
        Health = Boss.GetComponent<EnemyHealth>().health;
        SetHealth(Health);
        if (Boss == null)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

}

