using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{

    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private TMP_Text textLabel;
    private ResponseHandler responseHandler;
    public bool isSpeaking = false;

    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        responseHandler = GetComponent<ResponseHandler>();
    }


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
        for (int i = 0; i < dialogueObjecty.Dialogue.Length; i++)
        {
            string dialogue = dialogueObjecty.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);

            if(i == dialogueObjecty.Dialogue.Length - 1 && dialogueObjecty.HasResponses) { break; }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (!dialogueObjecty.isEnd)
        {
            responseHandler.ShowResponses(dialogueObjecty.Responses, store);
        }
        else
        {
            isSpeaking = false;
            CloseDialogueBox();
        }
    }

    public void CloseDialogueBox()
    {
        this.gameObject.SetActive(false);
        DialogueBox.gameObject.SetActive(false);
        textLabel.text = string.Empty;
    }
}
