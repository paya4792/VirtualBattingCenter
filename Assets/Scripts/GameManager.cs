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
    public float dx, dy = 0.0f;

    [Header("投球間隔")]
    public float pitching_interval = 4.0f;

    [Header("投球時サウンド")]
    public AudioClip sound_on_pitch;

    // ボールの実体配列
    [System.NonSerialized]
    public GameObject[] balls;

    // ボールのリジッドボディ配列
    [System.NonSerialized]
    public Rigidbody[] ballRigs;

    // ボールのコライダー配列
    [System.NonSerialized]
    public SphereCollider[] ballCols;

    // ボールの変化球スクリプト配列
    [System.NonSerialized]
    public VelocityAdderForBall[] vafbs;

    // ボールの最大数
    public int ball_amount = 60;

    // 現在使用しているボール
    public int current_ball_num = 0;

    // 投球タイマー
    private float timer_for_pitch = 0.0f;

    private float time = 0.0f;

    // 初期化
    private void Start()
    {
        // 投球位置の角度取得
        Vector3 localAngle = throwPoint.localEulerAngles;

        // ボールの実体配列の初期化
        balls = new GameObject[ball_amount];
        ballRigs = new Rigidbody[ball_amount];
        ballCols = new SphereCollider[ball_amount];
        vafbs = new VelocityAdderForBall[ball_amount];


        for (int i = 0; i < ball_amount; i++)
        {
            // ボールの実体を生成
            balls[i] = Instantiate(ball_prefab, throwPoint.position, Quaternion.identity);

            // ボールに加速度を与えるスクリプトを設定し取得
            balls[i].AddComponent<VelocityAdderForBall>();
            vafbs[i] = balls[i].GetComponent<VelocityAdderForBall>();

            // ボールのリジッドボディを取得
            ballRigs[i] = balls[i].GetComponent<Rigidbody>();

            // ボールのコライダーを取得
            ballCols[i] = balls[i].GetComponent<SphereCollider>();

            SetBallInactive(i);
        }
    }

    // 毎フレーム実行
    private void Update()
    {

        timer_for_pitch += Time.deltaTime;
        if (timer_for_pitch >= pitching_interval)
        {
            timer_for_pitch = 0.0f;
            PitchBall();
        }
    }

    public void SetBallActive(int num)
    {
        if (!balls[num].activeSelf)
        {
             // ボールを有効化しておく
             ballRigs[num].isKinematic = false;
             ballCols[num].enabled = true;
             balls[num].SetActive(true);
        }
    }

    public void SetBallInactive(int num)
    {
        if (balls[num].activeSelf)
        {
            // ボールを無効化しておく
            ballRigs[num].isKinematic = true;
            ballCols[num].enabled = false;
            balls[num].SetActive(false);
        }
    }

    // 投球
    public void PitchBall()
    {
        SetBallActive(current_ball_num);

        balls[current_ball_num].transform.position = throwPoint.position;
        ballRigs[current_ball_num].velocity = Vector3.zero;
        
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
            "設定角度:" + theta * Mathf.Rad2Deg +
            "投球中ボール" + current_ball_num
            ) ;

        // 投げる力の設定
        float throwPower = dist_upPos / arrivalTime;
        Vector3 force = throwPoint.forward * throwPower * ballRigs[current_ball_num].mass;

        // ボールの変化球の設定
        vafbs[current_ball_num].SetRigidbody(ballRigs[current_ball_num]);
        vafbs[current_ball_num].SetSphereCollider(ballCols[current_ball_num]);
        vafbs[current_ball_num]._vx = dx;
        vafbs[current_ball_num]._vy = dy;
        vafbs[current_ball_num].SetVelocity(arrivalTime);

        // ボールを投げる
        ballRigs[current_ball_num].AddForce(force, ForceMode.Impulse);

        // 投球サウンドを鳴らす
        AudioSource.PlayClipAtPoint(sound_on_pitch, transform.position);

        // 現在のボール
        if (current_ball_num < ball_amount)
        {
            current_ball_num++;

            if (current_ball_num == ball_amount)
            {
                current_ball_num = 0;
            }
            SetBallInactive(ball_amount - current_ball_num);
        }
        
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
