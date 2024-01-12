using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "LevelScene";

    [SerializeField]
    private Transform sceneTransition;

    private void Start()
    {
        ActiveSceneTransitionForScene();
    }

    public void ActiveSceneTransitionForScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
    }

    public void ToHomeScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToAnotherScene(MENU));
    }

    public void ToPlaystateScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToAnotherScene(GAME));
    }

    public void ToLevelChossingState()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToAnotherScene(LEVEL_CHOOSE));
    }

    public void ToNextSpinChallenge()
    {
        StopAllCoroutines();
        if (LevelManager.instance.currentLevelIndex < LevelManager.instance.levelData.GiveAllLevelAssigned().Count - 1)
        {
            LevelManager.instance.currentLevelIndex++;
            StartCoroutine(ChangeToAnotherScene(GAME));
        }
        else
        {
            StartCoroutine(ChangeToAnotherScene(LEVEL_CHOOSE));
        }
    }


    private IEnumerator ChangeToAnotherScene(string sceneName)
    {
        DOTween.KillAll();
        //Optional: Add animation here
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
