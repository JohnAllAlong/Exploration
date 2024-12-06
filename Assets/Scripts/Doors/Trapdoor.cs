using System.Linq;
using UnityEngine;
using CustomInput.Events;
using Player;
using UnityEngine.SceneManagement;

public class Trapdoor : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject targetDoor;
    [SerializeField] private string targetScene = "";

    [Header("Trapdoor Sprites")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Popup Settings")]
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject spawnedPopupObject;
    [SerializeField] private bool spawnedPopup;
    [SerializeField] private float popupVisibleRadius;


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
        if (playerHoveringTrapdoor)
        {
            if (PlayerData.singleton.trapdoorCooldownTimer.IsRunning())
            {
                Debug.LogWarning("Trapdoor on cooldown!");
                return;
            }


            GUIUtilitys.FadeInSprite(PlayerData.GetScreenFader(), 0.500f, () =>
            {
                new Timer(0.2f).OnEnd(() => {
                    if (targetScene != "")
                    {
                        SceneManager.LoadScene(targetScene);
                        return;
                    }
                    playerTransform.position = targetDoor.transform.position;
                    GUIUtilitys.FadeOutSprite(PlayerData.GetScreenFader(), 0.500f);
                }).StartTimer();

                PlayerData.singleton.trapdoorCooldownTimer.OnEnd(() =>
                {
                    PlayerData.singleton.trapdoorCooldownTimer = new(PlayerData.singleton.trapdoorCooldownTime);
                }).StartTimer();
            });
        }
    }

    protected void Update()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, spriteRenderer.bounds.size, 0, Vector2.zero);
        RaycastHit2D playerHit = hits.ToList().Find(hits => hits.collider.CompareTag("Player"));
        playerHoveringTrapdoor = playerHit;

        if (playerHoveringTrapdoor)
        {
            playerTransform = playerHit.collider.transform;
            spriteRenderer.sprite = hoverSprite;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }

        RaycastHit2D[] interactionHits = Physics2D.CircleCastAll(transform.position, popupVisibleRadius, Vector2.up);
        RaycastHit2D playerInteractionHit = interactionHits.ToList().Find(hits => hits.collider.CompareTag("Player"));
        bool hitPlayer = playerInteractionHit;

        if (!spawnedPopup && hitPlayer)
        {
            spawnedPopup = true;
            spawnedPopupObject = Instantiate(popup);
            spawnedPopupObject.transform.position = transform.position + new Vector3(0, 1, 0);
        }
        else if (!hitPlayer)
        {
            Destroy(spawnedPopupObject);
            spawnedPopup = false;
        }
    }
}
