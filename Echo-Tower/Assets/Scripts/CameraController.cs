using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraController : MonoBehaviour
{
    [Header("カメラが回るスピード")]
    public float rotationSpeed = 0.2f; 

    void Update()
    {
        // もしマウスが接続されていなかったらエラーを防ぐために無視する
        if (Mouse.current == null) return;

        // 右クリックが押されている間
        if (Mouse.current.rightButton.isPressed)
        {
            // マウスの横移動の量を取得
            float mouseX = Mouse.current.delta.x.ReadValue();

            // Y軸（縦の軸）を中心に、軸ごと回転させる
            transform.Rotate(0, mouseX * rotationSpeed, 0);
        }
    }
}