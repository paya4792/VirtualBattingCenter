using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ThrowBall : MonoBehaviour
{
    public GameObject Ball;
    public AudioClip sound;
    public float time;
    private float Timecount;
    public Text backScreenText;
    public Text countText;
    public Text distanceText;
    public Text ballSpeedText;
    public GameObject homebase;
    public bool run = false;
    private int num;
    private bool isHitted = false;
    private bool isGrounded = false;
    private GameObject ball;

    public float controll;
    public float stamina;

    private float stamina_cur;

    private GameObject target;
    private float totaTime;
    private float arrivalTime;
    private float theta;
    private float hypotenuse;
    private float hypotenuseSpeed;
    private float gravityY;
    private CatcherScript catcher;

    private GameObject strikezone;
    private StrikeScript strike;
    private BSOScript bso;

    public bool isRandom = false;

    public BallStruct[] Balls;

    [System.Serializable]
    public struct BallStruct
    {
        public string name;//球種
        public float speed;//球速
        public float dx;//横方向変化量
        public float dy;//縦方向変化量
        public float sharpness;//変化球キレ
        public float persent;//投球割合
    }



    private void Start()
    {
        Transform myTransform = this.transform;
        Vector3 localAngle = myTransform.localEulerAngles;
        backScreenText.text = " " + Balls[num].name + "";
        myTransform.localEulerAngles = localAngle;
        distanceText.text = 0 + "m";
        countText.text = ((int)(time - Timecount + 1)).ToString();
        ballSpeedText.text = "___km/h";

        target = GameObject.FindGameObjectWithTag("Catcher");
        catcher = target.GetComponent<CatcherScript>();

        strikezone = GameObject.Find("StrikeZone");
        strike = strikezone.GetComponent<StrikeScript>();
        bso = strikezone.GetComponent<BSOScript>();

        stamina_cur = stamina;
    }

    void Update()
    {
        if (run) {
            Running();
        }
        if (isHitted && !isGrounded)
        {
            Vector3 PosA = homebase.transform.position;
            Vector3 PosB = ball.transform.position;
            PosA.y = 0.0f;
            PosB.y = 0.0f;
            float dis = Vector3.Distance(PosA, PosB);
            distanceText.text = (int)dis + "m";
        }
    }

    public void OnPutMenuButton(string button) {
        if (isRandom) { backScreenText.text = "ランダム"; }
        switch (button) {
            case "start": run = true; break;
            case "stop": run = false; break;
            case "increase": ChangeSpeed(true); break;
            case "decrease": ChangeSpeed(false); break;
            case "random": isRandom = !isRandom; break;
            case "pos": catcher.isRandom = !catcher.isRandom; break;
            default: break;
        }
    }

    public void OnBallHitted()
    {
        isHitted = true;
        isGrounded = false;
        catcher.OnHitted();
    }

    public void OnBallGrounded()
    {
        if (isHitted)
        {
            isGrounded = true;
        }
    }

    public void OnBallFouled()
    {
        if (isHitted)
        {
            isHitted = false;
            catcher.OnHitted();
            bso.AddBSO('F');
        }
    }

    public void OnBallFared()
    {
        if (isHitted)
        {
            isHitted = false;
            catcher.OnHitted();
            bso.AddBSO('H');
        }
    }

    public void OnBallCatched()
    {
        isHitted = false;
        strike.OnBallCatched();
        strike.ResetBool();
        catcher.OnHitted();
    }

    public void ChangeSpeed(bool change) {
        if (change)
        {
            if (num < Balls.Length - 1)
            {
                num++;
            }
            else
            {
                num = 0;
            }
        }
        else if (!change) {
            if (num > 0)
            {
                num--;
            }
            else
            {
                num = Balls.Length - 1;
            }
        }
     backScreenText.text = " " + Balls[num].name + "";
    }

    public void Shot()
    {
        if (stamina_cur > 1.0f) { stamina_cur -= UnityEngine.Random.Range(0.01f,1.0f); }
        distanceText.text = 0 + "m";
        ball = (GameObject)Instantiate(Ball, transform.position, Quaternion.identity);

        //ボール読み込み
        BallScript ballScript = ball.GetComponent<BallScript>();
        Rigidbody ballRig = ball.GetComponent<Rigidbody>();

        //投球目標を設定
        var _target = transform.position;
        _target.x = target.transform.position.x - (UnityEngine.Random.Range(-100 + controll, 100 - controll) / 200);
        _target.y = target.transform.position.y - (UnityEngine.Random.Range(-100 + controll, 100 - controll) / 200);
        _target.z = target.transform.position.z;
        _target.y -= Balls[num].dy;
        _target.x -= Balls[num].dx;

        //距離
        float distance = Vector3.Distance(transform.position, _target);

        var speed = Balls[num].speed - UnityEngine.Random.Range(0, 10);

        //到達時間
        arrivalTime = distance / (speed / 3.6f);

        //球速表示
        StartCoroutine(DelayMethod(2.0f, () =>
        {
            ballSpeedText.text = Balls[num].name + "\n" + (int)speed + "km/h";

            StartCoroutine(DelayMethod(4.0f, () =>
            {
                ballSpeedText.text = "";
            }));

        }));

        //落下距離を測定
        float drop_dist = Mathf.Abs(0.5f * Physics.gravity.y * arrivalTime * arrivalTime);
        Vector3 upPos = _target + Vector3.up * drop_dist;
        float dist_upPos = Vector3.Distance(transform.position, upPos);

        theta = -Mathf.Acos((Mathf.Pow(dist_upPos,2) + Mathf.Pow(distance,2) - Mathf.Pow(drop_dist, 2)) / (2 * dist_upPos * distance));

        Debug.Log(theta * Mathf.Deg2Rad);

        //角度設定
        transform.LookAt(_target);
        transform.Rotate(Vector3.right, theta * Mathf.Rad2Deg, Space.Self);

        //投げる力の設定
        hypotenuseSpeed = dist_upPos / arrivalTime;
        Vector3 force = transform.forward * hypotenuseSpeed * ballRig.mass;

        ballScript.dx = Balls[num].dx;
        ballScript.dy = Balls[num].dy;
        ballScript.sharpness = Balls[num].sharpness;
        ballScript.SetVelocity(arrivalTime);

        ballRig.AddForce(force, ForceMode.Impulse);
        catcher.OnPitch();
    }

    public void Running() {
        Timecount += Time.deltaTime;
        countText.text = ((int)(time - Timecount + 1)).ToString();
        if (Timecount >= time)
        {
            Timecount = 0.0f;
            run = false;
            catcher.SetPosition();
            strike.isPitched = true;
            if (isRandom) { ChangeBall(); }
            Shot();
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }
    }

    public void ChangeBall()
    {
        float rnd = UnityEngine.Random.Range(0.0f,100.0f);
        float pers = 0.0f;
        int i;

        for (i = 0; i < Balls.Length; i++)
        {
            pers += Balls[i].persent;
            if (pers > rnd) { break; }
        }

        num = i;

        backScreenText.text = "ランダム";
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
