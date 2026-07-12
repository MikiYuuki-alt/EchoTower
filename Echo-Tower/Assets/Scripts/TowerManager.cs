using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TowerManager : MonoBehaviour
{
    [Header("塔の体力")]
    public int hp = 3;

    [Header("HPを表示するテキストUI")]
    public TextMeshProUGUI hpText;

    [Header("画面を赤くするUI（DamageOverlay）")]
    public GameObject damageOverlay;

    private CameraShake cameraShake;
    private float originalFontSize;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();

        if (hpText != null)
        {
            originalFontSize = hpText.fontSize;
            UpdateUIText();
        }

        if (damageOverlay != null)
        {
            damageOverlay.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        hp -= 1;

        // 連続でダメージを受けた時、前の演出を一度止める
        StopAllCoroutines();

        // ★【修正ポイント1】演出ストップで文字がバグるのを防ぐため、ここで一度確実にリセットする！
        if (hpText != null)
        {
            hpText.fontSize = originalFontSize;
            hpText.color = new Color(0f, 1f, 1f); // シアンに戻す
            UpdateUIText();
        }
        if (damageOverlay != null)
        {
            damageOverlay.SetActive(false);
        }

        if (hp <= 0)
        {
            StartCoroutine(GameOverRoutine());
        }
        else
        {
            StartCoroutine(DamageWarningRoutine());
        }
        if (ComboManager.instance != null) { ComboManager.instance.ResetCombo(); }
    }
     
    IEnumerator DamageWarningRoutine()
    {
        if (cameraShake != null)
        {
            cameraShake.Shake(0.3f, 0.5f);
        }

        if (hpText != null && damageOverlay != null)
        {
            damageOverlay.SetActive(true);

            // ★【修正ポイント2】文字をデカくしすぎない（1.1倍くらいにしてピクッとさせるだけ）
            hpText.fontSize = originalFontSize * 1.1f;

            // ★【修正ポイント3】文字の内容をガラッと変えるのではなく、元の文章を使う
            string normalText = "SYSTEM INTEGRITY: " + hp + "/3";
            string glitchText = "SY5T3M ERR0R... " + hp + "/3"; // 少しだけ文字化け

            // 赤と黄色でチカチカさせる
            for (int i = 0; i < 6; i++)
            {
                hpText.text = (i % 2 == 0) ? normalText : glitchText;
                hpText.color = (i % 2 == 0) ? Color.red : Color.yellow;

                if (i == 2) { damageOverlay.SetActive(false); }

                yield return new WaitForSeconds(0.08f);
            }

            damageOverlay.SetActive(false);
            hpText.color = Color.red;
            hpText.text = normalText;
            yield return new WaitForSeconds(0.6f);

            // 確実に戻す
            hpText.color = new Color(0f, 1f, 1f);
            hpText.fontSize = originalFontSize;
            UpdateUIText();
        }
    }

    void UpdateUIText()
    {
        if (hpText != null)
        {
            hpText.text = "ENERGY: " + hp + "/3";
        }
    }

    IEnumerator GameOverRoutine()
    {
        if (cameraShake != null)
        {
            cameraShake.Shake(1.0f, 1.0f);
        }

        if (hpText != null && damageOverlay != null)
        {
            damageOverlay.SetActive(true);
            hpText.fontSize = originalFontSize * 1.3f;

            for (int i = 0; i < 15; i++)
            {
                hpText.color = (i % 2 == 0) ? Color.red : Color.black;
                hpText.text = (i % 2 == 0) ? "CRITICAL ERROR" : "0x000000000000";
                yield return new WaitForSeconds(0.1f);
            }

            hpText.color = Color.red;
            hpText.text = ">>> SYSTEM OFFLINE <<<";
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("TitleScene");
    }
}