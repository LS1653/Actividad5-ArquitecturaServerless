using UnityEngine;
using UnityEngine.UI;

public class ReactionGame : MonoBehaviour
{
    [SerializeField] private RectTransform gameArea;
    [SerializeField] private RectTransform target;
    [SerializeField] private Button targetButton;

    private void Start()
    {
        MoveTarget();
    }

    public void TargetClicked()
    {
        MoveTarget();
    }

    private void MoveTarget()
    {
        float targetWidth = target.rect.width;
        float targetHeight = target.rect.height;

        float minX = -gameArea.rect.width / 2f + targetWidth / 2f;
        float maxX = gameArea.rect.width / 2f - targetWidth / 2f;

        float minY = -gameArea.rect.height / 2f + targetHeight / 2f;
        float maxY = gameArea.rect.height / 2f - targetHeight / 2f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        target.anchoredPosition = new Vector2(randomX, randomY);
    }
}