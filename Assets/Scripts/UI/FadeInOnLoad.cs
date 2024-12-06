using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOnLoad : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _fader;
    [SerializeField] private Image _imageFader;
    [SerializeField] private float _fadeTime;

    protected void OnEnable()
    {
        if (_imageFader == null)
        {
            _fader.enabled = true;
            _fader.color = new Color(_fader.color.r, _fader.color.g, _fader.color.b, 1);
        }
        else
        {
            _imageFader.enabled = true;
            _imageFader.color = new Color(_imageFader.color.r, _imageFader.color.g, _imageFader.color.b, 1);
        }
    }

    protected void Start()
    {

        new Timer(1).OnEnd(() => {
            if (_imageFader == null)
            {
                GUIUtilitys.FadeOutSprite(_fader, _fadeTime, () =>
                {
                    _fader.enabled = false;
                });
            }
            else
            {
                GUIUtilitys.FadeOutSprite(_imageFader, _fadeTime, () =>
                {
                    _imageFader.enabled = false;
                });
            }
        }).StartTimer();
    }
}
