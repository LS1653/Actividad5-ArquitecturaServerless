using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] private FirebaseManager firebaseManager;

    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private TMP_Text messageText;

    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject loginPanel;

    public void Register()
    {
        string username = usernameInput.text.Trim();
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
        {
            messageText.text = "Completa todos los campos.";
            return;
        }

        messageText.text = "Registrando...";

        FirebaseAuth auth = firebaseManager.Auth;

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    messageText.text = "Registro cancelado.";
                    return;
                }

                if (task.IsFaulted)
                {
                    messageText.text = "No se pudo realizar el registro.";
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseUser user = task.Result.User;

                Debug.Log("Usuario registrado correctamente.");
                Debug.Log("UID: " + user.UserId);
                Debug.Log("Email: " + user.Email);

                messageText.text = "Registro exitoso.";

                registerPanel.SetActive(false);
                loginPanel.SetActive(true);
            });
    }

    public void GoToLogin()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}