using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : MonoBehaviour
{
    //loads level 2
    public void loadnext()
    {
        GameProgression.LoadSecondLevel();
    }
}
