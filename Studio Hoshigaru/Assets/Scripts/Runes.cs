using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int cost;
    [TextArea] public string description;
    public Image image;

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
