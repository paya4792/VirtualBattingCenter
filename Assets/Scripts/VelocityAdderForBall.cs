using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityAdderForBall : MonoBehaviour
{
    // 横変化量
    [System.NonSerialized]
    public float _vx;

    // 縦変化量
    [System.NonSerialized]
    public float _vy;

    // リジッドボディ
    private Rigidbody _rigidbody;

    // コライダー
    private SphereCollider _sphereCollider;

    // 変化
    private Vector3 _velocity;

    // ボールが投げられてからの時間
    private float _timer = 0.0f;

    // ボールの到達時間
    private float _arrivalTime = 0.0f;

    // ボールが投げられたか
    private bool _pitched = false;

    // ボールがなにかに衝突したか
    private bool _collided = false;

    public void SetRigidbody(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public void SetSphereCollider(SphereCollider sphereCollider)
    {
        _sphereCollider = sphereCollider;
    }

    public void SetVelocity(float arrivalTime)
    {
        _arrivalTime = arrivalTime;
        _velocity.x = _vx;
        _velocity.y = _vy;
        _pitched = true;
    }


    void Update()
    {
        _timer += Time.deltaTime * 1.0f;
    }

    private void Start()
    {
        //SetVelocity();
    }

    private void FixedUpdate()
    {
        if (!_collided && _pitched)
        {
            AddForce();
        }

    }

    private void AddForce()
    {
        _rigidbody.AddForce(_velocity * Mathf.Clamp((_arrivalTime / _timer), 0.0f, 1.0f), ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collided = true;
    }
}
