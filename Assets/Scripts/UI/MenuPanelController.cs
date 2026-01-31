using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPanelController : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    
    public GameObject settingsPanel;
    
    private void OnEnable()
    {
        startButton.onClick.AddListener(StartButtonFunc);
        exitButton.onClick.AddListener(ExitButtonFunc);
    }
    
    public void StartButtonFunc()
    {
        //SceneLoadManager.Instance.LoadGameScene();
    }
    
    public void ExitButtonFunc()
    {
        Application.Quit();
    }
}