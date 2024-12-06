using System.Linq;
using CustomInput.Events;
using Player;
using TMPro;
using UnityEngine;
using System;

public class DoorInteractor : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float _detectionRadius = 3;
    [SerializeField] private int _keyCollectibleID = 0;
    [SerializeField] private GameObject _openPopup;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _removeFromInventoryAfterUse;
    [SerializeField] private InventoryCanvasRenderer _inventoryRenderer;

    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameHandler _gameHandler;
    public bool open;
    public int doorID;

    public Action<bool> onDoorInteraction = delegate { };

    private PlayerCollectibleController _cc;
    private bool _inRange;
    private bool _showPopup = true;
    private bool _spawnedPopup;
    private GameObject _spawnedOpenPopup;

    protected void OnEnable()
    {
        InputHandler.OnceBtnOnInteraction += TryOpenDoor;
    }

    protected void OnDisable()
    {
        InputHandler.OnceBtnOnInteraction -= TryOpenDoor;
    }

    protected void Start()
    {
        if (_gameHandler != null)
        {
            open = _gameHandler.loader.GetDoor(doorID);
            _animator.SetBool("open", open);
            _collider.enabled = !open;
            _showPopup = !open;

            _gameHandler.doors.Add(this);
        }
        _cc = PlayerData.GetCollectibleController();
    }

    public void TryOpenDoor(ReturnData input)
    {
        if (_cc.InvOpen()) return;

        if (_cc.HasCollectable(_keyCollectibleID) && _cc.CollectibleInHotbar(_keyCollectibleID) && _inRange)
        {
            open = !open;
            _animator.SetBool("open", open);

            if (_removeFromInventoryAfterUse)
                _inventoryRenderer.RemoveCollectible(_cc.GetCollectibleInHotbar(_keyCollectibleID));

            _showPopup = !open;
            Destroy(_spawnedOpenPopup);
            _spawnedPopup = false;
            _collider.enabled = !open;
            onDoorInteraction(open);
        }
    }

    protected void Update()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _detectionRadius, Vector2.up);
        if (hits.ToList().Find(h => h.collider.CompareTag("Player")))
        {
            _inRange = true;
            if (!_spawnedPopup && _showPopup)
            {
                _spawnedPopup = true;
                _spawnedOpenPopup = Instantiate(_openPopup);
                _spawnedOpenPopup.transform.position = transform.position + new Vector3(0, -2.5f, 0);
            }
        } else if (_spawnedPopup)
        {
            Destroy(_spawnedOpenPopup);
            _spawnedPopup = false;
        }
    }
}
