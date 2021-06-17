using UnityEngine;

[System.Serializable]
public class Response 
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObjecty dialogueObjecty;

    public string ResponseText => responseText;

    public DialogueObjecty DialogueObjecty => dialogueObjecty;
}
