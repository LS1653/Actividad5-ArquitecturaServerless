using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    public PlayerData CurrentPlayer { get; private set; }

    private DatabaseReference databaseReference;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoadUserData(string uid)
    {
        // Se corrigió GetReference por Child
        DatabaseReference userReference = databaseReference.Child("users").Child(uid);

        userReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("La carga de datos del usuario fue cancelada.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("Error al cargar los datos del usuario.");
                Debug.LogError(task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                Debug.LogError("No existen datos para este usuario.");
                return;
            }

            string username = snapshot.Child("username").Value.ToString();
            int score = int.Parse(snapshot.Child("score").Value.ToString());

            CurrentPlayer = new PlayerData
            {
                username = username,
                score = score
            };

            Debug.Log("Datos del usuario cargados correctamente.");
            Debug.Log("Username: " + CurrentPlayer.username);
            Debug.Log("Score: " + CurrentPlayer.score);
        });
    }
}