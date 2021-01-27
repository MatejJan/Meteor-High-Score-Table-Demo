using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    public GameObject scoreEntryPrefab;
    public const string serverUrl = "http://localhost:3000";

    private string scoresUrl = serverUrl + "/publications/scores";

    void Start()
    {
        LoadScores();
    }

    public void LoadScores()
    {
        // Load scores asynchronously.
        StartCoroutine(LoadScoresCoroutine());
    }

    private IEnumerator LoadScoresCoroutine()
    {
        // Send a GET request to the scores URL.
        using (UnityWebRequest webRequest = UnityWebRequest.Get(scoresUrl))
        {
            yield return webRequest.SendWebRequest();

            // Make sure we got a valid response.
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Score loading error.");
                Debug.LogError(webRequest.error);
            }
            else
            {
                if (webRequest.isDone)
                {
                    // Parse response data as JSON.
                    JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

                    if (jsonData == null)
                    {
                        Debug.LogWarning("No score data.");
                    }
                    else
                    {
                        // Delete any previous score entries.
                        Transform entriesTransform = transform.Find("Entries");

                        foreach (Transform childTransform in entriesTransform)
                        {
                            GameObject.Destroy(childTransform.gameObject);
                        }

                        // Add current score entries.
                        var scores = jsonData["scores"];

                        for (int scoreIndex = 0; scoreIndex < scores.Count; scoreIndex++)
                        {
                            JSONNode scoreData = scores[scoreIndex];

                            // Create score entry and position it in the list.
                            GameObject scoreEntry = GameObject.Instantiate(scoreEntryPrefab, entriesTransform);
                            scoreEntry.transform.localPosition = new Vector3(0, -scoreIndex * 25, 0);

                            // Set the name of the player.
                            Text nameText = scoreEntry.transform.Find("Name").GetComponent<Text>();
                            nameText.text = scoreData["name"].ToString();

                            // Set the score value.
                            Text scoreText = scoreEntry.transform.Find("Score").GetComponent<Text>();
                            scoreText.text = scoreData["score"].ToString();
                        }
                    }
                }
            }
        }
    }
}
