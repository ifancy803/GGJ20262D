using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("下一关场景名（需加入 Build Settings）")]
    public string nextSceneName;

    private bool triggered = false; // 防止重复触发

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        if (string.IsNullOrEmpty(nextSceneName)) return;

        triggered = true;
        SceneManager.LoadScene(nextSceneName);
    }
}