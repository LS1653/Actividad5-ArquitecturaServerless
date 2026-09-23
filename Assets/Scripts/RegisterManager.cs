using Firebase.Auth;
using Firebase.Database;
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

        messageText.text = "Comprobando username...";

        CheckUsername(username, email, password);
    }

    private void CheckUsername(string username, string email, string password)
    {
        DatabaseReference usernameReference =
            FirebaseDatabase.DefaultInstance
                .GetReference("usernames")
                .Child(username);

        usernameReference.GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    messageText.text = "Comprobación cancelada.";
                    return;
                }

                if (task.IsFaulted)
                {
                    messageText.text = "No se pudo comprobar el username.";
                    Debug.LogError(task.Exception);
                    return;
                }

                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    messageText.text = "Ese username ya está ocupado.";
                    return;
                }

                CreateFirebaseUser(username, email, password);
            });
    }

    private void CreateFirebaseUser(
        string username,
        string email,
        string password)
    {
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

                SaveUsername(user.UserId, username);
            });
    }

    private void SaveUsername(string uid, string username)
    {
        DatabaseReference usernameReference =
            FirebaseDatabase.DefaultInstance
                .GetReference("usernames")
                .Child(username);

        usernameReference.SetValueAsync(uid)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    messageText.text = "No se pudo guardar el username.";
                    return;
                }

                if (task.IsFaulted)
                {
                    messageText.text = "Error al guardar el username.";
                    Debug.LogError(task.Exception);
                    return;
                }

                Debug.Log("Username guardado correctamente.");

                SaveUserData(uid, username);
            });
    }

    private void SaveUserData(string uid, string username)
    {
        DatabaseReference userReference =
            FirebaseDatabase.DefaultInstance
                .GetReference("users")
                .Child(uid);

        UserData userData = new UserData
        {
            username = username,
            score = 0
        };

        string json = JsonUtility.ToJson(userData);

        userReference.SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    messageText.text = "No se pudo guardar el usuario.";
                    return;
                }

                if (task.IsFaulted)
                {
                    messageText.text = "Error al guardar los datos.";
                    Debug.LogError(task.Exception);
                    return;
                }

                Debug.Log("Datos del usuario guardados correctamente.");

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

[System.Serializable]
public class UserData
{
    public string username;
    public int score;
}