using System.Collections.Generic;
using CustomInput;
using CustomInput.Events;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryCanvasRenderer : MonoBehaviour
{
    [SerializeField] private bool _openedInv;
    [SerializeField] private CollectibleSlot _transferingItem;
    private PlayerCollectibleController _collectibleController;

    protected void Start()
    {
        _collectibleController = PlayerData.GetCollectibleController();
        _collectibleController.onSlotToggle += SlotToggled;
        _collectibleController.onInventoryAddition += InventoryAddition;
    }
    protected void OnDisable()
    {
        _collectibleController.onSlotToggle -= SlotToggled;
        _collectibleController.onInventoryAddition -= InventoryAddition;
    }

    public void RemoveCollectible(CollectibleSlot slot)
    {
        Transform dropPoint = _collectibleController.GetDropPoint();
        List<CollectibleSlot> hotbarSlots = _collectibleController.GetHotbar();

        Destroy(slot.occupation.gameObject);
        slot.WipeSlotContents();

        //clear htobar if neccesary
        if (slot.type == SlotType.belt)
        {
            CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == slot.id);
            foundHotbarSlot.ReplaceSlotContents(slot);
        }
    }
    public void OnceBtnDropCollectible(ReturnData _)
    {
        if (Events.IsPaused("KeyboardMove", "GamepadMove").Contains(false)) return;
        Transform dropPoint = _collectibleController.GetDropPoint();
        List<CollectibleSlot> hotbarSlots = _collectibleController.GetHotbar();

        if (_transferingItem == null)
        {
            //check if slot has item
            CollectibleSlot tryGetSlot = _collectibleController.GetSelectedSlot();
            if (tryGetSlot != null)
            {
                //check if slot has item in it
                if (tryGetSlot.occupied)
                {
                    //drop selected item
                    tryGetSlot.occupation.gameObject.transform.position = dropPoint.position;
                    tryGetSlot.occupation.gameObject.SetActive(true);
                    tryGetSlot.WipeSlotContents();

                    //clear htobar if neccesary
                    if (tryGetSlot.type == SlotType.belt)
                    {
                        CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == tryGetSlot.id);
                        foundHotbarSlot.ReplaceSlotContents(tryGetSlot);
                    }
                    tryGetSlot = null;
                }
            }
            return;
        };

        //drop transfer item
        _transferingItem.occupation.gameObject.transform.position = dropPoint.position;
        _transferingItem.occupation.gameObject.SetActive(true);
        _transferingItem.WipeSlotContents();

        //clear hotbar if neccesary
        if (_transferingItem.type == SlotType.belt)
        {
            CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == _transferingItem.id);
            foundHotbarSlot.WipeSlotContents();
        }
        _transferingItem = null;

    }

    private void InventoryAddition(Collectible newAddition)
    {

        //check if selected slot is occupied, if not store the item there first before the next available slot found
        if (!_collectibleController.GetSelectedSlot().occupied)
        {
            _collectibleController.GetSelectedSlot().ReplaceSlotContents(newAddition);
        }
        else
        {
            //store in next available slot
            CollectibleSlot nextFree = _collectibleController.GetNextFreeSlot();
            nextFree.ReplaceSlotContents(newAddition);
        }
    }

    private void SlotToggled(CollectibleSlot slot)
    {
        //dont allow movement of self
        if (slot == _transferingItem)
        {
            _transferingItem = null;
            slot.toggle.SetIsOnWithoutNotify(true);
            return;
        }

        //dont allow movement in the belt (covers up a bug i couldnt fix :/)
        if (_transferingItem != null)
            if (slot.type == SlotType.belt && _transferingItem.type == SlotType.belt)
            {
                slot.toggle.SetIsOnWithoutNotify(true);
                _transferingItem.toggle.SetIsOnWithoutNotify(true);
                _transferingItem = null;
            };


        if (_transferingItem == null && slot.occupied)
        {
            //item has been picked up and is transfering
            _transferingItem = slot;
        }
        else if (_transferingItem != null)
        {
            //item has been dropped from transfering int another slot
            if (slot.occupied)
            {
                //slot has item in it, move to old spot

                CollectibleSlot cachedSlot = Instantiate(slot);
                bool cacheOldSlotOccupied = cachedSlot.IsOccupied();
                Collectible cacheOldSlotOcupation = cachedSlot.GetOccupation();
                Image cacheOldSlotImage = cachedSlot.GetImage();

                slot.ReplaceSlotContents(_transferingItem);
                _transferingItem.ReplaceSlotContents(cacheOldSlotOcupation, cacheOldSlotOcupation, cacheOldSlotImage);

                //hotbar support
                List<CollectibleSlot> hotbarSlots = _collectibleController.GetHotbar();

                //check if hotbar
                if (slot.type == SlotType.belt)
                {
                    CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == slot.id);
                    foundHotbarSlot.ReplaceSlotContents(cacheOldSlotOccupied, cacheOldSlotOcupation, cacheOldSlotImage);
                }

                if (_transferingItem.type == SlotType.belt)
                {
                    CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == _transferingItem.id);
                    foundHotbarSlot.WipeSlotContents();
                }

                Destroy(cachedSlot);
            }
            else
            {
                //slot has no item in it so dont swap items
                slot.ReplaceSlotContents(_transferingItem);
                _transferingItem.WipeSlotContents();
                List<CollectibleSlot> hotbarSlots = _collectibleController.GetHotbar();

                //check if hotbar
                if (slot.type == SlotType.belt)
                {
                    CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == slot.id);
                    foundHotbarSlot.ReplaceSlotContents(slot);
                }

                if (_transferingItem.type == SlotType.belt)
                {
                    CollectibleSlot foundHotbarSlot = hotbarSlots.Find(s => s.id == _transferingItem.id);
                    foundHotbarSlot.WipeSlotContents();
                }
            }
            _transferingItem = null;
        }
        else
        {
            //fallback - dont allow toggle if no intem in slot
            slot.toggle.SetIsOnWithoutNotify(true);
        }
    }



    protected void Update()
    {
        if (_collectibleController.IsInventoryOpen()  && !_openedInv)
        {
            _openedInv = true;
            if (Devices.GetCurrent() as Gamepad != null)
                if (_collectibleController.GetActiveSlot() != null )
                    _collectibleController.GetActiveSlot().toggle.Select();
        }
        else if (!_collectibleController.IsInventoryOpen())
        {
            //reset transfering item if transfering item exists
            if (_transferingItem != null)
            {
                _collectibleController.SetActiveSlot(_transferingItem);
                _transferingItem.toggle.SetIsOnWithoutNotify(true);
                _transferingItem = null;
            }
            _openedInv = false;
        }
    }
}
