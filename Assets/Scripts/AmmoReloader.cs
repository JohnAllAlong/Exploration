using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AmmoReloader : MonoBehaviour
{
    public Slider ammoSlider;

    public void Fill(float time)
    {
        new Timer(time).OnUpdate((elapsed) =>
        {
            float lerp = Mathf.Lerp(ammoSlider.minValue, ammoSlider.maxValue, Mathf.SmoothStep(0.0f, 1.0f, elapsed / time));
            ammoSlider.value = lerp;
        }).StartTimer();
    }

    public void Empty(float time)
    {
        new Timer(time).OnUpdate((elapsed) =>
        {
            float lerp = Mathf.Lerp(ammoSlider.maxValue, ammoSlider.minValue, Mathf.SmoothStep(0.0f, 1.0f, elapsed / time));
            ammoSlider.value = lerp;
        }).StartTimer();
    }
}

[CustomEditor(typeof(AmmoReloader))]
public class AmmoReloaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AmmoReloader ammoReloader = (AmmoReloader)target;

        if (GUILayout.Button("Fill"))
        {
            ammoReloader.Fill(4);
        }

        if (GUILayout.Button("Empty"))
        {
            ammoReloader.Empty(4);
        }
    }
}
