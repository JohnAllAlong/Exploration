using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaAttack : BossStateHandler
{
    [SerializeField] private GameObject areaAttackPrefab;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float destroyAttackTime;

    private float currentTimer;

    protected override void OnEnable() {
        currentTimer = 0f;
    }

    

    private void Update() {
        currentTimer += Time.deltaTime * attackSpeed;

        Debug.Log(currentTimer + "   " + RetrieveState());

        if (currentState == bossState.Attacking1 && currentTimer >= 1f) {
            GameObject areaAttack = Instantiate(areaAttackPrefab, playerPos.position, Quaternion.identity);

            Destroy(areaAttack, destroyAttackTime);
        }
    }
}
