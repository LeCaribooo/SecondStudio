using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    SWORD,
    BOW,
    HAMMER,
    SHURIKEN
}

public class Runes : MonoBehaviour
{
    public Weapon runesWeapon;
    public Behaviour capacity;
    public bool isChoose;

    void Start()
    {
             
    }

    void Update()
    {
        
    }

    public void Choose(bool state)
    {
        isChoose = state;
        capacity.enabled = state;
    }
}
