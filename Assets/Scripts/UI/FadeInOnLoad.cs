using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnLoad : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _fader;
    [SerializeField] private float _fadeTime;

    protected void OnEnable()
    {
        _fader.enabled = true;
    }

    protected void Start()
    {
        GUIUtilitys.FadeOutSprite(_fader, _fadeTime, () =>
        {
            _fader.enabled = false;
        });
    }
}
