using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int playerPoints = 0;
    public int npcPoints = 0;

    public int playerSets = 0;
    public int npcSets = 0;

    public bool isPlayerServing = true;

    public TextMeshProUGUI scoreText;

    private bool isGameOver = false;

    // ★追加：PlayerとNPCのTransformをInspectorからアタッチ
    public Transform playerTransform;
    public Transform npcTransform;

    // ★追加：各自の定位置
    public Vector3 playerHomePosition = new Vector3(0, 1.903f, -12f);
    public Vector3 npcHomePosition = new Vector3(0, 1.903f, 12f);

    void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddPoint(bool isPlayer)
    {
        if (isGameOver) return;

        if (isPlayer) playerPoints++;
        else npcPoints++;

        // ★追加：ポイントが入るたびに定位置へ戻す
        ResetPositions();

        CheckGameWin();
        UpdateScoreDisplay();
    }

    // ★追加：PlayerとNPCを定位置に戻すメソッド
    public void ResetPositions()
    {
        if (playerTransform != null)
            playerTransform.position = playerHomePosition;

        if (npcTransform != null)
            npcTransform.position = npcHomePosition;
    }

    void CheckGameWin()
    {
        if (playerPoints >= 3 && npcPoints >= 3)
        {
            if (playerPoints >= npcPoints + 2)
            {
                playerSets++;
                isPlayerServing = !isPlayerServing;
                ResetPoints();
                CheckMatchWin();
            }
            else if (npcPoints >= playerPoints + 2)
            {
                npcSets++;
                isPlayerServing = !isPlayerServing;
                ResetPoints();
                CheckMatchWin();
            }
        }
        else
        {
            if (playerPoints >= 4)
            {
                playerSets++;
                isPlayerServing = !isPlayerServing;
                ResetPoints();
                CheckMatchWin();
            }
            else if (npcPoints >= 4)
            {
                npcSets++;
                isPlayerServing = !isPlayerServing;
                ResetPoints();
                CheckMatchWin();
            }
        }
    }

    void ResetPoints()
    {
        playerPoints = 0;
        npcPoints = 0;
    }

    void UpdateScoreDisplay()
    {
        if (isGameOver) return;

        string pScore = "";
        string nScore = "";

        if (playerPoints >= 3 && npcPoints >= 3 && playerPoints == npcPoints)
        {
            pScore = "Deuce";
            nScore = "Deuce";
        }
        else if (playerPoints >= 3 && npcPoints >= 3 && playerPoints > npcPoints)
        {
            pScore = "Advantage";
            nScore = "40";
        }
        else if (playerPoints >= 3 && npcPoints >= 3 && npcPoints > playerPoints)
        {
            pScore = "40";
            nScore = "Advantage";
        }
        else
        {
            pScore = GetTennisScore(playerPoints);
            nScore = GetTennisScore(npcPoints);
        }

        string playerServeIndicator = isPlayerServing ? "●" : " ";
        string npcServeIndicator = isPlayerServing ? " " : "●";

        scoreText.text = $"{playerServeIndicator} {playerSets} PLAYER {pScore} - {nScore} NPC {npcSets} {npcServeIndicator}";
    }

    string GetTennisScore(int points)
    {
        switch (points)
        {
            case 0: return "0";
            case 1: return "15";
            case 2: return "30";
            case 3: return "40";
            default: return "0";
        }
    }

    void CheckMatchWin()
    {
        if (playerSets >= 4)
        {
            isGameOver = true;
            scoreText.text = $"{playerSets} PLAYER WIN! - GAME OVER";
        }
        else if (npcSets >= 4)
        {
            isGameOver = true;
            scoreText.text = $"GAME OVER - {npcSets} NPC WIN!";
        }
    }
}