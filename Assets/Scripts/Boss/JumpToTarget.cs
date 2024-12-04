using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToTarget : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    private Transform mantisPos;
    private bool isJumping;

    private float currentTimer;

    private Transform defaultTarget;

    private Vector3 targetPos;
    private Vector3 initialPos;

    private Animator mantisAnimator;

    private void OnEnable() {
        mantisPos = transform.parent;
        isJumping = false;

        currentTimer = 0f;

        defaultTarget = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
        mantisAnimator = GetComponent<Animator>();
    }

    public void JumpEvent() {
        targetPos = defaultTarget.position;
        initialPos = mantisPos.transform.position;
        currentTimer = 0f;

        isJumping = true;
    }

    private void Update() {
        //if the mantis is still jumping
        if (isJumping) {
            currentTimer += Time.deltaTime * jumpPower;

            //move towards player            
            if (currentTimer < 1) {
                mantisPos.transform.position = Vector3.Lerp(initialPos, targetPos, currentTimer);
            } else {
                isJumping = false;
                mantisAnimator.SetTrigger("Land");
            }
        }
    }
}
