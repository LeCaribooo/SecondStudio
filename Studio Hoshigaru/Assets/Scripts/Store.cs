using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;


public class Store : MonoBehaviour,IPointerDownHandler
{
    public GameObject runesDisplay;
    public GameObject buyableRunes;
    public Text RuneDiplay;
    public GameObject description;
    public Image image;
    public Text name;
    public Text explanation;
    public Text cost;
    public Weapon weapon;


    public RunesManager manager;
    public Runes selectedRune;
    public PlayerRunes playerRunes;

    public List<Runes> runesInStore;
    public List<GameObject> actualDisplay;

    // Start is called before the first frame update
    void Start()
    {
        selectedRune = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedRune != null)
        { 
            description.SetActive(true);
            name.text = selectedRune.name;
            explanation.text = selectedRune.description;
            cost.text = "Cout : " + playerRunes.parts + "/" + selectedRune.cost.ToString();
            image.sprite = selectedRune.image.sprite;
        }
    }

    public void Spawn(Runes rune)
    {
        GameObject Display = Instantiate(runesDisplay, buyableRunes.transform.position, Quaternion.identity);
        actualDisplay.Add(Display);
        Display.GetComponent<RectTransform>().SetParent(buyableRunes.transform);
        Display.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        Display.GetComponent<RunesDisplay>().SetDisplay(rune);
    }

    public void OnClick_Buy()
    {
        if(selectedRune.cost <= playerRunes.parts)
        {
            manager.AddRune(selectedRune);
            playerRunes.parts -= selectedRune.cost;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetPhotonView().IsMine)
        {
            Runes[] allRunes = collision.gameObject.GetComponentsInChildren<Runes>();
            playerRunes = collision.gameObject.GetComponentInParent<PlayerRunes>();
            manager = collision.gameObject.GetComponentInParent<PlayerControler>().runesManager;
            for (int i = 0; i < allRunes.Length; i++)
            {
                if (allRunes[i].runesWeapon == weapon)
                {
                    runesInStore.Add(allRunes[i]);
                    Spawn(allRunes[i]);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetPhotonView().IsMine)
        {
            for (int i = 0; i < actualDisplay.Count; i++)
            {
                Destroy(actualDisplay[i]);
            }
            actualDisplay.Clear();
            selectedRune = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerEnter.name == "RuneDisplay(Clone)")
            selectedRune = eventData.pointerEnter.GetComponent<RunesDisplay>().rune;
    }
}
