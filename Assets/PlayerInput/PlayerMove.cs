using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;

    public void VecKeyboardMove(Values input)
    {
        print(input.vector);
        transform.position += (Vector3)input.vector * Time.deltaTime * _moveSpeed;
    }

    public void VecGamepadMove(Values input)
    {
        print(input.vector);
        transform.position += (Vector3)input.vector * Time.deltaTime * _moveSpeed;
    }
}
