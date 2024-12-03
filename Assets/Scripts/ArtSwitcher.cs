using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtSwitcher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new();
    public float transitionTime;
    public int currentSprite = 0;
    public Timer timer;


    protected void Start()
    {
        timer = new Timer(transitionTime).OnEnd(() => {

            if (currentSprite >= sprites.Count)
                currentSprite = 0;

            spriteRenderer.sprite = sprites[currentSprite];
            currentSprite++;

        }).Loop(true).StartTimer();
    }

    protected void OnDestroy()
    {
        timer.Reset();
        timer.Destroy();
    }
}
