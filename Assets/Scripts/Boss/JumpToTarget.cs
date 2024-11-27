using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToTarget : MonoBehaviour
{
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpPower;
    private Transform mantisPos;
    private bool isJumping;

    private float currentTimer;

    private Transform defaultTarget;

    private Vector2 directionToTarget;

    private Animator mantisAnimator;

    private void OnEnable() {
        mantisPos = transform.parent;
        isJumping = false;

        currentTimer = 0f;

        defaultTarget = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
        mantisAnimator = GetComponent<Animator>();
    }

    public void JumpEvent() {
        isJumping = true;
        Vector3 targetPos = defaultTarget.position;

        directionToTarget = (targetPos - mantisPos.position).normalized;
    }

    private void Update() {
        if (isJumping && currentTimer < jumpDuration) {
            currentTimer += Time.deltaTime;
            mantisPos.Translate(directionToTarget * jumpPower * Time.deltaTime);
            
        } else if (isJumping && currentTimer >= jumpDuration) {
            isJumping = false;
            mantisAnimator.SetTrigger("Land");
            currentTimer = 0;
        }
    }


}
