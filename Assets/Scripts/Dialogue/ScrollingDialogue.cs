using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attaching this class to a dialogue game object will make the text scroll from left to right.
/// </summary>

[RequireComponent(typeof(Text))]
public class ScrollingDialogue : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField]
    [Range(0f, 20f)] private float scrollSpeed;

    private float timer;

    private string dialogueText;

    private void OnEnable() {
        dialogueText = GetComponent<Text>().text;
        timer = 0f;
    }

    private void Update() {
        GetComponent<Text>().text = "";
        int dialogueLength = dialogueText.Length;
        int numChars = (int)Mathf.Floor(timer);

        if (timer < dialogueLength) timer += Time.deltaTime * scrollSpeed;

        for (int i = 0; i < numChars; i++) {
            GetComponent<Text>().text += dialogueText[i];
        }

    }
}
