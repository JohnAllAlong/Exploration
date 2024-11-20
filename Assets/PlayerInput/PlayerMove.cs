using UnityEngine;
using CustomInput.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;

    public void VecKeyboardMove(Values input)
    {
        //print(input.vector);
        _rb.velocity = input.vector * _moveSpeed;
    }

    public void VecGamepadMove(Values input)
    {
        _rb.velocity = input.vector * _moveSpeed;
    }

    public void VecMovementCancled(Values _)
    {
        _rb.velocity = Vector2.zero;
    }
}
