using System.Collections.Generic;
using UnityEngine;

public class CollectibleGUIManager : MonoBehaviour
{
    [SerializeField] private PlayerCollectibleController _collectibleController;
    [SerializeField] private Sprite _defaultSlotSprite;
    private List<CollectibleSlot> Slots => _collectibleController.slots;

    //TODO Redo the entire GUI for Inventory

    //must call on start so the controller can set its vars before GUI controller does
    protected void Start()
    {
        //_collectibleController.OnInventoryAddition += AddNewCollectibleToSlot;
        // _collectibleController.OnInventoryDrop += DropCollectibleFromSlot;
        //_collectibleController.OnInventorySelection += ScrollCollectibleGUI;
    }

    protected void OnDisable()
    {
        //_collectibleController.OnInventoryAddition -= AddNewCollectibleToSlot;
        //_collectibleController.OnInventoryDrop -= DropCollectibleFromSlot;       
        //_collectibleController.OnInventorySelection -= ScrollCollectibleGUI;
    }
}
