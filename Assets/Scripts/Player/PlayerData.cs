using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public class PlayerData : MonoBehaviour
    {
        [Header("Script Refrences")]
        public PlayerMove playerMoveScript;
        public GoGoGadgetGun playerWeaponScript;
        public PlayerCollectibleController playerInventoryScript;

        [Header("Trapdoor Cooldown")]
        public float trapdoorCooldownTime;
        public Timer trapdoorCooldownTimer;

        [Header("Screen")]
        public SpriteRenderer screenFader;

        public static PlayerData singleton;

        protected void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            trapdoorCooldownTimer = new(trapdoorCooldownTime);
        }

        private void Update()
        {
            //trapdoorCooldownTimer.UpdateTimer();
        }

        public static PlayerCollectibleController GetCollectibleController()
        {
            if (singleton.playerInventoryScript == null) return default;
            return singleton.playerInventoryScript;
        }

        public static SpriteRenderer GetScreenFader()
        {
            if (singleton.screenFader == null) return default;
            return singleton.screenFader;
        }

        public static Transform GetTransform()
        {
            return singleton.transform;
        }

        public static PlayerMove GetPlayerMove()
        {
            if (singleton.playerMoveScript == null) return default;
            return singleton.playerMoveScript;
        }

        public static GoGoGadgetGun GetPlayerWeapon()
        {
            if (singleton.playerWeaponScript == null) return default;
            return singleton.playerWeaponScript;
        }
    }
}

