using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;

    public TextMeshProUGUI speakerText;
    public Image image;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed;

    private Coroutine typingCoroutine;

    public bool IsTyping { get; private set; }
    private string currentText;

    public void ShowLine(DialogueLine line)
    {
        dialoguePanel?.SetActive(true);
        speakerText.text = line.speaker;
        image.sprite = line.sprite;
        currentText = line.text;
        typingSpeed = line.typingSpeed;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        IsTyping = true;
        dialogueText.text = "";

        foreach (char letter in currentText)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false;
    }

    public void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = currentText;
        IsTyping = false;
    }

    public void Hide()
    {
        dialoguePanel?.SetActive(false);
    }
}