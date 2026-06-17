using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceInputTest : MonoBehaviour
{
    [Header("動かす対象のCube")]
    public Transform targetCube;

    [Header("動きの調整設定")]
    public float volumeSensitivity = 50f;
    public float pitchSensitivity = 0.5f;
    public float neutralPitchIndex = 8f;

    [Header("波の厚み（平べったさ）")]
    public float waveThickness = 0.1f; //

    [Header("ノイズ対策（この数値以下の雑音は無視）")]
    public float minVolumeThreshold = 0.01f; 

    private AudioSource audioSource;
    private string micName;
    private float currentTargetHeight = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            audioSource.clip = Microphone.Start(micName, true, 10, 44100);
            audioSource.loop = true;

            while (!(Microphone.GetPosition(micName) > 0)) { }
            audioSource.Play();
        }
        else
        {
            Debug.LogError("マイクが見つかりません！");
        }
    }

    void Update()
    {
        if (targetCube == null) return;

        float volume = GetVolume();

        // 声を出している（しきい値以上の音量がある）時だけ高さを計算
        if (volume > minVolumeThreshold)
        {
            float pitchIndex = GetPitch();

            // ピッチが正しく検出された場合のみ高さを更新
            if (pitchIndex > 0)
            {
                currentTargetHeight = (pitchIndex - neutralPitchIndex) * pitchSensitivity;
            }
        }
        else
        {
            // 声を出していない時は、ゆっくり真ん中(Y=0)に戻る
            currentTargetHeight = 0f;
        }

        // 大きさの反映
        float targetScale = 1f + (volume * volumeSensitivity);
        targetCube.localScale = Vector3.Lerp(targetCube.localScale, new Vector3(targetScale, waveThickness, targetScale), Time.deltaTime * 15f);

        // 高さの反映
        Vector3 newPos = new Vector3(0, currentTargetHeight, 0);
        targetCube.position = Vector3.Lerp(targetCube.position, newPos, Time.deltaTime * 10f);
    }

    float GetVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audioSource.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256f;
    }

    float GetPitch()
    {
        float[] spectrum = new float[1024];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        float maxV = 0;
        int maxN = -1; // 検出できなかったら-1を返す

        // PCのファン音とかのノイズを無視するため、i=3 から判定をスタート
        for (int i = 3; i < 1024; i++)
        {
            if (spectrum[i] > maxV && spectrum[i] > 0.005f)
            {
                maxV = spectrum[i];
                maxN = i;
            }
        }
        return maxN;
    }
}