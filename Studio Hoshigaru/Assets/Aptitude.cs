using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum State
{ 
    LOCKED,
    UNLOCKED,
    BOUGHT
}

public class Aptitude : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int levelNeeded;
    public int cost;
    public Text description;
    public State state;
    public Image icon;
    public Aptitude[] closestAptitude;
    private AptitudeManager aptitudeManager;
    public bool canBuy = false;
    public Button button;


    void Start()
    {
        aptitudeManager = GetComponentInParent<AptitudeManager>();
        if(!canBuy)
            canBuy = CheckIfCanBuy();
    }

    void Update()
    {
        if(!canBuy)
            canBuy = CheckIfCanBuy();
        if (state == State.UNLOCKED)
        {
            icon.sprite = aptitudeManager.unlocked;
            button.interactable = true;
        }           
        else if(state == State.LOCKED)
        {
            icon.sprite = aptitudeManager.locked;
            button.interactable = false;
        }
        else
        {
            icon.sprite = aptitudeManager.bougth;
            button.interactable = false;
        }
    }

    private bool CheckIfCanBuy()
    {
        for (int i = 0; i < closestAptitude.Length; i++)
        {
            if (closestAptitude[i].canBuy)
            {
                return true;
            }
        }
        return false;
    }


    public void OnClick_Buy()
    {
        if(aptitudeManager.playerExperience.point >= cost)
        {
            state = State.BOUGHT;
            aptitudeManager.playerExperience.point -= cost;
            button.interactable = false;
            for (int i = 0; i < closestAptitude.Length; i++)
            {
                closestAptitude[i].canBuy = true;
            }
            aptitudeManager.EvaluateSkillTree();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        aptitudeManager.text.text = description.text;
        aptitudeManager.description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        aptitudeManager.description.SetActive(false);
    }
}
