using Firebase.Auth;
using UnityEngine;

public class LogoutManager : MonoBehaviour
{
    [SerializeField] private FirebaseManager firebaseManager;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject gameOverPanel;

    public void Logout()
    {
        FirebaseAuth auth = firebaseManager.Auth;

        if (auth == null)
        {
            Debug.LogError("Firebase Authentication no está inicializado.");
            return;
        }

        auth.SignOut();

        UserDataManager.Instance.ClearUserData();

        // Ocultamos cualquier panel que haya quedado abierto.
        gameOverPanel.SetActive(false);

        Debug.Log("Sesión cerrada.");

        gamePanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}