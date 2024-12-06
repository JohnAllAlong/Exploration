using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectibleSlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private PlayerCollectibleController _collectibleController;

    protected void Start()
    {
        collectibleImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _collectibleController = PlayerData.GetCollectibleController();
        _collectibleController.slots.Add(this);

        //add to the hotbar if this is the "belt" section of the inventory (aka in-inv hotbar)
        if (type == SlotType.hotbar)
            _collectibleController.GetHotbar().Add(this);

        SetImage(_collectibleController.defaultEmptySlotImage);

    }

    protected void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(isOn => { Toggled(); });
    }

    protected void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    public void Toggled()
    {
        _collectibleController.onSlotToggle(this);
    }


    public void OnSelect(BaseEventData _)
    {
        _collectibleController.SetActiveSlot(this);
    }

    public void OnPointerEnter(PointerEventData _)
    {
        _collectibleController.SetActiveSlot(this);
    }


    public void ReplaceSlotContents(CollectibleSlot newSlotContents, bool occupied = true)
    {
        SetImage(newSlotContents.collectibleImage.sprite);
        occupation = newSlotContents.occupation;
        this.occupied = occupied;
        toggle.SetIsOnWithoutNotify(true);
    }

    public void ReplaceSlotContents(bool occupied, Collectible occupation, Image image)
    {
        SetImage(image.sprite);
        this.occupation = occupation;
        this.occupied = occupied;
        toggle.SetIsOnWithoutNotify(true);
    }

    public void ReplaceSlotContents(Collectible newSlotContents, bool occupied = true)
    {
        SetImage(newSlotContents.collectibleImage);
        occupation = newSlotContents;
        this.occupied = occupied;
        toggle.SetIsOnWithoutNotify(true);
    }

    public void WipeSlotContents()
    {
        SetImage(_collectibleController.defaultEmptySlotImage);
        occupation = null;
        occupied = false;
    }

    public void SetImage(Sprite newImage)
    {
        collectibleImage.sprite = newImage;
    }

    public Image GetImage()
    {
        return collectibleImage;
    }

    public Collectible GetOccupation()
    {
        return occupation;
    }

    public bool IsOccupied()
    {
        return occupied;
    }

    public void SetTempImage(Sprite newImage)
    {
        collectibleImage.color = new Color(255, 255, 255, 0.5f);
        collectibleImage.sprite = newImage;
    }

    public SlotType type;
    public int id;
    public Toggle toggle { get; set; }
    public Image collectibleImage;
    public Collectible occupation;
    public bool occupied;
}

public enum SlotType
{
    none,
    backpack,
    keyItems,
    belt,
    hotbar
}