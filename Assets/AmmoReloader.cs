using UnityEditor;
using UnityEngine;

public class AmmoReloader : MonoBehaviour
{

}

[CustomEditor(typeof(AmmoReloader))]
public class AmmoReloaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AmmoReloader ammoReloader = (AmmoReloader)target;

        if (GUILayout.Button("Fill From 0"))
        {
        }
    }
}
