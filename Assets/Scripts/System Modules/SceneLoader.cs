using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{

    [SerializeField] UnityEngine.UI.Image transitionImage; 
    [SerializeField] float fadeTime = 3.5f;
    Color color; 
    const string GAMEPLAY = "Gameplay";
    const string MAINMENU = "MainMenu";







    IEnumerator LoadingCoroutine(string sceneName)
    {

        //loadsceneAsync可以在后台提前加载场景，不妨碍主线程的运行，可以解决画面卡顿一下的问题，返回值为loadingOperation类型，用allowsceneActivation来控制是否将加载好的场景激活
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;
        transitionImage.gameObject.SetActive(true);
        while(color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);//这个间隔时间不会受到time.timeScale的影响，即使timesca的值设为0也不会受影响
            transitionImage.color = color;
            yield return null;
        }


        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);
        loadingOperation.allowSceneActivation = true;

        
        while(color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);//这个间隔时间不会受到time.timeScale的影响，即使timesca的值设为0也不会受影响
            transitionImage.color = color;
            yield return null;
        }
        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));
    }




    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAINMENU));
    }
}
