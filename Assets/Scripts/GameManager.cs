using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("ボールを投げる位置")]
    [SerializeField]
    public Transform throwPoint;

    [Header("投げるボールのプレハブ")]
    public GameObject ball_prefab;

    // ボールの実体
    private GameObject ball;

    [Header("投げる目標位置")]
    public Transform target;

    [Header("球速")]
    public float speed = 130.0f;

    [Header("変化量")]
    public float dx, dy;


    private float time = 0.0f;

    // 初期化
    private void Start()
    {
        Vector3 localAngle = throwPoint.localEulerAngles;
    }

    // 毎フレーム実行
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBall();
        }
    } 

    // 投球
    public void ThrowBall()
    {
        // ボールの実体を生成
        ball = (GameObject)Instantiate(ball_prefab, throwPoint.position, Quaternion.identity);

        // ボールのスクリプトを取得
        VelocityAdderForBall vafb = ball.GetComponent<VelocityAdderForBall>();

        // ボールのリジッドボディを取得
        Rigidbody ballRig = ball.GetComponent<Rigidbody>();

        // 投球目標を設定
        var _target = transform.position;
        _target.x = target.position.x;
        _target.y = target.position.y;
        _target.z = target.position.z;


        // 距離の計算
        float distance = Vector3.Distance(throwPoint.position, _target);

        // 到達するまでの時間を計算
        float arrivalTime = distance / (speed / 3.6f);

        float _dx = dx * arrivalTime * arrivalTime * 0.5f;
        float _dy = dy * arrivalTime * arrivalTime * 0.5f;
    
        _target.y -= _dy;
        _target.x -= _dx;


        // 落下量の測定
        float drop_dist = Mathf.Abs(
            0.5f * 
            Physics.gravity.y * 
            Mathf.Pow(arrivalTime, 2));

        // 落下量を考慮した投球目標を計算
        Vector3 upPos = _target;
        

        // 落下量を考慮した投球目標との距離を計算
        float dist_upPos = Vector3.Distance(throwPoint.position, upPos);

        // 射出角度の計算
        float theta = -Mathf.Acos((Mathf.Pow(dist_upPos,2) + Mathf.Pow(distance,2) - Mathf.Pow(drop_dist, 2)) / (2 * dist_upPos * distance));

        // 角度を設定
        throwPoint.LookAt(upPos);
        throwPoint.Rotate(Vector3.right, theta * Mathf.Rad2Deg, Space.Self);

        Debug.Log(
            "落下量:" + drop_dist +
            "距離:" + distance +
            "到達時間:" + arrivalTime +
            "設定角度:" + theta * Mathf.Rad2Deg
            );

        // 投げる力の設定
        float throwPower = dist_upPos / arrivalTime;
        Vector3 force = throwPoint.forward * throwPower * ballRig.mass;

        vafb._vx = dx;
        vafb._vy = dy;
        vafb.SetVelocity(arrivalTime);

        ballRig.AddForce(force, ForceMode.Impulse);
    }
}
