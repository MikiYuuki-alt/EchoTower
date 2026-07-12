using UnityEngine;
using TMPro; // TextMeshProを使うための合言葉

public class ComboManager : MonoBehaviour
{
    // ★どこからでもアクセスできるようにする魔法（シングルトン）
    public static ComboManager instance;

    [Header("コンボを表示するテキストUI")]
    public TextMeshProUGUI comboText;

    [Header("コンボがリセットされるまでの時間(秒)")]
    public float resetTime = 2.0f;

    private int comboCount = 0; // 現在のコンボ数
    private float resetTimer = 0f; // リセットまでのタイマー

    void Awake()
    {
        // シングルトンの初期設定
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        UpdateUIText(); // ゲーム開始時は表示をリセット
    }

    void Update()
    {
        // コンボが繋がっている間は、タイマーを減らす
        if (comboCount > 0)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= resetTime)
            {
                ResetCombo(); // 時間が経ったらコンボをリセット
            }
        }
    }

    // ★敵を倒したときに、他のスクリプトから呼ぶ関数
    public void AddCombo()
    {
        comboCount += 1; // コンボをプラス
        resetTimer = 0f; // タイマーをリセットして、またコンボを繋げられるようにする
        UpdateUIText();
    }

    // コンボをリセットする関数
    public void ResetCombo()
    {
        comboCount = 0;
        UpdateUIText();
    }

    // 通常時のHPテキスト更新
    void UpdateUIText()
    {
        if (comboText != null)
        {
            if (comboCount > 0)
            {
                comboText.text = comboCount + " COMBO!";
            }
            else
            {
                comboText.text = ""; // コンボが0の時は文字を消す
            }
        }
    }
}