using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;
using System;

public class PlayerCollectibleController : MonoBehaviour
{
    [SerializeField] private List<Collectible> _inventory = new();
    [SerializeField] private Collectible _selected;
    [SerializeField] private List<Collectible> _nearby = new();
    [SerializeField] private int _maxSlots = 4;

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private Transform _playerTransform;

    public Action<Collectible> OnInventoryAddition;
    public Action<Collectible> OnInventoryDrop;

    /// <summary>
    /// drops a collectible from the players inventory
    /// </summary>
    /// <param name="col">collectible to drop</param>
    public void DropCollectible(Collectible col)
    {
        Collectible tryFindCollectible = _inventory.Find(c => c == col);
        if (tryFindCollectible != null)
        {
            tryFindCollectible.transform.position = _playerTransform.position;
            tryFindCollectible.gameObject.SetActive(true);
            _inventory.Remove(tryFindCollectible);
            OnInventoryDrop(tryFindCollectible);
        }
    }


    public void OnceBtnPickupItem(Values input)
    {
        if (_inventory.Count != _maxSlots)
        {
            _inventory.Add(_selected);
            _selected.gameObject.SetActive(false);
            OnInventoryAddition(_selected);
            _selected = null;
        } else
        {
            Debug.LogWarning("Player Inventory Full!");
        }

    }

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
            _selected = closestCol;
        }
        else
        {
            _selected = null;
            _lineRenderer.SetPosition(0, Vector2.zero);
            _lineRenderer.SetPosition(1, Vector2.zero);
        }
    }

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
