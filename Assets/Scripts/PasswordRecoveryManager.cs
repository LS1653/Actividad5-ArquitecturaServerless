using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class PasswordRecoveryManager : MonoBehaviour
{
    [SerializeField] private FirebaseManager firebaseManager;

    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_Text messageText;

    public void RecoverPassword()
    {
        string email = emailInput.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            messageText.text = "Escribe tu correo electrónico.";
            return;
        }

        messageText.text = "Enviando correo...";

        FirebaseAuth auth = firebaseManager.Auth;

        auth.SendPasswordResetEmailAsync(email)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    messageText.text = "Solicitud cancelada.";
                    return;
                }

                if (task.IsFaulted)
                {
                    messageText.text = "No se pudo enviar el correo.";
                    Debug.LogError(task.Exception);
                    return;
                }

                Debug.Log("Correo de recuperación enviado.");

                messageText.text =
                    "Se ha enviado un correo para recuperar tu contraseña.";
            });
    }
}