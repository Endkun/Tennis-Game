using UnityEngine;

// 判定結果のタイプ
public enum PointResult
{
    None,       // まだ判定なし（プレイ中）
    InBounds,   // イン（プレイ継続）
    OutOfBounds, // アウト（相手のポイント）
    Net,        // ネット（相手のポイント）
    Fault       // その他のミス（自陣落下など）
}

public class PlayerHantei : MonoBehaviour
{
    // 最後に打ったのは誰かを管理する
    public enum LastHitter { Player, Enemy, None }
    public LastHitter lastHitter = LastHitter.None;

    // ゲームマネージャー（もしあれば）への参照（ポイント加算用、ここではプレースホルダー）
    // public GameManager gameManager; 

    // 初期位置（リセット用）
    private Vector3 initialPosition;
    private Rigidbody rb;

    void Start()
    {
        initialPosition = transform.position; // 最初の位置を記憶
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("ボールに Rigidbody がアタッチされていません！");
        }
    }

    // 衝突検知（ボールが何かに当たった時）
// 衝突検知（ボールが何かに当たった時）
    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        // --- 1. ラケットの打球判定 ---
        if (tag == "RacketPlayer") 
        {
            lastHitter = LastHitter.Player;
            Debug.Log(">>> PLAYER shot the ball! >>>");
            return; 
        }
        else if (tag == "RacketEnemy") 
        {
            lastHitter = LastHitter.Enemy;
            Debug.Log(">>> ENEMY shot the ball! >>>");
            return;
        }

        // --- 2. コート判定（イン/アウト） ---
        string scorer = "";

        // **プレイヤーが最後に打った場合**
        if (lastHitter == LastHitter.Player) 
        {
            if (tag == "TCourt") // 敵コート（赤）：イン
            {
                HandleInBounds("ENEMY (Right)");
            }
            else if (tag == "TOutCourt") // 敵アウト（黒）：アウト
            {
                scorer = "Enemy";
                HandleOutOfBounds("ENEMY (Black)", scorer);
            }
            else if (tag == "PCourt" || tag == "POutCourt") // 自陣（黄色/水色）：ミス
            {
                scorer = "Enemy";
                HandleOutOfBounds("PLAYER (own side)", scorer);
            }
            else if (tag == "Net") // ネット
            {
                scorer = "Enemy";
                HandleOutOfBounds("NET", scorer);
            }
        }
        // **敵が最後に打った場合**
        else if (lastHitter == LastHitter.Enemy) 
        {
            if (tag == "PCourt") // プレイヤーコート（黄色）：イン
            {
                HandleInBounds("PLAYER (Left)");
            }
            else if (tag == "POutCourt") // プレイヤーアウト（水色）：アウト
            {
                scorer = "Player";
                HandleOutOfBounds("PLAYER (Cyan)", scorer);
            }
            else if (tag == "TCourt" || tag == "TOutCourt") // 自陣（赤/黒）：ミス
            {
                scorer = "Player";
                HandleOutOfBounds("ENEMY (own side)", scorer);
            }
            else if (tag == "Net") // ネット
            {
                scorer = "Player";
                HandleOutOfBounds("NET", scorer);
            }
        }
        else
        {
            if (tag == "PCourt" || tag == "TCourt" || tag == "POutCourt" || tag == "TOutCourt")
            {
                Debug.Log("Ball bounced (Hitter: NONE)");
            }
        }
    }
    // イン判定の補助関数（プレイ継続）
    void HandleInBounds(string targetSide)
    {
        Debug.Log($"<color=green>IN on {targetSide}'s side!</color>");
        // プレイ継続（次のヒッターの待機など）
        // 一度バウンドした、などのフラグを立てる必要があるが、
        // このシンプルなセットアップでは次のヒッターが NONE になるだけでプレイが続く。
        
        // lastHitter = LastHitter.None; // 次の打者を待つ。
    }

    // アウト判定の補助関数（ポイント加算、リセット）
    void HandleOutOfBounds(string faultLocation, string scorer)
    {
        string faultyPlayer = (scorer == "Enemy") ? "Player" : "Enemy";
        Debug.Log($"<color=red>OUT at {faultLocation}!</color> Fault by {faultyPlayer}. Scorer: {scorer}");
        
        // ポイント処理、リセット
        // GameManager.Instance.AddPoint(scorer); 

        // ボールをリセット（サーブ位置、など）
        ResetBall();
        
    }

    // ボールを中央リセット（簡易）
    void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition; // 最初の位置へ
        lastHitter = LastHitter.None; // ヒッターをリセット
        Debug.Log("<color=yellow>Game Reset</color>");
    }
}