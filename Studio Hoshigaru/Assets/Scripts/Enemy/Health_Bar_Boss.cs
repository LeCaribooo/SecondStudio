using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar_Boss : MonoBehaviour
{
    public Slider slider;
    public string tag;
    private GameObject[] Bosses;
    public int bossIndex;
    public int Health;

    public void Start()
    {
        Bosses = GameObject.FindGameObjectsWithTag(tag);
        Health = Bosses[bossIndex].GetComponent<EnemyHealth>().health;
        SetMaxHealth(Health);
    }

    public void Update()
    {
        Health = Bosses[bossIndex].GetComponent<EnemyHealth>().health;
        SetHealth(Health);
        if (Health <= 0)
        {
            this.enabled = false;
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

