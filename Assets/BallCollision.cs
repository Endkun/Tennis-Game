using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private ScoreManager scoreManager;

    private string lastHitter = "";
    private string lastBoundedCourt = "";
    private int bounceCount = 0;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        ResetBall();
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleBallHit(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        HandleBallHit(other.gameObject);
    }

    void HandleBallHit(GameObject hitObject)
    {
        string targetName = hitObject.name;

        // ---------------------------------------------------
        // 1. ラケット（Player / NPC）に当たった時の処理
        // ---------------------------------------------------
        if (targetName.Contains("Player") || hitObject.CompareTag("Player"))
        {
            lastHitter = "Player";
            lastBoundedCourt = "";
            bounceCount = 0;
            return;
        }
        if (targetName.Contains("NPC"))
        {
            lastHitter = "NPC";
            lastBoundedCourt = "";
            bounceCount = 0;
            return;
        }

        // ---------------------------------------------------
        // 2. コート内（PCourt / TCourt）にバウンドした時の処理
        // ---------------------------------------------------
        if (targetName == "PCourt")
        {
            if (lastBoundedCourt == "PCourt") bounceCount++;
            else { lastBoundedCourt = "PCourt"; bounceCount = 1; }

            if (bounceCount >= 2)
            {
                scoreManager.AddPoint(false);
                ResetBall();
            }
            return;
        }

        if (targetName == "TCourt")
        {
            if (lastBoundedCourt == "TCourt") bounceCount++;
            else { lastBoundedCourt = "TCourt"; bounceCount = 1; }

            if (bounceCount >= 2)
            {
                scoreManager.AddPoint(true);
                ResetBall();
            }
            return;
        }

        // ---------------------------------------------------
        // 3. コート外（POutcourt / TOutcourt）に落ちた時の処理
        // ---------------------------------------------------
        if (targetName == "POutcourt" || targetName == "TOutcourt")
        {
            if (lastBoundedCourt == "")
            {
                if (lastHitter == "Player")
                    scoreManager.AddPoint(false);
                else if (lastHitter == "NPC")
                    scoreManager.AddPoint(true);
                else
                    ResetBall();
            }
            else
            {
                if (lastBoundedCourt == "TCourt")
                    scoreManager.AddPoint(true);
                else if (lastBoundedCourt == "PCourt")
                    scoreManager.AddPoint(false);
            }

            ResetBall();
        }
    }

    void ResetBall()
    {
        ResetBallState();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ★変更：サーブ権に応じてボールの初期位置を切り替え
        if (scoreManager.isPlayerServing)
        {
            // プレイヤーサーブ：プレイヤー側（手前）にセット
            transform.position = new Vector3(0, 2.5f, -8);
        }
        else
        {
            // NPCサーブ：NPC側（奥）にセット
            transform.position = new Vector3(0, 2f, 8);
        }
    }

    void ResetBallState()
    {
        lastHitter = "";
        lastBoundedCourt = "";
        bounceCount = 0;
    }
}