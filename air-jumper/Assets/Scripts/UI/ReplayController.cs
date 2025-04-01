using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayController : MonoBehaviour
{
    // 此方法会在按钮点击时触发
    public void ReplayGame()
    {
        Time.timeScale = 1f; // 如果之前暂停过游戏，恢复时间流动
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        BuildingInventoryManager.Instance?.Clear();
    }
}
