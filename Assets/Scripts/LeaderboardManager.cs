using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private Transform content;
    [SerializeField] private GameObject rowPrefab;

    public void OpenLeaderboard()
    {
        gameOverPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        LoadLeaderboard();
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void LoadLeaderboard()
    {
        DatabaseReference usersReference =
            FirebaseDatabase.DefaultInstance
                .RootReference
                .Child("users");

        usersReference.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("La carga del leaderboard fue cancelada.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("Error al cargar el leaderboard.");
                    Debug.LogError(task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                List<PlayerData> players = new List<PlayerData>();

                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string username = userSnapshot
                        .Child("username")
                        .Value?
                        .ToString();

                    string scoreText = userSnapshot
                        .Child("score")
                        .Value?
                        .ToString();

                    if (string.IsNullOrEmpty(username) ||
                        string.IsNullOrEmpty(scoreText))
                    {
                        continue;
                    }

                    int score = int.Parse(scoreText);

                    PlayerData player = new PlayerData
                    {
                        username = username,
                        score = score
                    };

                    players.Add(player);
                }

                players = players
                    .OrderByDescending(player => player.score)
                    .ToList();

                ShowLeaderboard(players);
            });
    }

    private void ShowLeaderboard(List<PlayerData> players)
    {
        ClearLeaderboard();

        for (int i = 0; i < players.Count; i++)
        {
            GameObject rowObject =
                Instantiate(rowPrefab, content);

            LeaderboardRowUI row =
                rowObject.GetComponent<LeaderboardRowUI>();

            row.SetData(
                i + 1,
                players[i].username,
                players[i].score
            );
        }

        Debug.Log("Leaderboard cargado. Jugadores: " + players.Count);
    }

    private void ClearLeaderboard()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}