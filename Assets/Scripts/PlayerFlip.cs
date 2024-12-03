using CustomInput.Events;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _player;
    [SerializeField] private Vector2 _currentDirection = Vector2.left;

    public void OnceAxisLeftRight(ReturnData input)
    {
        //if movement is paused then we dont flip
        //if the list returned contains true anywhere, that means one of the movement events is paused
        if (Events.IsPaused("KeyboardMove", "GamepadMove").Contains(true)) return;

        if (input.axis == Vector2.left && input.axis != _currentDirection)
        {
            Flip(Vector2.left);
        }

        if (input.axis == Vector2.right && input.axis != _currentDirection)
        {
            Flip(Vector2.right);
        }

        _currentDirection = input.axis;
    }





    /// <summary>
    /// Flips the player and all local transforms attached to PlayerFlip
    /// </summary>
    /// <param name="direction">direction to flip towards (X Axis ONLY)</param>
    public void Flip(Vector2 direction)
    {
        for (int i = 0; i != gameObject.transform.childCount; i++)
        {
            Transform flipMe = gameObject.transform.GetChild(i);
            if (direction == Vector2.left)
            {
                    flipMe.localPosition = new Vector2(-flipMe.localPosition.x, flipMe.localPosition.y);
                _player.flipX = false;
            }
            if (direction == Vector2.right)
            {
                    flipMe.localPosition = new Vector2(-flipMe.localPosition.x, flipMe.localPosition.y);
                _player.flipX = true;
            }
        }
    }
}
