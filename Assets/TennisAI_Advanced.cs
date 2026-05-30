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
    public float courtDepthMin = 15f;
    public float courtDepthMax = 5f;
    public float defaultZ = 12f;

    [Header("定位置")]
    public Vector3 homePosition = new Vector3(0f, 1.903f, 12f);

    public bool canMove = true;

    void Update()
    {
        if (!canMove) return;

        if (ballRb == null) return;

        // ★ボールが止まっている（リセット直後）は定位置に戻る
        if (ballRb.velocity.magnitude < 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;
            float newX = Mathf.MoveTowards(transform.position.x, homePosition.x, step);
            float newZ = Mathf.MoveTowards(transform.position.z, homePosition.z, step);
            transform.position = new Vector3(newX, transform.position.y, newZ);
            return;
        }

        float targetX;
        float targetZ;

        if (ballRb.velocity.z > 0)
        {
            targetX = ballRb.position.x;
            targetZ = ballRb.position.z;
        }
        else
        {
            targetX = 0f;
            targetZ = defaultZ;
        }

        targetX = Mathf.Clamp(targetX, -courtWidth, courtWidth);
        targetZ = Mathf.Clamp(targetZ, courtDepthMin, courtDepthMax);

        float stepMove = moveSpeed * Time.deltaTime;
        float newMoveX = Mathf.MoveTowards(transform.position.x, targetX, stepMove);
        float newMoveZ = Mathf.MoveTowards(transform.position.z, targetZ, stepMove);

        transform.position = new Vector3(newMoveX, transform.position.y, newMoveZ);
    }
}