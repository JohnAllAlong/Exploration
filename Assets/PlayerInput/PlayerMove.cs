using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;

public class PlayerMove : MonoBehaviour
{
    public void KeyboardMove(Values input)
    {
        print(input.vector);
        transform.position += (Vector3)input.vector / 20;
    }

    public void GamepadMove(Values input)
    {
        print(input.vector);
        transform.position += (Vector3)input.vector / 20;
    }
}
