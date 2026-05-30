using UnityEngine;

// faceオブジェクト（面のメッシュ）に貼る
public class RacketFaceHit : MonoBehaviour
{
    [Header("打ち返しパワー")]
    public float hitPower = 18f;      // 弾き飛ばす強さ
    public float upwardAngle = 0.3f;  // ネットを越えさせるための上向き補正

    void OnCollisionEnter(Collision collision)
    {
        // ぶつかった相手がボールだったら
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                // 1. ボールの勢いを一度リセット（これで挙動が安定する）
                ballRb.velocity = Vector3.zero;

                // 2. 「面の正面方向」を取得
                // image_cf6cbd.jpg の向きに基づき、ワールド座標でのtransform.up（緑矢印方向）を使用
                Vector3 shotDirection = transform.up;

                // 3. 上向きに飛ばすための補正
                //shotDirection.y += upwardAngle;

                // 4. 正面方向へ力を加えて弾き飛ばす
                ballRb.AddForce(shotDirection.normalized * hitPower, ForceMode.Impulse);

                Debug.Log("フェイス（面）で綺麗に打ち返しました！");
            }
        }
    }
}