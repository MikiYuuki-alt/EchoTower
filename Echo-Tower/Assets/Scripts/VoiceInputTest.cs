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
    public float waveThickness = 0.1f;

    [Header("ノイズ対策（この数値以下の雑音は無視）")]
    public float minVolumeThreshold = 0.01f;

    private AudioSource audioSource;
    private string micName;
    private float currentTargetHeight = 0f;

    // 設定画面で操作するマイク感度
    private float currentMicSensitivity = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // タイトル画面で保存したマイク感度を読み込む
        // スライダーの0.5が「1倍」、1.0が「2倍」の感度になるように *2f しています
        currentMicSensitivity = PlayerPrefs.GetFloat("MicSensitivity", 0.5f) * 2f;

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

        // 取得した生の音量に、設定画面の感度（ブースト）を掛け
        float volume = GetVolume() * currentMicSensitivity;

        if (volume > minVolumeThreshold)
        {
            float pitchIndex = GetPitch();

            if (pitchIndex > 0)
            {
                currentTargetHeight = (pitchIndex - neutralPitchIndex) * pitchSensitivity;
            }
        }
        else
        {
            currentTargetHeight = 0f;
        }

        // 大きさの反映
        float targetScale = 1f + (volume * volumeSensitivity);
        targetCube.localScale = Vector3.Lerp(targetCube.localScale, new Vector3(targetScale, waveThickness, targetScale), Time.deltaTime * 15f);

        // 高さの反映
        Vector3 newPos = new Vector3(0, currentTargetHeight, 0);
        targetCube.position = Vector3.Lerp(targetCube.position, newPos, Time.deltaTime * 10f);
    }

    // 音量計算を、より声に敏感に反応する「RMS方式」に改良
    float GetVolume()
    {
        float[] data = new float[256];
        audioSource.GetOutputData(data, 0);
        float sum = 0;
        foreach (float s in data)
        {
            sum += s * s; // ただ足すのではなく、二乗（掛け算）して足す
        }
        return Mathf.Sqrt(sum / 256f); // 最後にルートをかけて戻す
    }

    float GetPitch()
    {
        float[] spectrum = new float[1024];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        float maxV = 0;
        int maxN = -1;

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