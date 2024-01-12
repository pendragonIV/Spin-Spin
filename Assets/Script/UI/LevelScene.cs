using DG.Tweening;
using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;

    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;

    public Transform sceneTransition;
    [SerializeField]
    private Transform commingSoonLevel;

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

    void Start()
    {
        PrepareLevelsForThisScene();
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

    private void PrepareLevelsForThisScene()
    {
        for (int i = 0; i < LevelManager.instance.levelData.GiveAllLevelAssigned().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            holder.GetComponent<LevelHolder>().SetLevelIndex(i);
            Level level = LevelManager.instance.levelData.GetTheLevelAtGivenIndex(i);
            if (LevelManager.instance.levelData.GetTheLevelAtGivenIndex(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableLevelClickAndUI();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableLevelClickAndUI();
            }

            if (LevelManager.instance.levelData.GetTheLevelAtGivenIndex(i).isCompleted)
            {
                holder.GetComponent<LevelHolder>().SetCompletedLevelUI();
            }

            holder.GetComponent<CanvasGroup>().alpha = 0;
            holder.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.InSine).SetDelay(i * 0.2f);
        }
        commingSoonLevel.GetComponent<CanvasGroup>().alpha = 0;
        commingSoonLevel.GetComponent<CanvasGroup>().DOFade(1, 0.5f).SetEase(Ease.InSine).SetDelay(LevelManager.instance.levelData.GiveAllLevelAssigned().Count * 0.2f);

    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
