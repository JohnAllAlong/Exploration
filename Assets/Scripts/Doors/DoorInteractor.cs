using CustomInput.Events;
using TMPro;
using UnityEngine;

public class DoorInteractor : MonoBehaviour
{
    [Header("System Set Variables")]
    [SerializeField] private bool _hasKey = false;
    [SerializeField] private bool _inRange = false;

    [Header("Important")]
    [SerializeField] private Vector2 _doorDirection = Vector2.zero;
    [SerializeField] private int _keyCollectibleID = 0;
    [SerializeField] private string _playerDoesntHaveKeyText;
    [SerializeField] private string _playerHasKeyText;


    [Header("Manually Set Variables")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private TextMeshProUGUI _alertText;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _doorObject;
    [SerializeField] private PlayerCollectibleController _playerCollectibleController;


    private void OnEnable()
    {
        //InputHandler.OnceBtnOnInteractionUse += TryOpenDoor;
    }

    private void OnDisable()
    {
        //InputHandler.OnceBtnOnInteractionUse -= TryOpenDoor;
    }

    public void TryOpenDoor(ReturnData input)
    {
        if (!_doorObject.activeInHierarchy || !_hasKey) return;

        //_playerCollectibleController.UseCollectable(_keyCollectibleID);
        _doorObject.SetActive(false);
        _lineRenderer.enabled = false;
        _alertText.text = "";

        //disable this script
        enabled = false;
    }

    private void Update()
    {
        if (!_doorObject.activeInHierarchy) return;

        //check each frame if the player has the key
        if (_playerCollectibleController.HasCollectable(_keyCollectibleID) && !_hasKey)
        {
            _hasKey = true;
        } else
        {
            _hasKey = false;
        }

        if (_inRange)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, _playerTransform.position);
            LayerMask mask = LayerMask.NameToLayer("Door");
            RaycastHit2D hit = Physics2D.Raycast(_playerTransform.position, _doorDirection, Mathf.Infinity, mask);
            Debug.DrawRay(_playerTransform.position, _doorDirection, Color.blue);
            if (hit)
            {
                _lineRenderer.SetPosition(1, hit.point);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!_doorObject.activeInHierarchy) return;
        if (collider.CompareTag("Player") && _hasKey)
        {
            _inRange = true;
            _alertText.text = _playerHasKeyText;
        }
        else if (collider.CompareTag("Player"))
        {
            _alertText.text = _playerDoesntHaveKeyText;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (!_doorObject.activeInHierarchy) return;
        if (collider.CompareTag("Player"))
        {
            _inRange = false;
            _lineRenderer.enabled = false;
            _alertText.text = "";
        }
    }
}
