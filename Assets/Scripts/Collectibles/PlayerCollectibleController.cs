using System;
using System.Collections.Generic;
using CustomInput.Events;
using UnityEngine;

public class PlayerCollectibleController : MonoBehaviour
{
    [Header("Settings")]
    public float pickupRadius;
    public Sprite defaultEmptySlotImage;

    [Header("Inventory Management")]
    [SerializeField] private List<Collectible> _inventory = new();
    [SerializeField] private List<CollectibleSlot> _hotbar = new();
    public List<CollectibleSlot> slots = new();

    [Header("System")]
    [SerializeField] private List<Collectible> _nearbyCollectibles = new();
    [SerializeField] private Collectible _nearestCollectible;
    [SerializeField] private Collectible _lastNearestCollectible;
    [SerializeField] private CollectibleSlot _currentSelectedSlot;

    public Action<CollectibleSlot> onSlotToggle;
    public Action<Collectible> onInventoryAddition;
    public Action<CollectibleSlot> onSlotSelect;

    [Header("Transforms")]
    [SerializeField] private Transform _dropTransform;
    [SerializeField] private Transform _playerTransform;

    [Header("Prefabs")]
    [SerializeField] private GameObject _pickupCollectiblePopup;

    [Header("Menus")]
    [SerializeField] private GameObject _fullInventory;


    private bool _spawnedPopup = false;
    private GameObject _spawnedPopupObject;

    protected void Start()
    {
        //LateStart
        new Timer(0.5f).OnEnd(delegate { _fullInventory.SetActive(false); }).StartTimer();
    }

    protected void OnEnable()
    {
        InputHandler.OnceBtnOnInteraction += OnceBtnInteraction;
    }

    protected void OnDisable()
    {
        InputHandler.OnceBtnOnInteraction -= OnceBtnInteraction;
    }

    public CollectibleSlot GetSelectedSlot()
    {
        return _currentSelectedSlot;
    }

    public Transform GetDropPoint()
    {
        return _dropTransform;
    }

    public List<CollectibleSlot> GetHotbar()
    {
        return _hotbar;
    }

    public CollectibleSlot GetNextFreeSlot()
    {
        List<CollectibleSlot> sorted = SortSlots();
        foreach (CollectibleSlot slot in sorted)
        {
            if (slot.occupied == false) return slot;
        }
        return null;
    }

    public List<CollectibleSlot> SortSlots()
    {
        List<CollectibleSlot> backpack = GetListOfSlotType(SlotType.backpack);
        List<CollectibleSlot> keyItems = GetListOfSlotType(SlotType.keyItems);
        List<CollectibleSlot> belt = GetListOfSlotType(SlotType.belt);

        List<CollectibleSlot> sortedBackpack = new();
        List<CollectibleSlot> sortedKeyItems = new();
        List<CollectibleSlot> sortedBelt = new();

        for (int i = 0; i < backpack.Count; i++)
        {
            backpack.Find(s => s.id == i);
            sortedBackpack.Add(backpack[i]);
        }

        sortedBackpack.Reverse();

        for (int i = 0; i < keyItems.Count; i++)
        {
            keyItems.Find(s => s.id == i);
            sortedKeyItems.Add(keyItems[i]);
        }

        sortedKeyItems.Reverse();

        for (int i = 0; i < belt.Count; i++)
        {
            belt.Find(s => s.id == i);
            sortedBelt.Add(belt[i]);
        }

        sortedBelt.Reverse();

        sortedBackpack.AddRange(sortedKeyItems);
        sortedBackpack.AddRange(sortedBelt);
        return sortedBackpack;
    }


