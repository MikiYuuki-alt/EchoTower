using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition; // カメラの元の位置を記憶する

    // カメラシェイクを開始する命令
    public void Shake(float duration, float magnitude)
    {
        // すでに揺れている場合は、一度止めてからやり直す
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        originalPosition = transform.localPosition; // 開始時の位置を記録

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 時間経過に合わせて、ランダムな方向（X, Y）にカメラをずらす
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null; // 次のフレームまで待つ
        }

        // 揺れが終わったら、カメラの位置を元に戻す
        transform.localPosition = originalPosition;
    }
}