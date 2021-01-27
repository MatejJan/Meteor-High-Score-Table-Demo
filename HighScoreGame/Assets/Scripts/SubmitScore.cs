using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SubmitScore : MonoBehaviour
{
    public GameObject highScoreTableGameObject;

    private string scoreInsertUrl = HighScoreTable.serverUrl + "/methods/scores.insert";

    public void OnClick()
    {
        // Submit the score asynchronously.
        StartCoroutine(SubmitScoreCoroutine());
    }

    private IEnumerator SubmitScoreCoroutine()
    {
        // Create dummy score data.
        var scoreData = new Dictionary<string, string>()
        {
            { "name", "Retro" },
            { "score", Random.Range(100, 1000000).ToString() }
        };

        // Send a POST request to the server.
        using (UnityWebRequest webRequest = UnityWebRequest.Post(scoreInsertUrl, scoreData))
        {
            yield return webRequest.SendWebRequest();

            // Make sure we got a valid response.
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Score submissions error.");
                Debug.LogError(webRequest.error);
            }
            else
            {
                // Refresh the high score table.
                highScoreTableGameObject.GetComponent<HighScoreTable>().LoadScores();
            }
        }
    }
}
