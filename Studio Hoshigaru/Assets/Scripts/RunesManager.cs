using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesManager : MonoBehaviour
{
    [SerializeField] WeaponSelection wp;
    [SerializeField] PlayerRunes playerRunes;
    [SerializeField] GameObject[] runesSlot = new GameObject[4];
    List<Runes> displayedRunes = new List<Runes>();
    List<Runes> allRunes = new List<Runes>();
    List<Runes> swordRunes = new List<Runes>();
    List<Runes> bowRunes = new List<Runes>();
    List<Runes> hammerRunes = new List<Runes>();
    List<Runes> shurikenRunes = new List<Runes>();
    public Dictionary<string, Runes> BookOfRunes;

    public Runes estoc;
    public Runes tripleShoot;
    public Runes bleed;
    public Runes charge;
    
    void Start()
    {
        BookOfRunes = new Dictionary<string, Runes>();
        BookOfRunes.Add("Estoc", estoc);
        BookOfRunes.Add("TripleShoot", tripleShoot);
        BookOfRunes.Add("Bleed", bleed);
        BookOfRunes.Add("Charge", charge);


        for (int i = 0; i < playerRunes.nbOfRunes; i++)
        {
            runesSlot[i].SetActive(true);
        }
    }

    void Update()
    {
        for (int i = 0; i < playerRunes.nbOfRunes; i++)
        {
            runesSlot[i].SetActive(true);
        }
    }

    public void AddRune(Runes Rune)
    {
        GameObject rune = Instantiate(Rune.gameObject, transform.position, Quaternion.identity);
        rune.transform.SetParent(this.transform);
        rune.SetActive(false);
        allRunes.Add(rune.gameObject.GetComponent<Runes>());
        if (Rune.runesWeapon == Weapon.SWORD)
            swordRunes.Add(rune.gameObject.GetComponent<Runes>());
        else if (Rune.runesWeapon == Weapon.BOW)
            bowRunes.Add(rune.gameObject.GetComponent<Runes>());
        else if (Rune.runesWeapon == Weapon.HAMMER)
            hammerRunes.Add(rune.gameObject.GetComponent<Runes>());
        else
            shurikenRunes.Add(rune.gameObject.GetComponent<Runes>());
    }

    public void AddRune(string str)
    {
        Runes Rune;
        BookOfRunes.TryGetValue(str, out Rune);
        if (Rune == null)
            return;
        GameObject rune = Instantiate(Rune.gameObject, transform.position, Quaternion.identity);
        rune.transform.SetParent(this.transform);
        rune.SetActive(false);
        allRunes.Add(rune.gameObject.GetComponent<Runes>());
        if (Rune.runesWeapon == Weapon.SWORD)
            swordRunes.Add(rune.gameObject.GetComponent<Runes>());
        else if (Rune.runesWeapon == Weapon.BOW)
            bowRunes.Add(rune.gameObject.GetComponent<Runes>());
        else if (Rune.runesWeapon == Weapon.HAMMER)
            hammerRunes.Add(rune.gameObject.GetComponent<Runes>());
        else
            shurikenRunes.Add(rune.gameObject.GetComponent<Runes>());
    }


    public void Display()
    {
        for (int i = 0; i < displayedRunes.Count; i++)
        {
            displayedRunes[i].gameObject.SetActive(false);
        }
        switch (wp.actualWeaponString)
        {
            case "hasSword":
                displayedRunes = swordRunes;
                break;
            case "hasBow":
                displayedRunes = bowRunes;
                break;
            case "hasShuriken":
                displayedRunes = shurikenRunes;
                break;
            case "hasHammer":
                displayedRunes = hammerRunes;
                break;
            default:
                break;
        }
        for (int i = 0; i < displayedRunes.Count; i++)
        {
            displayedRunes[i].gameObject.SetActive(true);
        }
    }
}
