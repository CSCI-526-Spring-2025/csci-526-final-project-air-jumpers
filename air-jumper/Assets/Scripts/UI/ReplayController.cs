using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayController : MonoBehaviour
{
    // �˷������ڰ�ť���ʱ����
    public void ReplayGame()
    {
        Time.timeScale = 1f; // ���֮ǰ��ͣ����Ϸ���ָ�ʱ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        BuildingInventoryManager.Instance?.Clear();
    }
}
