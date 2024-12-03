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
    public void OnceBtnMouseFire(ReturnData input)
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
    }

    public void AxisGamepadAim(ReturnData input)
    {
        _GamepadBulletDir = input.axis;
    }

    public void OnceBtnGamepadFire(ReturnData input)
    {
        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        bullet.transform.up = _GamepadBulletDir;

        bulletRB.AddRelativeForce(bullet.transform.up * _bulletSpeed);
    }
}
