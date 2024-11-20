using System.Collections.Generic;
using UnityEngine;

public class CollectibleGUIController : MonoBehaviour
{
    [SerializeField] private PlayerCollectibleController _collectibleController;
    [SerializeField] private Sprite _defaultSlotSprite;
    private List<CollectibleSlot> Slots => _collectibleController.slots;

    // 0 = slot1
    // 1 = slot2
    // etc
    [SerializeField] private CollectibleSlot _selectedSlot;

    //must call on start so the controller can set its vars before GUI controller does
    protected void Start()
    {

        _selectedSlot = Slots[0];

        _collectibleController.OnInventoryAddition += AddNewCollectibleToSlot;
        _collectibleController.OnInventoryDrop += DropCollectibleFromSlot;
        _collectibleController.OnInventorySelection += ScrollCollectibleGUI;
    }

    protected void OnDisable()
    {
        _collectibleController.OnInventoryAddition -= AddNewCollectibleToSlot;
        _collectibleController.OnInventoryDrop -= DropCollectibleFromSlot;       
        _collectibleController.OnInventorySelection -= ScrollCollectibleGUI;
    }

    public void AddNewCollectibleToSlot(Collectible collectible)
    {
        collectible.slot = Slots.IndexOf(_selectedSlot);

        if (_selectedSlot.occupied)
        {
            _selectedSlot.occupation = collectible;
            _selectedSlot.slot = Slots.IndexOf(_selectedSlot);
            _selectedSlot.collectibleImage.sprite = collectible.itemImage;
            _selectedSlot.collectibleName.text = collectible.itemShortName;
        } else
        {
            _selectedSlot.occupation = collectible;
            _selectedSlot.occupied = true;
            _selectedSlot.slot = Slots.IndexOf(_selectedSlot);
            _selectedSlot.collectibleImage.sprite = collectible.itemImage;
            _selectedSlot.collectibleName.text = collectible.itemShortName;
        }
    }

    public void DropCollectibleFromSlot(Collectible collectible)
    {
        CollectibleSlot newSlot = Slots[collectible.slot];
        newSlot.collectibleImage.sprite = _defaultSlotSprite;
        newSlot.collectibleName.text = "";
        Slots[collectible.slot] = newSlot;
        collectible.slot = 0;
    }

    public void ScrollCollectibleGUI(int selectedSlot)
    {
        CollectibleSlot slot = Slots[selectedSlot];
        _selectedSlot.collectibleSelector.SetActive(false);

        slot.collectibleSelector.SetActive(true);
        _selectedSlot = slot;
    }
}
