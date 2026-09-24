using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject gameOverPanel;

    public void OpenLeaderboard()
    {
        gameOverPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
}