using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;
using CustomInput;

public class GoGoGadgetGun : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletSpeed;
    private Vector2 _GamepadBulletDir;
    public void OnceBtnMouseFire(Values input)
    {
        // Get the mouse screen position and convert it to a world position
        Vector2 mousePos = Devices.GetMouse().position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        // rotate bullet to face direction of mouse
        bullet.transform.up = -((Vector2)transform.position - mousePos);

        // make bullet g o
        bulletRB.AddRelativeForce(bullet.transform.up * _bulletSpeed);

        //bulletRB.AddForce(-((Vector2)transform.position - mousePos) * _bulletSpeed, ForceMode2D.Impulse);

        
    }

    public void VecGamepadAim(Values input)
    {
        _GamepadBulletDir = -((Vector2)transform.position - input.vector);
    }

    public void OnceBtnGamepadFire(Values input)
    { 

    }
}
