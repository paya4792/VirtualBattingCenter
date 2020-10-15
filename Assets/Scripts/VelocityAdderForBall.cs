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

    [Header("ボールのリグ")]
    public Rigidbody _rigidbody;

    [Header("ボールのコライダー")]
    public SphereCollider _sphereCollider;

    // 変化球
    private Vector3 _velocity;

    // ボールが投げられてからの時間
    private float _timer = 0.0f;

    // ボールの到達時間
    private float _arrivalTime = 0.0f;

    // ボールがなにかに衝突したか
    private bool _collided = false;

    void Update()
    {
        _timer += Time.deltaTime * 1.0f;

        if (_timer >= 20.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //SetVelocity();
    }

    private void FixedUpdate()
    {
        if (!_collided)
        {
            AddForce();
        }

    }

    private void AddForce()
    {
        _rigidbody.AddForce(_velocity * Mathf.Clamp((_arrivalTime / _timer), 0.0f, 1.0f), ForceMode.Acceleration);
    }

    private void SetVelocity()
    {
        _rigidbody.AddForce(_velocity * Mathf.Clamp((_arrivalTime / _timer), 0.0f, 1.0f), ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collided = true;
    }

    public void SetVelocity(float arrivalTime)
    {
        _arrivalTime = arrivalTime;
        _velocity.x = _vx;
        _velocity.y = _vy;
    }
}
