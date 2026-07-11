using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("湧かせたい敵のプレハブたち（複数登録できるよ！）")]
    public GameObject[] enemyPrefabs;

    [Header("敵が湧く間隔（秒）")]
    public float spawnInterval = 3f;

    [Header("塔からどれくらい離れたところに湧かせるか")]
    public float spawnRadius = 20f;

    // ↓ここを追加！
    [Header("敵が湧く高さの範囲（最小と最大）")]
    public float minSpawnHeight = 0.5f; // 地面近く
    public float maxSpawnHeight = 10.0f; // かなり空中

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        // 1. 登録された敵リスト（配列）の中から、ランダムに1種類を選ぶ
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[randomIndex];

        // 2. 塔を中心に360度ランダムな角度を計算
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // 円の水平位置（X, Z）を計算
        float x = Mathf.Cos(angle) * spawnRadius;
        float z = Mathf.Sin(angle) * spawnRadius;

        // 【ここがポイント！】高さをランダムにする
        float randomY = Random.Range(minSpawnHeight, maxSpawnHeight);

        // 新しいランダムな3D位置を作る
        Vector3 spawnPosition = new Vector3(x, randomY, z);

        // 3. くじ引きで選ばれた敵を、その位置に生み出す！
        Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
    }
}