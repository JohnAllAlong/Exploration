using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    public void KeyboardMove(Values input)
    {
        print(input.vector);
        transform.position += (Vector3)input.vector * Time.deltaTime * _moveSpeed;
    }

    public void GamepadMove(Values input)
    {
        print(input.vector);
        transform.position += (Vector3)input.vector * Time.deltaTime * _moveSpeed;
    }
}
