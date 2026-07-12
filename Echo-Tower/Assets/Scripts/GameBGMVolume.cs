using UnityEngine;

public class BGMVolumeApply : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            // タイトル画面で設定・保存された音量を読み込む（なければ0.5）
            float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            audioSource.volume = savedVolume;
        }
    }
}