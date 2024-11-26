using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;

    [Header("Teleportation")]
    [SerializeField]
    private Timer teleporterCooldown;
    [SerializeField]
    private float teleporterCooldownTime;

    public static PlayerData singleton;
    private void OnEnable()
    {
        teleporterCooldown = new Timer(teleporterCooldownTime).DestroyOnEnd(false);
        singleton = this;
    }

    /// <summary>
    /// get the players transform
    /// </summary>
    /// <returns>Transform</returns>
    public Transform GetPlayerTransform()
    {
        return _playerTransform;
    }

    /// <summary>
    /// get the players cooldown timer for teleportation
    /// </summary>
    /// <returns>timer</returns>
    public Timer GetPlayerTeleporterCooldownTimer()
    {
        return teleporterCooldown;
    }
}
