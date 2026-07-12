using UnityEngine;
using UnityEngine.UI; // スライダーを使うための合言葉

public class SettingsManager : MonoBehaviour
{
    [Header("設定画面のパネル")] // ★追加：パネルを登録する枠
    public GameObject settingsPanel;

    [Header("BGM用のスライダー")]
    public Slider bgmSlider;

    [Header("マイク感度用のスライダー")]
    public Slider micSlider;

    [Header("タイトルのAudioSource（もしあれば）")]
    public AudioSource titleAudioSource;

    void Start()
    {
        // ★追加：ゲーム開始時は設定パネルを隠しておく
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // 過去に保存された設定があれば読み込み、なければ「0.5（真ん中）」にする
        float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float savedMic = PlayerPrefs.GetFloat("MicSensitivity", 0.5f);

        // スライダーのつまみの位置を保存された値に合わせる
        if (bgmSlider != null) bgmSlider.value = savedBGM;
        if (micSlider != null) micSlider.value = savedMic;

        // タイトルBGMの音量を即座に適用
        if (titleAudioSource != null) titleAudioSource.volume = savedBGM;

        // スライダーが動いたときに実行する関数を登録する
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        if (micSlider != null) micSlider.onValueChanged.AddListener(OnMicChanged);
    }

    // ★追加：設定画面を開く処理（OPTIONSボタン用）
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    // ★追加：設定画面を閉じる処理（CLOSEボタン用）
    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // BGMスライダーが動いたときに呼ばれる処理
    void OnBGMChanged(float value)
    {
        if (titleAudioSource != null)
        {
            titleAudioSource.volume = value; // タイトル曲の音量をリアルタイムに変更
        }
        PlayerPrefs.SetFloat("BGMVolume", value); // 値をセーブ
    }

    // マイク感度スライダーが動いたときに呼ばれる処理
    void OnMicChanged(float value)
    {
        PlayerPrefs.SetFloat("MicSensitivity", value); // 値をセーブ
    }
}