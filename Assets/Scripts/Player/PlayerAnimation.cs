using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;   

    public void SetState(int state)
    {
        _animator.SetInteger("State", state);
    }
}
