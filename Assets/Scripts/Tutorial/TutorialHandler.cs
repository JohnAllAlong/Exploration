using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This script handles switching between phases of the tutorial

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _tutorialPhases;
    private bool _weaponCheck;
    [SerializeField] private bool _hasWeapon;
    [SerializeField] private GameObject _tutorialTarget;
    [SerializeField] private DoorInteractor _doorInteractor;
    [SerializeField] private Image _fader;
    public string loadScene;

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

        if (phase == 1)
        {
            _weaponCheck = true;
        }

        if (phase == 3)
        {
            _doorInteractor.onDoorInteraction += DoorInteraction;
        }
    }

    private void DoorInteraction(bool _)
    {
        _fader.enabled = true;
        GUIUtilitys.FadeInSprite(_fader, 2, delegate { 
            SceneManager.LoadScene(loadScene);
        });
    }

    private void OnDisable()
    {
        _doorInteractor.onDoorInteraction -= DoorInteraction;
    }

    private void Update()
    {
        if (_weaponCheck)
        {            
            PlayerCollectibleController playerCollectibleController = PlayerData.GetCollectibleController();
            _hasWeapon = playerCollectibleController.HasCollectable(1) && playerCollectibleController.CollectibleInHotbar(1);
            if (_hasWeapon)
            {
                _weaponCheck = false;
                TutorialPhase(2);
                _tutorialTarget.SetActive(true);
            }                               
        }
    }
}
