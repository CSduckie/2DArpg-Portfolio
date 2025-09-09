using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        // 单例初始化逻辑
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 场景切换时保留该对象
        }
        else
        {
            Destroy(gameObject); // 如果已经存在一个实例，销毁多余的
        }
    }

    // 同步加载场景
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 异步加载场景
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private System.Collections.IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log("加载进度：" + asyncLoad.progress);
            yield return null;
        }
    }

    // 重新加载当前场景
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LoadScene(currentScene.name);
    }

    // 返回主菜单（根据你的主菜单场景名修改）
    public void ReturnToMainMenu()
    {
        LoadScene("MainMenu");
    }

    // 退出游戏（打包后有效）
    public void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
