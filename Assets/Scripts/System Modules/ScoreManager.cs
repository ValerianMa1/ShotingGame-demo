using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    int score;
    int currentscore;
    [SerializeField] Vector3 scoreTextScale = new Vector3(1.2f,1.2f,1f);


    public void ResetScore()
    {
        score = 0;
        currentscore = 0;
        ScoreDisplay.UpdateText(score);
    }


    public void AddScore(int scorepoint)
    {
        currentscore += scorepoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }



    


    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while(score < currentscore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }
}
