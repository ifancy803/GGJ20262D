using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // 引入List需要
using DG.Tweening;

public class MenuPanelController : MonoBehaviour
{
    [Header("Panels")]
    public RectTransform menuPanelRect;  // 主界面
    public RectTransform levelPanelRect; // 选关界面

    [Header("Buttons")]
    public Button startGameBtn;
    public Button quitGameBtn;

    // --- 重点修改部分 ---
    [System.Serializable] // 这个标签让自定义类可以在Inspector中显示
    public class LevelConfig
    {
        public string description; // 可选：仅用于你在Inspector里写备注，比如 "第一关: 森林"
        public Button button;      // 把UI按钮拖进去
        public string sceneName;   // 手动输入场景的名字，例如 "Level_1" 或 "ForestScene"
    }

    [Header("Level Configuration")]
    public List<LevelConfig> levels; // 这里会显示一个列表，你可以点 "+" 号添加任意数量
    // --------------------

    [Header("Animation")]
    public float slideDuration = 0.6f;
    public Ease slideEase = Ease.InOutQuart;

    private float screenWidth;

    void Start()
    {
        // 获取移动距离
        screenWidth = menuPanelRect.rect.width; 

        // 绑定通用事件
        startGameBtn.onClick.AddListener(OnStartGameClicked);
        quitGameBtn.onClick.AddListener(OnQuitGameClicked);

        // --- 遍历配置列表进行绑定 ---
        foreach (var level in levels)
        {
            // 检查配置是否为空，防止报错
            if (level.button != null && !string.IsNullOrEmpty(level.sceneName))
            {
                // 必须在循环内部用局部变量缓存场景名，否则闭包可能会导致所有按钮都进入同一个关卡
                string targetScene = level.sceneName; 
                
                level.button.onClick.AddListener(() => EnterLevel(targetScene));
            }
        }
    }

    // 1. 切换面板动画
    void OnStartGameClicked()
    {
        menuPanelRect.DOAnchorPosX(-screenWidth, slideDuration).SetEase(slideEase);
        levelPanelRect.DOAnchorPosX(0, slideDuration).SetEase(slideEase);
    }

    // 2. 进入具体关卡 (改为接收字符串)
    void EnterLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 3. 退出游戏
    void OnQuitGameClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}