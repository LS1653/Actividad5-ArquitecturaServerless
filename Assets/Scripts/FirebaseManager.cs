using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseApp app;
    private FirebaseAuth auth;

    public FirebaseAuth Auth => auth;

    private void Start()
    {
        CheckFirebaseDependencies();
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                DependencyStatus dependencyStatus = task.Result;

                if (dependencyStatus == DependencyStatus.Available)
                {
                    app = FirebaseApp.DefaultInstance;
                    auth = FirebaseAuth.DefaultInstance;

                    Debug.Log("Firebase inicializado correctamente.");
                    Debug.Log("Firebase Authentication inicializado correctamente.");
                }
                else
                {
                    Debug.LogError(
                        "No se pudieron resolver las dependencias de Firebase: "
                        + dependencyStatus
                    );
                }
            });
    }
}