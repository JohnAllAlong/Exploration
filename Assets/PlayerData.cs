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

        [Header("Trapdoor Cooldown")]
        public float trapdoorCooldownTime;
        public Timer trapdoorCooldownTimer;

        public static PlayerData singleton;

        protected void Awake()
        {
            singleton = this;
            trapdoorCooldownTimer = new Timer(trapdoorCooldownTime).DestroyOnEnd(false);
        }


        public static Timer GetTrapdoorCooldownTimer()
        {
            return singleton.trapdoorCooldownTimer;
        }
        public static float GetTrapdoorCooldownTime()
        {
            return singleton.trapdoorCooldownTime;
        }


        public static Transform GetTransform()
        {
            return singleton.transform;
        }

        public static PlayerMove GetPlayerMove()
        {
            return singleton.playerMoveScript;
        }

        public static GoGoGadgetGun GetPlayerWeapon()
        {
            return singleton.playerWeaponScript;
        }
    }
}

