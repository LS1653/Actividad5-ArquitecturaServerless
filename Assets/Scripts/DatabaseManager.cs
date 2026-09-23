using Firebase.Database;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference databaseReference;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("Firebase Realtime Database inicializada correctamente.");
    }
}