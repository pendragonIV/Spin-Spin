using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);
        gameLogo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        gameLogo.GetComponent<Image>().DOFade(1, 1f).SetUpdate(true);
    }

    public void ShowHowToPlayPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        ShowTutorialPanelAnim(tutorPanel.GetComponent<CanvasGroup>());

    }

    public void HideHowToPlayPanel()
    {
        StartCoroutine(HideTutorialPanelAnim(tutorPanel.GetComponent<CanvasGroup>()));

    }   

    private void ShowTutorialPanelAnim(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);
    }

    private IEnumerator HideTutorialPanelAnim(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        tutorPanel.gameObject.SetActive(false);

    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
