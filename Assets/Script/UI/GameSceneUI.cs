using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;

    [SerializeField]
    private TMP_Text moveLeft;

    private void Start()
    {
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
    }

    public void UpdatePlayerMoveLeft(float timeLeft)
    {
        int min = Mathf.FloorToInt(timeLeft / 60);
        int sec = Mathf.FloorToInt(timeLeft % 60);
        this.moveLeft.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void PopupWinPanelInGameScene()
    {
        winPanel.gameObject.SetActive(true);
        FadePanelInGameScene(winPanel.GetComponent<CanvasGroup>());
    }

    public void PopupLosePanelInGameScene()
    {
        losePanel.gameObject.SetActive(true);
        FadePanelInGameScene(winPanel.GetComponent<CanvasGroup>());
    }

    private void FadePanelInGameScene(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);
    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
