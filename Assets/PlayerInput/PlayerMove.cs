using UnityEngine;
using CustomInput.Events;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;

    public void VecKeyboardMove(Values input)
    {
        //print(input.vector);
        transform.position += _moveSpeed * Time.deltaTime * (Vector3)input.vector;
    }

    public void VecGamepadMove(Values input)
    {
        //print(input.vector);
        transform.position += _moveSpeed * Time.deltaTime * (Vector3)input.vector;
    }
}
