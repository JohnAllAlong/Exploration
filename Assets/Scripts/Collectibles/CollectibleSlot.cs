using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleSlot : MonoBehaviour
{
    public TextMeshProUGUI collectibleName;
    public Image collectibleImage;
    public GameObject collectibleSelector;
    public int slot;
    public bool occupied;
    public Collectible occupation;
}
