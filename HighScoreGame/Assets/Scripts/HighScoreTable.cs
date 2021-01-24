using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    public GameObject scoreEntryPrefab;

    private string scoresUrl = "http://localhost:3000/publications/scores";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScores());
    }

    private IEnumerator LoadScores()
    {
        using (UnityWebRequest webData = UnityWebRequest.Get(scoresUrl))
        {
            yield return webData.SendWebRequest();
            if (webData.isNetworkError || webData.isHttpError)
            {
                Debug.LogError("Score loading error.");
                Debug.LogError(webData.error);
            }
            else
            {
                if (webData.isDone)
                {
                    JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(webData.downloadHandler.data));

                    if (jsonData == null)
                    {
                        Debug.LogWarning("No score data.");
                    }
                    else
                    {
                        Transform entriesTransform = transform.Find("Entries");

                        var scores = jsonData["scores"];
                        for (int scoreIndex = 0; scoreIndex < scores.Count; scoreIndex++)
                        {
                            JSONNode scoreData = scores[scoreIndex];

                            GameObject scoreEntry = GameObject.Instantiate(scoreEntryPrefab, entriesTransform);
                            scoreEntry.transform.localPosition = new Vector3(0, -scoreIndex * 40, 0);

                            Text nameText = scoreEntry.transform.Find("Name").GetComponent<Text>();
                            nameText.text = scoreData["name"].ToString();

                            Text scoreText = scoreEntry.transform.Find("Score").GetComponent<Text>();
                            scoreText.text = scoreData["score"].ToString();
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
