using UnityEngine;
using CustomInput.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private PlayerAnimation _playerAnimation;
    
    public void VecKeyboardMove(Values input)
    {
        //print(input.vector);
        _rb.velocity = input.vector * _moveSpeed;
        _playerAnimation.SetState(1);
    }

    public void VecGamepadMove(Values input)
    {
        _rb.velocity = input.vector * _moveSpeed;
        _playerAnimation.SetState(1);
    }

    public void VecMovementCancled(Values _)
    {
        _rb.velocity = Vector2.zero;
        _playerAnimation.SetState(0);
    }
}
