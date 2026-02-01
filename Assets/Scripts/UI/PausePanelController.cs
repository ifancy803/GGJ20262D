using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PausePanelController : MonoBehaviour
{
    [Header("UI 组件")]
    public GameObject pausePanelObj;       // 整个黑色背景Panel
    public CanvasGroup pauseCanvasGroup;   // 用于做透明度渐变
    public RectTransform contentContainer; // 中间的弹窗框（放按钮的父物体）
    public Button openPauseBtn;            // 右上角打开按钮

    [Header("功能按钮")]
    public Button resumeBtn;       // 继续
    public Button backMenuBtn;     // 返回主菜单
    public Button quitBtn;         // 退出

    [Header("主菜单场景名")]
    public string mainMenuSceneName = "MainMenu"; // 务必改成你实际的场景名

    private bool isPaused = false;

    void Start()
    {
        // 初始化隐藏
        pausePanelObj.SetActive(false);
        pauseCanvasGroup.alpha = 0;

        // 绑定事件
        openPauseBtn.onClick.AddListener(PauseGame);
        resumeBtn.onClick.AddListener(ResumeGame);
        backMenuBtn.onClick.AddListener(ReturnToMenu);
        quitBtn.onClick.AddListener(QuitGame);
    }

    // 暂停
    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // 冻结时间
        
        pausePanelObj.SetActive(true);

        // 动画1：背景渐显 (忽略TimeScale)
        pauseCanvasGroup.DOFade(1f, 0.3f).SetUpdate(true);
        
        // 动画2：弹窗从小变大弹出 (忽略TimeScale)
        contentContainer.localScale = Vector3.one * 0.7f;
        contentContainer.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    // 继续
    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 恢复时间

        // 动画：淡出后关闭物体
        pauseCanvasGroup.DOFade(0f, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            pausePanelObj.SetActive(false);
        });
    }

    // 返回主菜单
    void ReturnToMenu()
    {
        Time.timeScale = 1f; // 切换场景前必须恢复时间！
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // 退出游戏
    void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}