using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLampFluctuator : MonoBehaviour
{
    [SerializeField] private Light2D _light;

    [Header("Fluctuation Transition Length")]
    [SerializeField] private float _TransitionTime;


    [Header("Random Light Radius")]
    [SerializeField] private float _minRadius;
    [SerializeField] private float _maxRadius;

    private float radiusCached = 0;
    private float radius = 0;
    private float wantedRadius = 0;


    protected void Start()
    {
        radiusCached = _light.pointLightInnerRadius;

        new Timer(_TransitionTime).OnUpdate((elapsed) => {

            if (wantedRadius == 0)
                wantedRadius = Random.Range(_minRadius, _maxRadius);

            radius = Mathf.Lerp(radiusCached, wantedRadius, elapsed / _TransitionTime);
            _light.pointLightInnerRadius = radius;
        }).OnEnd(() => {
            wantedRadius = 0;
            radiusCached = radius;
            radius = 0;
        }).Loop(true).StartTimer();
    }
}
