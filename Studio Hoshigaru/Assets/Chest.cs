using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Canvas pressT;
    public string function;
    bool OnIt;
    bool isPressed;
    GameObject onMe;

    private void Update()
    {
        if (OnIt)
        {
            pressT.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                onMe.GetComponentInChildren<WeaponSelection>().OnClick_ActiveButton(function);
                isPressed = true;
            }
        }
        if (isPressed)
        {
            pressT.gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onMe = collision.gameObject;
            OnIt = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onMe = null;
            OnIt = false;
            pressT.gameObject.SetActive(false);
        }
    }
}
