using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動の合言葉

public class TitleManager : MonoBehaviour
{
    // ボタンが押されたら呼ばれる処理
    public void StartGame()
    {
        // "GameScene" を読み込んでゲームを始める！
        TransitionManager.instance.FadeToScene("GameScene");
    }
}