using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore.V1;

namespace DisasterDonationAlleviationApp.Services
{
    public class FirebaseService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirebaseService()
        {
            // Initialize Firestore with the service account JSON file
            GoogleCredential credential = GoogleCredential.FromFile("C:/Users/User/source/repos/DisasterDonationAlleviationApp/DisasterDonationAlleviationApp/wwwroot/disasteralleviation-61dab-firebase-adminsdk-pafa0-9ed94811ce.json");

            // Create FirestoreDb instance with the credential
            FirestoreClientBuilder builder = new FirestoreClientBuilder
            {
                CredentialsPath = "C:/Users/User/source/repos/DisasterDonationAlleviationApp/DisasterDonationAlleviationApp/wwwroot/disasteralleviation-61dab-firebase-adminsdk-pafa0-9ed94811ce.json"
            };

            _firestoreDb = FirestoreDb.Create("disasteralleviation-61dab", builder.Build());
        }

        // Method to write data to Firestore
        public async Task WriteDataAsync(string collectionName, string documentId, Dictionary<string, object> data)
        {
            DocumentReference documentReference = _firestoreDb.Collection(collectionName).Document(documentId);
            await documentReference.SetAsync(data);
        }

        // Method to read data from Firestore
        public async Task<Dictionary<string, object>> ReadDataAsync(string collectionName, string documentId)
        {
            DocumentReference documentReference = _firestoreDb.Collection(collectionName).Document(documentId);
            DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                return snapshot.ToDictionary();
            }

            return null;
        }
    }
}
