using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundMinimap : MonoBehaviour
{
    public bool round;
    private Image mRectBorder;
    private Image mRoundBorder;
    private Image mMapMask;
    // Start is called before the first frame update
    public void OnValidate()
    {
        mRectBorder = GetComponent<Image>();
        mRoundBorder = transform.Find("fond minimap").GetComponent<Image>();
        mMapMask = transform.Find("Image").GetComponent<Image>();

        mRectBorder.enabled = !round;
        mMapMask.enabled = round;
        mRoundBorder.enabled = round;

    }
}
