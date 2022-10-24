using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreTable : MonoBehaviour {

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private int randomScore;
    private List<int> newList;
    private ButtonScript buttonScript;
    public TMP_InputField userName;
    public TMP_InputField userScore;
   

    private void Awake() {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
       
        if (highscores == null) {
            
            // There's no stored table, initialize
            for (int i = 0; i < 9; i++)
            {
                randomScore = Random.Range(50, 100);
                string name = "Player" + (i + 1);

                AddHighscoreEntry(randomScore, name);
            }

            AddHighscoreEntry(int.Parse(userScore.text), userName.text); // kullanıcı değeri.

            
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);

           
        }

        else
        {
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {

                int rndscoreAdd = Random.Range(100, 200);
                highscores.highscoreEntryList[i].score += rndscoreAdd;
                JsonUtility.FromJsonOverwrite(jsonString, highscores.highscoreEntryList[i].score);
            }

            Debug.Log(highscores.highscoreEntryList[9].score);

            AddHighscoreEntry(int.Parse(userScore.text) + highscores.highscoreEntryList[9].score, userName.text); // kullanıcı değeri.

        }
        // Sort entry list by Score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
              for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                  if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                      // Swap
                      HighscoreEntry tmp = highscores.highscoreEntryList[i];
                      highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                      highscores.highscoreEntryList[j] = tmp;
                  }
              }
          }


          highscoreEntryTransformList = new List<Transform>();
         foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
             CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
          }

    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {

        float templateHeight = 100f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;

        int score = highscoreEntry.score; // highscores.highscoreEntryList[i].score
        entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;

        
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<TMP_Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TMP_Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<TMP_Text>().color = Color.green;
        }

       

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() 
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }


    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }




    
    [System.Serializable] 
    private class HighscoreEntry {
        public int score;
        public string name;
    }

}
