using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles switching between phases of the tutorial

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _tutorialPhases;

    private void Start()
    {
        // Start phase 0 of the tutorial, the one that teaches the player how to move
        TutorialPhase(0);
    }

    // When a tutorial phase is complete, call this function
    // pass in number of desired phase you wish to activate
    public void TutorialPhase(int phase)
    {
        // loop through array and deactivate all dialogue prefabs
        for (int i = 0; i < _tutorialPhases.Length; i++)
        {
            _tutorialPhases[i].SetActive(false);
        }

        //enable the desired dialogue prefab
        _tutorialPhases[phase].gameObject.SetActive(true);
    }
}
