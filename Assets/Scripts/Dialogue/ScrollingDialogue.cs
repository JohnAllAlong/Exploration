using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attaching this class to a dialogue game object will make the text scroll from left to right.
/// </summary>

[RequireComponent(typeof(Text))]
public class ScrollingDialogue : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] [Range(0f, 20f)] private float scrollSpeed;
    [SerializeField] private string dialogueText;
    [SerializeField] private Color textColor;

    private float timer;

    private void OnEnable() {
        timer = 0f;
    }

    private void Update() {
        GetComponent<Text>().text = "";
        int dialogueLength = dialogueText.Length;

        if (timer < dialogueLength) timer += Time.deltaTime * scrollSpeed;
        int numChars = (int)Mathf.Floor(timer);

        for (int i = 0; i < numChars; i++) {
            GetComponent<Text>().color = textColor;
            GetComponent<Text>().text += dialogueText[i];
        }

    }
}
