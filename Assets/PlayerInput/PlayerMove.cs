using UnityEngine;
using CustomInput.Events;
using UnityEngine.Rendering.Universal;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private PlayerAnimation _playerAnimation;
    
    public void AxisKeyboardMove(ReturnData input)
    {
        //print(input.vector);
        _rb.velocity = input.axis * _moveSpeed;
        _playerAnimation.SetState(1);
    }

    public void AxisGamepadMove(ReturnData input)
    {
        _rb.velocity = input.axis * _moveSpeed;
        _playerAnimation.SetState(1);
    }

    public void AxisMovementCancled(ReturnData _)
    {
        _rb.velocity = Vector2.zero;
        _playerAnimation.SetState(0);
    }
}
