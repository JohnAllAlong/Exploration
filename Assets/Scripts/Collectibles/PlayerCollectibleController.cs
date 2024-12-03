using System.Collections.Generic;
using CustomInput.Events;
using UnityEngine;

public class PlayerCollectibleController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float pickupRadius;
    [SerializeField] private int _maxSlots = 4;

    [Header("System")]
    [SerializeField] private List<Collectible> _inventory = new();
    public List<CollectibleSlot> slots = new();
    [SerializeField] private List<Collectible> _nearbyCollectibles = new();
    [SerializeField] private Collectible _nearestCollectible;
    [SerializeField] private Collectible _lastNearestCollectible;

    [Header("Transforms")]
    [SerializeField] private Transform _dropTransform;
    [SerializeField] private Transform _playerTransform;

    [Header("Prefabs")]
    [SerializeField] private GameObject _pickupCollectiblePopup;

    [Header("Menus")]
    [SerializeField] private GameObject _fullInventory;


    private bool _spawnedPopup = false;
    private GameObject _spawnedPopupObject;

    protected void OnEnable()
    {
        InputHandler.OnceBtnOnInteraction += OnceBtnInteraction;
    }

    protected void OnDisable()
    {
        InputHandler.OnceBtnOnInteraction -= OnceBtnInteraction;
    }

    private void OnceBtnInteraction(ReturnData input)
    {
        if (_nearestCollectible != null)
        {
            Destroy(_nearestCollectible.gameObject);
            _inventory.Add(_nearestCollectible);
        }
    }

    public void OnceBtnOpenInv(ReturnData input)
    {
        _fullInventory.SetActive(!_fullInventory.activeInHierarchy);

        if (_fullInventory.activeInHierarchy)
        {
            Events.Pause("KeyboardMove", "GamepadMove");
        } else
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

    protected void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(_playerTransform.position, pickupRadius, Vector2.up);
        List<Collectible> _cache = new List<Collectible>();

        for (int i = 0; i < hits.Length; i++) {
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
        } else if (_nearestCollectible == null || _nearestCollectible != _lastNearestCollectible)
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
