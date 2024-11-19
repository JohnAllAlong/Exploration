using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleGUIController : MonoBehaviour
{
    [SerializeField] private PlayerCollectibleController _collectibleController;
    [SerializeField] private GameObject _slotsParent;
    [SerializeField] private Image _defaultSlotImage;

    // 0 = slot1
    // 1 = slot2
    // etc
    [SerializeField] private List<Image> _slots;
    [SerializeField] private Image _nextAvailableSlot;

    protected void OnEnable()
    {
        for (int i = 0; i != _slotsParent.transform.childCount; i++) {
            GameObject slot = _slotsParent.transform.GetChild(i).gameObject;
            _slots.Add(slot.GetComponent<Image>());
        }
        _nextAvailableSlot = _slots[0];

        _collectibleController.OnInventoryAddition += AddNewCollectibleToSlot;
        _collectibleController.OnInventoryDrop += DropCollectibleFromSlot;
    }

    protected void OnDisable()
    {
        _collectibleController.OnInventoryAddition -= AddNewCollectibleToSlot;
        _collectibleController.OnInventoryDrop -= DropCollectibleFromSlot;
    }

    public void AddNewCollectibleToSlot(Collectible collectible)
    {
        _nextAvailableSlot.sprite = collectible.itemImage;
        collectible.slot = _slots.IndexOf(_nextAvailableSlot);
        _nextAvailableSlot.transform.GetComponentInChildren<TextMeshProUGUI>().text = collectible.itemShortName;
        _nextAvailableSlot = _slots[_slots.IndexOf(_nextAvailableSlot) + 1];
    }

    public void DropCollectibleFromSlot(Collectible collectible)
    {
        Image slot = _slots[collectible.slot];
        slot = _defaultSlotImage;
        slot.transform.GetComponentInChildren<TextMeshProUGUI>().text = "";
        _slots.RemoveAt(collectible.slot);
        _nextAvailableSlot = _slots[collectible.slot];
        collectible.slot = 0;
    }
}
