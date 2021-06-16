using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{

    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool isSpeaking = false;

    private TypeWriterEffect typeWriterEffect;

    public void Begin(DialogueObjecty testDialogue)
    {
        isSpeaking = true;
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        CloseDialogueBox();
        ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObjecty dialogueObjecty)
    {
        this.gameObject.SetActive(true);
        DialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObjecty));
    }

    private IEnumerator StepThroughDialogue(DialogueObjecty dialogueObjecty)
    {
        foreach (string dialogue in dialogueObjecty.Dialogue)
        {
            yield return typeWriterEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        CloseDialogueBox();
        isSpeaking = false;
    }

    private void CloseDialogueBox()
    {
        this.gameObject.SetActive(false);
        DialogueBox.gameObject.SetActive(false);
        textLabel.text = string.Empty;
    
    }
}
