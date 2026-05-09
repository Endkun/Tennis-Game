using UnityEngine;
public class TennisAI_XZ : MonoBehaviour
{
    [Header("参照設定")]
    public Rigidbody ballRb;

    [Header("移動速度")]
    public float moveSpeed = 8f;

    [Header("左右の制限 (X軸)")]
    public float courtWidth = 8f;

    [Header("前後の制限 (Z軸)")]
    public float courtDepthMin = 15f; // ネット側の限界
    public float courtDepthMax = 5f; // 後ろ側の限界
    public float defaultZ = 2f;      // 待機位置

    void Update()
    {
        if (ballRb == null) return;

        // 1. 【ターゲット位置を決める】
        float targetX;
        float targetZ;

        // ボールが自分に向かってきている場合（velocity.z の正負はコートの向きに合わせて調整してください）
        if (ballRb.velocity.z > 0)
        {
            // ボールの位置を追いかける
            targetX = ballRb.position.x;
            targetZ = ballRb.position.z;
        }
        else
        {
            // ボールが離れていく時はホームポジションに戻る
            targetX = 0f;
            targetZ = defaultZ;
        }

        // 2. 【移動範囲の制限】
        targetX = Mathf.Clamp(targetX, -courtWidth, courtWidth);
        targetZ = Mathf.Clamp(targetZ, courtDepthMin, courtDepthMax);

        // 3. 【移動の計算】
        float step = moveSpeed * Time.deltaTime;

        float newX = Mathf.MoveTowards(transform.position.x, targetX, step);
        float newZ = Mathf.MoveTowards(transform.position.z, targetZ, step);

        // 4. 【座標の更新】
        // Y軸は現在の高さを維持
        transform.position = new Vector3(newX, transform.position.y, newZ);
    }
}