using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private FirebaseManager firebaseManager;

    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [SerializeField] private TMP_Text messageText;

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

                messageText.text = "Inicio de sesión exitoso.";
            });
    }
}