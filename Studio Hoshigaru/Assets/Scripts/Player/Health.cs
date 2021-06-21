using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun, IPunObservable
{
    public PlayerSO playerSO;

    public int numOfHearts;
    public int numOfHits;

    public Image[] hearts;
    public Sprite emptyHeart;
    public Sprite quarterHeart;
    public Sprite halfHeart;
    public Sprite hquarterHeart;
    public Sprite fullHeart;

    public bool isInvisible = false;

    public int defense;

    public int regen;
    public int regenTime;
    bool canRegen;

    void Start()
    {
        canRegen = true;
        regen = 0;
        defense = 0;
        numOfHearts = playerSO.numOfHearts;
        numOfHits = playerSO.numOfHits;
    } 

    void Update()
    {
        if(canRegen)
            StartCoroutine(Regen());
        if(numOfHits <= 0)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].sprite = emptyHeart;
            }
        }
        else
        {
            if (numOfHits > numOfHearts * 4)
            {
                numOfHits = numOfHearts * 4;
            }
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < numOfHits / 4)
                {
                    hearts[i].sprite = fullHeart;
                }
                else if (i > numOfHits / 4)
                {
                    hearts[i].sprite = emptyHeart;
                }
                else
                {
                    int rest = numOfHits % 4;
                    switch (rest)
                    {
                        case 0:
                            hearts[i].sprite = emptyHeart;
                            break;
                        case 1:
                            hearts[i].sprite = quarterHeart;
                            break;
                        case 2:
                            hearts[i].sprite = halfHeart;
                            break;
                        case 3:
                            hearts[i].sprite = hquarterHeart;
                            break;
                    }
                }

                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }
    }

    IEnumerator Regen()
    {
        canRegen = false;
        yield return new WaitForSeconds(60);
        if(numOfHits + regen <= numOfHearts * 4)
        {
            numOfHits += regen;
        }
        canRegen = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(numOfHits);
            stream.SendNext(numOfHearts);
            stream.SendNext(defense);
            stream.SendNext(regen);
        }
        else
        {
            numOfHits = (int)stream.ReceiveNext();
            numOfHearts = (int)stream.ReceiveNext();
            defense = (int)stream.ReceiveNext();
            regen = (int)stream.ReceiveNext();
        }
    }
}

