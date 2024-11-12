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
    [SerializeField] private string mainText;
    [SerializeField] private Color mainTextColor;
    [SerializeField] private string translatedText;
    [SerializeField] private Color translatedTextColor;

    private float timer;

    private void OnEnable() {
        timer = 0f;
    }

    private void Update() {
        GetComponent<Text>().text = "";
        int dialogueLength = mainText.Length;

        if (timer < dialogueLength) timer += Time.deltaTime * scrollSpeed;
        int numChars = (int)Mathf.Floor(timer);

        for (int i = 0; i < numChars; i++) {
            GetComponent<Text>().color = mainTextColor;
            GetComponent<Text>().text += mainText[i];
        }

    }
}
