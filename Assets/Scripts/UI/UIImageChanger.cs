using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIImageChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Image image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = defaultSprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        image.sprite = hoverSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        image.sprite = defaultSprite;
    }
}
