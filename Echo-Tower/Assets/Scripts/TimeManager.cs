using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
using System.Collections; 

public class TimeManager : MonoBehaviour
{
    [Header("制限時間(秒)")]
    public float timeLimit = 60f;

    [Header("タイマーを表示するテキストUI")]
    public TextMeshProUGUI timerText;

    [Header("クリア時に出すパネル")]
    public GameObject clearPanel; 

    private float remainingTime;
    private bool isGameOver = false;

    void Start()
    {
        remainingTime = timeLimit;

        // ゲーム開始時はパネルを必ず隠しておく
        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
        }

        UpdateUIText();
    }

    void Update()
    {
        if (isGameOver) return; // すでに終了していたら何もしない

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isGameOver = true;
            StartCoroutine(ClearRoutine()); // クリア演出をスタートする
        }

        UpdateUIText();
    }

    void UpdateUIText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime % 60F);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    // クリア演出からタイトルに戻る一連の流れ
    IEnumerator ClearRoutine()
    {
        // 1. 隠しておいた「CLEAR!」のパネルをドカンと表示する
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
        }

        // 2. そのまま3秒間、余韻を楽しむために待つ
        yield return new WaitForSeconds(3.0f);

        // 3. タイトル画面に戻る
        SceneManager.LoadScene("TitleScene");
    }
}