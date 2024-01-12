using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameSceneUI gameScene;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private float timeLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetTheLevelAtGivenIndex(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        timeLeft = currentLevelData.moveLimit;
        gameScene.UpdatePlayerMoveLeft(timeLeft);
        Time.timeScale = 1;
    }

    public void PlayerWinThisLevel()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        LevelManager.instance.levelData.ReSetGivenLevelData(LevelManager.instance.currentLevelIndex, true, true);
        if (LevelManager.instance.levelData.GiveAllLevelAssigned().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetTheLevelAtGivenIndex(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.ReSetGivenLevelData(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        isGameWin = true;
        StartCoroutine(DelayShowWinPanel());
        LevelManager.instance.levelData.SaveThisDataToJsonFile();
    }

    private void Update()
    {
        DecreaseTimeAndShowUI();
        CheckMoveLeftToCheckLose();
    }

    private IEnumerator DelayShowWinPanel()
    {
        yield return new WaitForSeconds(.5f);
        gameScene.PopupWinPanelInGameScene();
    }

    public void DecreaseTimeAndShowUI()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        gameScene.UpdatePlayerMoveLeft(timeLeft);
    }

    public void CheckMoveLeftToCheckLose()
    {
        if (timeLeft <= 0)
        {
            if (isGameWin || isGameLose)
            {
                return;
            }
            PlayerLoseThisLevelAndShowUI();
        }
    }


    public void PlayerLoseThisLevelAndShowUI()
    {
        isGameLose = true;
        gameScene.PopupLosePanelInGameScene();
    }

    public bool IsThisGameFinalOrWin()
    {
        return isGameWin;
    }

    public bool IsThisGameFinalOrLose()
    {
        return isGameLose;
    }
}

