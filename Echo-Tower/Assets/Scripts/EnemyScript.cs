using UnityEngine;
using UnityEngine.VFX; // VFX Graphをプログラムで操作するために必要

public class EnemyScript : MonoBehaviour
{
    [Header("やられた時に出すVFX（Visual Effect Graph）")]
    public VisualEffectAsset deathVFX;

    void OnTriggerEnter(Collider other)
    {
        // ぶつかった相手のタグが「PlayerWave」だったら
        if (other.CompareTag("PlayerWave"))
        {
            // 1. その場にVFXを発生させる
            if (deathVFX != null)
            {
                GameObject vfxObj = new GameObject("DeathExplosion");
                vfxObj.transform.position = transform.position;
                VisualEffect vfx = vfxObj.AddComponent<VisualEffect>();
                vfx.visualEffectAsset = deathVFX;

                // エフェクトが終わる頃（2秒後）に消去する
                Destroy(vfxObj, 2f);
            }

            // 2. 自分（敵の箱）を画面から消す
            Destroy(gameObject);
        }
    }
}