using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHolder : MonoBehaviour, IPointerClickHandler
{
    private const string GAME = "GameScene";
    [SerializeField]
    private TMP_Text levelIndexText;
    [SerializeField]
    private Image holderFilter;
    [SerializeField]
    private Sprite enabledLevel;
    [SerializeField]
    private CanvasGroup holderCG;

    private int levelIndex;

    private void Start()
    {
        levelIndexText.text = (levelIndex + 1).ToString();
    }

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.instance.currentLevelIndex = levelIndex;
        ToPlayStateScene();
    }

    public void DisableLevelClickAndUI()
    {
        holderFilter.gameObject.SetActive(true);
        holderCG.interactable = false;
        holderCG.blocksRaycasts = false;
    }

    public void EnableLevelClickAndUI()
    {
        holderFilter.gameObject.SetActive(false);
        holderCG.interactable = true;
        holderCG.blocksRaycasts = true;
    }

    public void SetCompletedLevelUI()
    {
        Image enabled = this.transform.GetComponent<Image>();
        enabled.sprite = enabledLevel;
        enabled.SetNativeSize();
    }

    public void ToPlayStateScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToAnotherScene(GAME));
    }

    private IEnumerator ChangeToAnotherScene(string sceneName)
    {
        DOTween.KillAll();
        //Optional: Add animation here
        LevelScene.instance.PlayChangeScene();
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
