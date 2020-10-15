using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBallPos : MonoBehaviour
{
    private bool drawArc = true;
    private int segmentCount = 60;
    private float predictionTime = 6.0F;
    [SerializeField, Tooltip("放物線のマテリアル")]
    private Material arcMaterial;
    [SerializeField, Tooltip("放物線の幅")]
    private float arcWidth = 0.10F;
    private LineRenderer[] lineRenderers;

    private Vector3 initialVelocity;
    private Vector3 arcStartPosition;
    [SerializeField, Tooltip("着弾地点に表示するマーカーのPrefab")]
    private GameObject pointerPrefab;

    private GameObject pointerObject;


    void Start()
    {
        // 放物線のLineRendererオブジェクトを用意
        CreateLineRendererObjects();

        // マーカーのオブジェクトを用意
        pointerObject = Instantiate(pointerPrefab, Vector3.zero, Quaternion.identity);
        pointerObject.SetActive(false);

    }

    public void DrawPass(Vector3 velocity, Vector3 pos)
    {
        // 初速度と放物線の開始座標を更新
        initialVelocity = velocity;
        arcStartPosition = pos;

        if (drawArc)
        {
            // 放物線を表示
            float timeStep = predictionTime / segmentCount;
            bool draw = false;
            float hitTime = float.MaxValue;
            for (int i = 0; i < segmentCount; i++)
            {
                // 線の座標を更新
                float startTime = timeStep * i;
                float endTime = startTime + timeStep;
                SetLineRendererPosition(i, startTime, endTime, !draw);

                // 衝突判定
                if (!draw)
                {
                    hitTime = GetArcHitTime(startTime, endTime);
                    if (hitTime != float.MaxValue)
                    {
                        draw = true; // 衝突したらその先の放物線は表示しない
                    }
                }
            }

            // マーカーの表示
            if (hitTime != float.MaxValue)
            {
                Vector3 hitPosition = GetArcPositionAtTime(hitTime);
                ShowPointer(hitPosition);

                pointerObject.transform.localScale = new Vector3(
                    1.0f * pos.y,
                    pointerObject.transform.localScale.y,
                    1.0f * pos.y
                    );
            }
        }
        else
        {
            // 放物線とマーカーを表示しない
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].enabled = false;
            }
            pointerObject.SetActive(false);
        }
    }


    private Vector3 GetArcPositionAtTime(float time)
    {
        return (arcStartPosition + ((initialVelocity * time) + (0.5f * time * time) * Physics.gravity));
    }


    private void SetLineRendererPosition(int index, float startTime, float endTime, bool draw = true)
    {
        lineRenderers[index].SetPosition(0, GetArcPositionAtTime(startTime));
        lineRenderers[index].SetPosition(1, GetArcPositionAtTime(endTime));
        lineRenderers[index].enabled = draw;
    }



    private void CreateLineRendererObjects()
    {
        // 親オブジェクトを作り、LineRendererを持つ子オブジェクトを作る
        GameObject arcObjectsParent = new GameObject("ArcObject");

        lineRenderers = new LineRenderer[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newObject = new GameObject("LineRenderer_" + i);
            newObject.transform.SetParent(arcObjectsParent.transform);
            lineRenderers[i] = newObject.AddComponent<LineRenderer>();

            // 光源関連を使用しない
            lineRenderers[i].receiveShadows = false;
            lineRenderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            lineRenderers[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lineRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // 線の幅とマテリアル
            lineRenderers[i].material = arcMaterial;
            lineRenderers[i].startWidth = arcWidth;
            lineRenderers[i].endWidth = arcWidth;
            lineRenderers[i].numCapVertices = 5;
            lineRenderers[i].enabled = false;
        }
    }



    private void ShowPointer(Vector3 position)
    {
        pointerObject.transform.position = position;
        pointerObject.SetActive(true);
    }



    private float GetArcHitTime(float startTime, float endTime)
    {
        // Linecastする線分の始終点の座標
        Vector3 startPosition = GetArcPositionAtTime(startTime);
        Vector3 endPosition = GetArcPositionAtTime(endTime);

        // 衝突判定
        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            // 衝突したColliderまでの距離から実際の衝突時間を算出
            float distance = Vector3.Distance(startPosition, endPosition);
            return startTime + (endTime - startTime) * (hitInfo.distance / distance);
        }
        return float.MaxValue;
    }

    public void OnSuiside()
    {
        Destroy(pointerObject);
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            Destroy(lineRenderers[i]);
        }
    }
}
