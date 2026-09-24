using TMPro;
using UnityEngine;

public class LeaderboardRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetData(int position, string username, int score)
    {
        positionText.text = position.ToString();
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}