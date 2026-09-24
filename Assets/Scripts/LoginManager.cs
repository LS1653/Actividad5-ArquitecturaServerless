using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private FirebaseManager firebaseManager;

    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    private UserDataManager userDataManager;

    [SerializeField] private TMP_Text messageText;

    [SerializeField] private ReactionGame reactionGame;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject gamePanel;

    public void Login()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
        {
            messageText.text = "Completa todos los campos.";
            return;
        }

        messageText.text = "Iniciando sesión...";

        FirebaseAuth auth = firebaseManager.Auth;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    messageText.text = "Inicio de sesión cancelado.";
                    return;
                }

                if (task.IsFaulted)
                {
                    messageText.text = "Correo o contraseña incorrectos.";
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;

                Debug.Log("Inicio de sesión exitoso.");
                Debug.Log("UID: " + user.UserId);
                Debug.Log("Email: " + user.Email);

                userDataManager = UserDataManager.Instance;
                userDataManager.LoadUserData(user.UserId);

                reactionGame.RestartGame();

                messageText.text = "Inicio de sesión exitoso.";

                GoToGame();
            });
    }

    public void GoToRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    private void GoToGame()
    {
        loginPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}