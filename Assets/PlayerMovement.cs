using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動スピード")]
    public float moveSpeed = 5f;

    void Update()
    {
        // キーボードの入力を受け取る（横: A/D or ←/→ , 縦: W/S or ↑/↓）
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        // 移動する方向を計算する
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
    
        // 実際の移動処理（現在の位置 + 動かしたい方向 * スピード * 時間）
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
