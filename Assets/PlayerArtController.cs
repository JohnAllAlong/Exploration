using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerArtController : MonoBehaviour
{
    [SerializeField]
    private GameObject _shadowcaster, _art;
    private bool flip;
    public void Flip ()
    {
        if (flip)
        {

        } else
        {

        }
    }
    
}

[CustomEditor(typeof(PlayerArtController)), CanEditMultipleObjects]
public class PlayerArtControllerEditor : Editor
{
    public void OnSceneGUI()
    {
        PlayerArtController PAC = target as PlayerArtController;
        if (GUILayout.Button("Flip"))
            PAC.Flip();

    }
}
