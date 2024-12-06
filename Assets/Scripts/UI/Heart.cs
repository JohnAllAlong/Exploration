using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public SpriteRenderer heart;

    public void TakeawayHeart()
    {
        heart.enabled = false;
    }
}
