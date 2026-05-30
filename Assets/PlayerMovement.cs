using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動スピード")]
    public float moveSpeed = 5f;

    [Header("移動制限")]
    public float minZ = -2.2f;   // ネット側の限界
    public float maxZ = -15f;    // 後ろ側の限界
    public float minX = -8f;     // 左の限界
    public float maxX = 8f;      // 右の限界

    // ★追加：入力を受け付けるかどうかのフラグ
    public bool canMove = true;

    void Update()
    {
        // ★追加：canMoveがfalseの時は操作を受け付けない
        if (!canMove) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 移動範囲の制限
        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z, maxZ, minZ);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}