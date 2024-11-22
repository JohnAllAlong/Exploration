using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;
using System;
using Saveables;
using Saving;

public class PlayerCollectibleController : MonoBehaviour
{
    [SerializeField] private List<Collectible> _inventory = new();
    [SerializeField] private GameObject _slotsParent;
    public List<CollectibleSlot> slots = new();

    [SerializeField] private Collectible _nearest;
    [SerializeField] private List<Collectible> _nearby = new();
    [SerializeField] private int _maxSlots = 4;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _dropTransform;

    public Action<Collectible> OnInventoryAddition;
    public Action<Collectible> OnInventoryDrop;
    public Action<int> OnInventorySelection;
    public Action<Collectible> OnInventoryCollectibleUse = (c) => { };

    protected void OnEnable()
    {
        for (int i = 0; i != _slotsParent.transform.childCount; i++)
        {
            GameObject slot = _slotsParent.transform.GetChild(i).gameObject;
            slots.Add(slot.GetComponent<CollectibleSlot>());
        }
    }

    /// <summary>
    /// drops a collectible from the players inventory
    /// </summary>
    /// <param name="col">collectible to drop</param>
    /// <param name="removeFromInventory">if set to true will drop item but NOT remove it from the inventory</param>
    public void DropCollectible(Collectible col, bool removeFromInventory = true, bool removeCompletely = false)
    {
        if (col == null) return; 
        Collectible tryFindCollectible = _inventory.Find(c => c.collectibleRID == col.collectibleRID);
        if (tryFindCollectible != null)
        {
            if (removeFromInventory)
                _inventory.Remove(tryFindCollectible);

            tryFindCollectible.transform.position = _dropTransform.position;
            tryFindCollectible.gameObject.SetActive(true);
            
            OnInventoryDrop(tryFindCollectible);
            if (removeCompletely)
            {
                Destroy(tryFindCollectible.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Cant find the item to drop in the inventory (is the slot empty?)");
        }
    }

    /// <summary>
    /// Uses a collectable via provided ID
    /// </summary>
    /// <param name="collectibleID">collectible to use</param>
    public void UseCollectable(int collectibleID)
    {
        foreach (Collectible collectible in _inventory)
        {
            if (collectible.collectibleID == collectibleID)
            {
                OnInventoryCollectibleUse(collectible);
                DropCollectible(collectible, true, true);
                return;
            }
        }
        Debug.LogWarning("Couldnt find collectible with id of " + collectibleID + " to use in the player inventory!");
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

    //input handlers
    //gamepad/keyboard

    //TODO make new script for these

    public void OnceVecScrollInventory(Values input)
    {
        //OnInventorySelection(_selctedSlot);
    }

    public void OnceBtnPickupItem(Values _)
    {
        return;
        /*
        if (_nearest == null) return;
        
        if (_inventory.Count != _maxSlots)
        {
            //check if slot is the same slot
            if (slots[_selctedSlot].occupied)
            {
                //print("Slot Occupied");
                //slot is occupied 

                //drop old collectible
                DropCollectible(_inventory.Find(c => c.collectibleRID == slots[_selctedSlot].occupation.collectibleRID));

                //set the random ID of the collectible
                _nearest.collectibleRID = (uint)UnityEngine.Random.Range(0, 99999999);

                //set slot with new collectible
                _inventory.Add(_nearest);

                _nearest.gameObject.SetActive(false);

                OnInventoryAddition(_nearest);
                _nearest = null;
                return;
            }
            else
            {
                //print("Slot Not Occupied");
                //slot is empty, fill

                //set the random ID of the collectible
                _nearest.collectibleRID = (uint)UnityEngine.Random.Range(0, 99999999);

                _inventory.Add(_nearest);
                _nearest.gameObject.SetActive(false);
                OnInventoryAddition(_nearest);
                _nearest = null;
                return;
            }
        }
        */
    }

    public void OnceBtnDropItem(Values _)
    {
        /*
           DropCollectible(_inventory.Find(c => c.collectibleRID == slots[_selctedSlot].occupation.collectibleRID));  
         */
    }


    //

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Collectible col))
        {
            _nearby.Add(col);
        }
    }

    protected void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Collectible col))
        {
            _nearby.Remove(col);
        }
    }

    protected void Update()
    {
        Collectible closestCol = GetClosestCollectible(_playerTransform.position, _nearby);
        if (closestCol != null)
        {
            _lineRenderer.SetPosition(0, _playerTransform.position);
            _lineRenderer.SetPosition(1, closestCol.transform.position);
            _nearest = closestCol;
        }
        else
        {
            _nearest = null;
            _lineRenderer.SetPosition(0, Vector2.zero);
            _lineRenderer.SetPosition(1, Vector2.zero);
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
