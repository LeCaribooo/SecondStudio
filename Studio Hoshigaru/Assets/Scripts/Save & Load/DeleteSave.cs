using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSave : MonoBehaviour
{
    [SerializeField]
    private string currentProfile;

    public void Delete()
    {
        Debug.LogWarning(currentProfile + " delete in progress...");
        foreach (string int_k in arrKey.int_key)
        {
            PlayerPrefs.DeleteKey(currentProfile + int_k);
        }
        foreach (string float_k in arrKey.float_key)
        {
            PlayerPrefs.DeleteKey(currentProfile + float_k);
        }
        foreach (string string_k in arrKey.string_key)
        {
            PlayerPrefs.DeleteKey(currentProfile + string_k);
        }
        Debug.LogWarning("..." + currentProfile + " deletion completed !");
    }

}
