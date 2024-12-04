using System.Linq;
using UnityEngine;
using CustomInput.Events;
using Player;

public class Trapdoor : MonoBehaviour
{
    [Header("Target Teleportation Door")]
    [SerializeField] private GameObject targetDoor;

    [Header("Trapdoor Sprites")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Runtime Variables")]
    [SerializeField] private bool playerHoveringTrapdoor;
    [SerializeField] private Transform playerTransform;

    protected void OnEnable()
    {
        InputHandler.OnceBtnOnInteraction += AttemptTeleport;
    }

    protected void OnDisable()
    {
        InputHandler.OnceBtnOnInteraction -= AttemptTeleport;
    }

    private void AttemptTeleport(ReturnData input)
    {
        Timer cooldownTimer = PlayerData.GetTrapdoorCooldownTimer();

        if (playerHoveringTrapdoor)
        {
            if (cooldownTimer.IsRunning())
            {
                Debug.LogWarning("Trapdoor on cooldown!");
                return;
            }

            playerTransform.position = targetDoor.transform.position;

            cooldownTimer.OnEnd(() => cooldownTimer.Reset(false)).StartTimer();
        }
    }

    protected void Update()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, spriteRenderer.bounds.size, 0, Vector2.zero);
        RaycastHit2D playerhit = hits.ToList().Find(hits => hits.collider.CompareTag("Player"));
        playerHoveringTrapdoor = playerhit;

        if (playerHoveringTrapdoor)
        {
            playerTransform = playerhit.collider.transform;
            spriteRenderer.sprite = hoverSprite;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}
