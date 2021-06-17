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

    public void Begin(DialogueObjecty testDialogue, GameObject store)
    {
        isSpeaking = true;
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        CloseDialogueBox();
        ShowDialogue(testDialogue, store);
    }

    public void ShowDialogue(DialogueObjecty dialogueObjecty, GameObject store)
    {
        this.gameObject.SetActive(true);
        DialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObjecty, store));
    }

    private IEnumerator StepThroughDialogue(DialogueObjecty dialogueObjecty, GameObject store)
    {
        foreach (string dialogue in dialogueObjecty.Dialogue)
        {
            yield return typeWriterEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        CloseDialogueBox();
        isSpeaking = false;
    }

    public void CloseDialogueBox()
    {
        this.gameObject.SetActive(false);
        DialogueBox.gameObject.SetActive(false);
        textLabel.text = string.Empty;
    }
}
