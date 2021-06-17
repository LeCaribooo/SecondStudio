using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunesManager : MonoBehaviour, IPointerDownHandler
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

    void Start()
    {
        for (int i = 0; i < playerRunes.nbOfRunes; i++)
        {
            runesSlot[i].SetActive(true);
        }
    }

    void Update()
    {
        Display();
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

    

    public void OnPointerDown(PointerEventData eventData)
    {
        int nextPosition = GetNextAvailableSlot();
        if(nextPosition != 5 && !runesSlot[nextPosition].GetComponent<RunesSlot>().isOccupied)
        {
            GameObject actualRune = eventData.pointerEnter;
            actualRune.GetComponent<RectTransform>().SetParent(runesSlot[nextPosition].transform);
            actualRune.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            runesSlot[nextPosition].GetComponent<RunesSlot>().isOccupied = true;
            actualRune.GetComponent<Runes>().Choose(true);
        }
    }


    public int GetNextAvailableSlot()
    {
        for (int i = 0; i < playerRunes.nbOfRunes; i++)
        {
            if (!runesSlot[i].GetComponent<RunesSlot>().isOccupied)
                return i;
        }
        return 5;
    }

}


