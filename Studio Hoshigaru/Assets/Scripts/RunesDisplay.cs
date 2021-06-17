using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunesDisplay : MonoBehaviour
{
    public Text text;
    public Runes rune;

    public void SetDisplay(Runes rune)
    {
        this.rune = rune;
        text.text = "* "+ rune.name;
    }
}
