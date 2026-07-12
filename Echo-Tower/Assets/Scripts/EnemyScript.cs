using UnityEngine;
using UnityEngine.VFX;

public class EnemyScript : MonoBehaviour
{
    [Header("やられた時に出すVFX")]
    public VisualEffectAsset deathVFX;

    [Header("移動スピード")]
    public float moveSpeed = 3f;

    private Transform targetTower;

    void Start()
    {
        // ヒエラルキーから「Tower」という名前のオブジェクトを探して目標にする
        GameObject towerObj = GameObject.Find("Tower");
        if (towerObj != null)
        {
            targetTower = towerObj.transform;
        }
    }

    void Update()
    {
        // 塔が見つかっていれば、そっちに向かって進む
        if (targetTower != null)
        {
            // 塔の方向を向く（傾かないようにY座標は自分のままにする）
            Vector3 targetPosition = new Vector3(targetTower.position.x, transform.position.y, targetTower.position.z);
            transform.LookAt(targetPosition);

            // 前に真っ直ぐ進む
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // パターンA：波（PlayerWave）に当たった時
        if (other.CompareTag("PlayerWave"))
        {
            PlayDeathVFX(); // 爆発エフェクトを出す
            Destroy(gameObject); // 自分を消す
        }
        // パターンB：塔（Tower）に当たった時
        else if (other.CompareTag("Tower"))
        {
            // ※ここで塔のHPを減らす処理を後で追加します
            Debug.Log("タワーにダメージ！");
            // 塔のスクリプト（TowerManager）を探して、ダメージを与える！
            TowerManager tower = other.GetComponent<TowerManager>();
            if (tower != null)
            {
                tower.TakeDamage();
            }
            Destroy(gameObject); // 塔に当たった敵も役目を終えて消える
        }
    }

    // VFXを出す処理（スッキリさせるために分けました）
    void PlayDeathVFX()
    {
        if (deathVFX != null)
        {
            GameObject vfxObj = new GameObject("DeathExplosion");
            vfxObj.transform.position = transform.position;
            VisualEffect vfx = vfxObj.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = deathVFX;
            Destroy(vfxObj, 2f);
        }
    }
    public void Die()
    {
        // ★追加：コンボを追加する！
        if (ComboManager.instance != null) { ComboManager.instance.AddCombo(); }

        // 死亡時のエフェクト生成や、オブジェクトの破棄処理が続く...
    }
}