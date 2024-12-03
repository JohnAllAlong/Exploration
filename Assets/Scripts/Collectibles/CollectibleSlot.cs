using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleSlot : MonoBehaviour
{
    public SlotType type;
    public Image collectibleImage;
    public Collectible occupation;
    public bool occupied;
}

//TODO re-make this system

public enum SlotType
{
    none,
    backpack,
    keyItems,
    belt
}