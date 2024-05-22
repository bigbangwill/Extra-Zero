using Newtonsoft.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using Unity.Services.Leaderboards.Models;

public class LeaderboardManager : MonoBehaviour
{
    // Create a leaderboard with this ID in the Unity Cloud Dashboard
    const string LeaderboardId = "Extra_Zero";

    private LeaderboardScoresPage scorePage;
    private LeaderboardEntry playerEntry;

    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Transform scoreParent;
    [SerializeField] private ScorePrefab selfScore;

    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;
    [SerializeField] private Sprite selfMedal;



    async void Awake()
    {
        await UnityServices.InitializeAsync();
        if(!AuthenticationService.Instance.IsSignedIn)
            await SignInAnonymously();
        await GetScores();
        await GetPlayerScore();
        PaintUI();
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task UpdateName(string name)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
    }

    public async Task AddScore(double score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    public async Task GetScores()
    {
        scorePage = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scorePage));
    }

    public async Task GetPlayerScore()
    {
        playerEntry = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public LeaderboardScoresPage ReturnScorePage()
    {
        return scorePage;
    }

    public LeaderboardEntry ReturnPlayerScore()
    {
        return playerEntry;
    }

    public void PaintUI()
    {
        List<LeaderboardEntry> scoreLists = ReturnScorePage().Results;

        for (int i = 0; i < scoreLists.Count; i++)
        {
            GameObject go = Instantiate(scorePrefab, scoreParent);
            ScorePrefab scorePrefabScript = go.GetComponent<ScorePrefab>();
            Sprite medal;
            switch (i)
            {
                case 0: medal = goldMedal; break;
                case 1: medal = silverMedal; break;
                case 2: medal = bronzeMedal; break;
                default: medal = null; break;
            }
            scorePrefabScript.SetScore(medal, scoreLists[i].PlayerName, scoreLists[i].Score.ToString());
        }
        LeaderboardEntry self = ReturnPlayerScore();
        selfScore.SetScore(selfMedal, self.PlayerName, self.Score.ToString());
    }

    public async void RefreshUI()
    {
        foreach (Transform child in scoreParent)
        {
            Destroy(child.gameObject);
        }
        await GetScores();
        await GetPlayerScore();
        PaintUI();
    }

}
