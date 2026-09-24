using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;

public class ReactionGame : MonoBehaviour
{
    [SerializeField] private RectTransform gameArea;
    [SerializeField] private RectTransform target;
    [SerializeField] private Button targetButton;

    [Header("Tamaño del objetivo")]
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private float minimumScale = 0.25f;
    [SerializeField] private float shrinkSpeed = 0.1f;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Puntuación")]
    [SerializeField] private int score = 0;

    private float currentScale;
    private bool gameFinished = false;

    private void Start()
    {
        currentScale = initialScale;
        target.localScale = Vector3.one * currentScale;

        MoveTarget();
    }

    private void Update()
    {
        ShrinkTarget();
    }

    private void ShrinkTarget()
    {
        if (currentScale <= minimumScale)
        {
            currentScale = minimumScale;
    
            EndGame();
    
            return;
        }
    
        currentScale -= shrinkSpeed * Time.deltaTime;
    
        currentScale = Mathf.Max(currentScale, minimumScale);
    
        target.localScale = Vector3.one * currentScale;
    }

    public void TargetClicked()
    {
        AddScore();
        MoveTarget();
    }

    private void MoveTarget()
    {
        float targetWidth = target.rect.width * currentScale;
        float targetHeight = target.rect.height * currentScale;

        float minX = -gameArea.rect.width / 2f + targetWidth / 2f;
        float maxX = gameArea.rect.width / 2f - targetWidth / 2f;

        float minY = -gameArea.rect.height / 2f + targetHeight / 2f;
        float maxY = gameArea.rect.height / 2f - targetHeight / 2f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        target.anchoredPosition = new Vector2(randomX, randomY);
    }

    private void AddScore()
    {
        if (currentScale > 0.75f)
        {
            score += 10;
        }
        else if (currentScale > 0.50f)
        {
            score += 20;
        }
        else if (currentScale > 0.25f)
        {
            score += 30;
        }
        else
        {
            score += 50;
        }
    
        Debug.Log("Score: " + score);
    }

    private void EndGame()
    {
        if (gameFinished)
            return;
    
        gameFinished = true;
    
        target.gameObject.SetActive(false);
    
        UserDataManager userDataManager = UserDataManager.Instance;
    
        int bestScore = userDataManager.CurrentPlayer.score;
    
        if (score > bestScore)
        {
            finalScoreText.text =
                "Esta partida: " + score +
                "\n\n¡NUEVO RÉCORD!\n" +
                score;
        }
        else
        {
            finalScoreText.text =
                "Esta partida: " + score +
                "\n\nMejor puntuación: " +
                bestScore;
        }
    
        gameOverPanel.SetActive(true);
    
        Debug.Log("FIN DEL JUEGO");
        Debug.Log("Score de esta partida: " + score);
        Debug.Log("Mejor score: " + bestScore);
    
        SaveScore();
    }

    private void SaveScore()
    {
        if (UserDataManager.Instance == null)
        {
            Debug.LogError("No existe UserDataManager.");
            return;
        }
    
        string uid = UserDataManager.Instance.CurrentUserId;
    
        if (string.IsNullOrEmpty(uid))
        {
            Debug.LogError("No existe un UID de usuario.");
            return;
        }
    
        UserDataManager.Instance.SaveScore(uid, score);
    }
}