using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attaching this class to a dialogue game object will make the text scroll from left to right.
/// </summary>
public class ScrollingDialogue : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] [Range(0f, 20f)] private float scrollSpeed;
    [SerializeField] [Range(0f, 10f)] private float delayBetweenDialogues;

    [SerializeField] string[] dialogueText;

    private float currentTextTimer;
    private float currentDelayTimer;
    private List<string> dialogueTexts;
    private List<Text> dialogues;

    private int numActiveDialogues;

    private void Start() {
        numActiveDialogues = 1;
        currentDelayTimer = 0f;
        currentTextTimer = 0f;
        dialogueTexts = new List<string>();
        dialogues = new List<Text>();

        // Add each text child in the dialogue prefab to some list or something i don't know
        foreach (Text dialogue in transform.GetComponentsInChildren<Text>()) {
            dialogues.Add(dialogue);
            dialogueTexts.Add(dialogue.text);
            dialogue.text = "";
        }
    }

    private void Update() {

        if (numActiveDialogues <= dialogues.Count) {
            if (currentTextTimer < dialogueTexts[numActiveDialogues-1].Length) {
                currentTextTimer += Time.deltaTime * scrollSpeed;

                dialogues[numActiveDialogues-1].text = "";
                int numCharsToPrint = (int)Mathf.Floor(currentTextTimer);

                for (int chars = 0; chars < numCharsToPrint; chars++) {
                    dialogues[numActiveDialogues-1].text += dialogueTexts[numActiveDialogues-1][chars];
                }   
            } else {
                currentDelayTimer += Time.deltaTime;
            }
        }

        
        if (currentDelayTimer >= delayBetweenDialogues) {
            dialogues[numActiveDialogues-1].text = "";
            numActiveDialogues++;
            currentTextTimer = 0f;
            currentDelayTimer = 0f;
        }
    }
}
