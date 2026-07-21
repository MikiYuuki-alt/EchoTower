using UnityEngine;

public class CyberGridFloor : MonoBehaviour
{
    [Header("グリッドの設定")]
    [ColorUsage(true, true)]
    public Color lineColor = new Color(0f, 2f, 2f, 1f);
    public Color bgColor = new Color(0.02f, 0.02f, 0.05f, 1f);

    public int gridSize = 512;
    public int lineThickness = 4;
    public float tileCount = 30f;

    [Header("星屑（ノイズ）の設定")]
    public float dustDensity = 0.015f;
    public Color dustColor = new Color(0.5f, 1f, 1f, 0.8f);

    void Start()
    {
        Texture2D tex = new Texture2D(gridSize, gridSize);
        // 左下から右上に向かって、1ピクセル（画素）ずつ順番に処理していくループ
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                bool isLine = (x < lineThickness || x > gridSize - lineThickness ||
                               y < lineThickness || y > gridSize - lineThickness);

                if (isLine)
                {
                    tex.SetPixel(x, y, lineColor);
                }
                else
                {
                    if (Random.value < dustDensity)
                    {
                        tex.SetPixel(x, y, dustColor);
                    }
                    else
                    {
                        tex.SetPixel(x, y, bgColor);
                    }
                }
            }
        }
        tex.Apply();

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetTexture("_BaseMap", tex);

        // 光る場所を「自動生成した画像（線と星屑）」だけに限定する
        mat.EnableKeyword("_EMISSION");
        mat.SetTexture("_EmissionMap", tex);
        mat.SetColor("_EmissionColor", Color.white);

        Renderer rend = GetComponent<Renderer>();
        rend.material = mat;
        rend.material.SetTextureScale("_BaseMap", new Vector2(tileCount, tileCount));
        rend.material.SetTextureScale("_EmissionMap", new Vector2(tileCount, tileCount)); // 光る部分もリピートさせる
    }
}