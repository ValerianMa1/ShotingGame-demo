
using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour
{
    static Text scoreText;


    //尽量在awake函数中对当前脚本进行初始化，awake函数比start函数先运行，如果在start函数中调用其他类的话可能会出现空引用的问题，即有些类还没初始化就被其他类调用了
    void Awake() 
    {
        scoreText = GetComponent<Text>();
    }

    void Start() 
    {
        ScoreManager.Instance.ResetScore();    
    }


    public static void UpdateText(int score)
    {
        scoreText.text = score.ToString();
    }

    public static void ScaleText(Vector3 targetScale)
    {
        scoreText.rectTransform.localScale = targetScale;   
    }
}
