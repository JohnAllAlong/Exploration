using System;
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

    [SerializeField] string[] dialogueTexts;
    [SerializeField] Vector2 defaultPosition;
    [SerializeField] Vector2 defaultSize;
    [SerializeField] int defaultFontSize;
    [SerializeField] Color defaultColor;
    [SerializeField] Font defaultFont;

    private float currentTextTimer;
    private float currentDelayTimer;

    //List of what the output of each dialogue
    private string[] targetTexts;
    
    //List of the actual text from each dialogue
    private List<Text> dialogues;

    
    //The total number of dialogues that were displayed
    private int numDisplayed;


    private void Start() {

        //Reset the timers that control how long the texts stay visible, and the time transitioning from one dialogue to the other
        currentDelayTimer = 0f;
        currentTextTimer = 0f;

        //Sets the initial value for how many dialogues were displayed
        numDisplayed = 0;

        //Declarations
        dialogues = new List<Text>();
        targetTexts = new string[dialogueTexts.Length];

        //Create a dialogue object based on the number of string inputs from the dialogueTexts array. Each object will contain text which represent the dialogues, and added to the "dialogues" list.
        for (int i = 0; i < dialogueTexts.Length; i++) {
            GameObject dialogueObject = new GameObject($"Dialogue[{i}]", typeof(RectTransform), typeof(Text));
            dialogueObject.transform.SetParent(transform);
            dialogueObject.GetComponent<RectTransform>().sizeDelta = defaultSize;
            dialogueObject.transform.position = transform.position;

            dialogues.Add(dialogueObject.GetComponent<Text>());
            dialogues[i].fontSize = defaultFontSize;
            dialogues[i].font = defaultFont;
            dialogues[i].color = defaultColor;
            dialogues[i].alignment = TextAnchor.MiddleCenter;

            targetTexts[i] = dialogueTexts[i];
        }
    }

    private void Update() {

        //If all of the dialogues have not been displayed yet
        if (numDisplayed < dialogues.Count) {

            //If the time to fully display the texts has not yet elapsed
            if (currentTextTimer < targetTexts[numDisplayed].Length)
            {
                //Keep displaying the text for the current dialogue
                DisplayText();
            }
            
            //If the time to display the texts has elapsed, update the delay timer, which is how long it takes before it transitions to the next dialogue
            else {
                currentDelayTimer += Time.deltaTime;
            }
        }
    
        //If the transition timer is done, then move on to the next dialogue
        if (currentDelayTimer >= delayBetweenDialogues)
        {
            MoveToNextDialogue();
        }
    }

    private void MoveToNextDialogue()
    {
        //Reset timer and turn the previous text blank
        dialogues[numDisplayed].text = "";
        numDisplayed++;
        currentTextTimer = 0f;
        currentDelayTimer = 0f;
    }

    private void DisplayText()
    {
        //Update the text timer by how fast the scrollSpeed is
        currentTextTimer += Time.deltaTime * scrollSpeed;

        //Set the current dialogue text to blank
        dialogues[numDisplayed].text = "";
        
        //How many chars to print out for the dialogue text
        int numCharsToPrint = (int)Mathf.Floor(currentTextTimer);

        for (int chars = 0; chars < numCharsToPrint; chars++)
        {
            //Set the texts based on the number of chars to print out and the text timer
            dialogues[numDisplayed].text += targetTexts[numDisplayed][chars];
        }
    }
}
