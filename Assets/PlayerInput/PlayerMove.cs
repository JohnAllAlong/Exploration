using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;
    [SerializeField] public bool _flipped;
    [SerializeField] public PlayerFlip _flipper;

    public void VecKeyboardMove(Values input)
    {
        //print(input.vector);
        if (input.vector == Vector2.left && !_flipped)
        {
            _flipper.Flip(Vector2.left);
            _flipped = true;
        }

        if (input.vector == Vector2.right && !_flipped)
        {
            _flipper.Flip(Vector2.right);
            _flipped = true;
        }

        transform.position += (Vector3)input.vector * Time.deltaTime * _moveSpeed;
    }

    public void VecGamepadMove(Values input)
    {
        if (input.vector == Vector2.left && !_flipped)
        {
            _flipper.Flip(Vector2.left);
            _flipped = true;
        }

        if (input.vector == Vector2.right && !_flipped)
        {
            _flipper.Flip(Vector2.right);
            _flipped = true;
        }

        //print(input.vector);
        transform.position += (Vector3)input.vector * Time.deltaTime * _moveSpeed;
    }

    public void VecMovementCancled(Values _)
    {
        _flipped = false;
    }
}