    private void OnceBtnInteraction(ReturnData input)
    {
        //dont run if movement is paused
        if (Events.IsPaused("KeyboardMove", "GamepadMove").Contains(true)) return;

        if (_nearestCollectible != null)
        {
            //check if slot is free in the inventory
            if (GetNextFreeSlot() != null)
            {
                _nearestCollectible.gameObject.SetActive(false);
                _inventory.Add(_nearestCollectible);
                onInventoryAddition(_nearestCollectible);
            }
            else
            {
                //no slots are free dont do anything
                Debug.LogWarning("Inventory Full");
            }
        }
    }

    public void OnceBtnOpenInv(ReturnData input)
    {
        _fullInventory.SetActive(!_fullInventory.activeInHierarchy);

        if (_fullInventory.activeInHierarchy)
        {
            Events.Pause("KeyboardMove", "GamepadMove");
        }
        else
        {
            Events.Resume("KeyboardMove", "GamepadMove");
        }
    }

    /// <summary>
    /// Checks if the inventory has a collectible by its ID
    /// </summary>
    /// <param name="collectibleID">collectible to find</param>
    public bool HasCollectable(int collectibleID)
    {
        foreach (Collectible collectible in _inventory)
        {
            if (collectible.collectibleID == collectibleID)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the hotbar has a collectible by its ID
    /// </summary>
    /// <param name="collectibleID">collectible to find</param>
    public bool CollectibleInHotbar(int collectibleID)
    {
        foreach (CollectibleSlot slot in _hotbar)
        {
            if (slot.occupation != null)
                if (slot.occupation.collectibleID == collectibleID)
                {
                    return true;
                }
        }
        return false;
    }

    /// <summary>
    /// gets an item in the hotbar by its ID
    /// </summary>
    /// <param name="collectibleID">collectible to find</param>
    public CollectibleSlot GetCollectibleInHotbar(int collectibleID)
    {
        foreach (CollectibleSlot slot in GetListOfSlotType(SlotType.belt))
        {
            if (slot.occupation != null)
                if (slot.occupation.collectibleID == collectibleID)
                {
                    return slot;
                }
        }
        return null;
    }

    public void SetActiveSlot(CollectibleSlot slot)
    {
        _currentSelectedSlot = slot;
    }

    public CollectibleSlot GetActiveSlot()
    {
        return _currentSelectedSlot;
    }

    public bool IsInventoryOpen()
    {
        return _fullInventory.activeInHierarchy;
    }

    public List<CollectibleSlot> GetListOfSlotType(SlotType type)
    {
        return slots.FindAll(s => s.type == type);
    }

    protected void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(_playerTransform.position, pickupRadius, Vector2.up);
        List<Collectible> _cache = new List<Collectible>();

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.collider.gameObject.TryGetComponent(out Collectible col))
            {
                _cache.Add(col);
            }
        }
        _nearbyCollectibles = _cache;
        _nearestCollectible = GetClosestCollectible(_playerTransform.position, _nearbyCollectibles);

        if (_nearestCollectible != null && !_spawnedPopup)
        {
            _lastNearestCollectible = _nearestCollectible;
            _spawnedPopup = true;
            _spawnedPopupObject = Instantiate(_pickupCollectiblePopup);
            _spawnedPopupObject.transform.position = _nearestCollectible.transform.position + new Vector3(0, 1, 0);
        }
        else if (_nearestCollectible == null || _nearestCollectible != _lastNearestCollectible)
        {
            Destroy(_spawnedPopupObject);
            _spawnedPopup = false;
        }
    }

    /// <summary>
    /// Gets the closest collectible relative to the provided pos
    /// </summary>
    /// <param name="pos">relative position</param>
    /// <param name="nearby">list of collectibles nearby to sort</param>
    /// <returns>nearest collectible as an object</returns>
    private Collectible GetClosestCollectible(Vector2 pos, List<Collectible> nearby)
    {
        Collectible closest = null;
        float minDist = Mathf.Infinity;
        foreach (Collectible col in nearby)
        {
            float dist = Vector3.Distance(col.transform.position, pos);
            if (dist < minDist)
            {
                closest = col;
                minDist = dist;
            }
        }
        return closest;
    }
}
