using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    int leaderboardID = 7107;
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip uiClick;
    [SerializeField] private AudioClip uiHover;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator SubmitScoreRoutine(int totalKills)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, totalKills, leaderboardID, (response) =>
        {
            if (response.success)
            {
                //Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "";
                string tempPlayerScores = "";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if(members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void UIClick()
    {
        audioSource.PlayOneShot(uiClick);
    }

    public void UIHover()
    {
        audioSource.PlayOneShot(uiHover);
    }

    public void Restart()
    {
        //Path.
        string sceneToLoad = SceneManager.GetActiveScene().path;

        #if UNITY_EDITOR
        //Load the scene.
        UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
        #else
            //Load the scene.
            SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
        #endif
    }

    public void Quit()
    {
        //Path.
        string sceneToLoad = SceneManager.GetSceneByBuildIndex(0).ToString();

#if UNITY_EDITOR
        //Load the scene.
        UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
#else
            //Load the scene.
            SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
#endif
    }
}