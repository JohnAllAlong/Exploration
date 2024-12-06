using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput.Events;
using CustomInput;
using Player;

public class GoGoGadgetGun : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private Timer _cooldownTimer;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private bool _playerHasGun;

    private Vector2 _GamepadBulletDir;


    protected void Start()
    {
        _cooldownTimer = new(_cooldownTime);
        _cooldownTimer.DestroyOnEnd(false);
    }

    public void OnceBtnMouseFire(ReturnData input)
    {
        if (_cooldownTimer.IsRunning() || !_playerHasGun) return;

        // Get the mouse screen position and convert it to a world position
        Vector2 mousePos = Devices.GetMouse().position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.transform.GetChild(0).GetComponent<Rigidbody2D>();

        // rotate bullet to face direction of mouse
        bullet.transform.up = -((Vector2)transform.position - mousePos);

        // make bullet g o
        bulletRB.AddRelativeForce(bullet.transform.up * _bulletSpeed);

        _cooldownTimer.StartTimer();

    }

    public void AxisGamepadAim(ReturnData input)
    {
        _GamepadBulletDir = input.axis;
    }

    public void OnceBtnGamepadFire(ReturnData input)
    {
        if (_cooldownTimer.IsRunning() || !_playerHasGun) return;


        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.transform.GetChild(0).GetComponent<Rigidbody2D>();

        bullet.transform.up = _GamepadBulletDir;

        bulletRB.AddRelativeForce(bullet.transform.up * _bulletSpeed);

        _cooldownTimer.StartTimer();

    }

    protected void Update()
    {
        if (PlayerData.GetCollectibleController().HasCollectable(1) && PlayerData.GetCollectibleController().CollectibleInHotbar(1))
        {
            _playerHasGun = true;
        } else
        {
            _playerHasGun = false;
        }
    }
}
