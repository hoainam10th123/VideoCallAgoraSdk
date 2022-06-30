using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace AgoraCallVideo.Extensions.GoogleCloud
{
    public class HandCodedLibrary : AuthLibrary
    {
        ///////////////////////////////////////////////
        // This is the preferred way of authenticating.
        ///////////////////////////////////////////////
        // [START auth_cloud_implicit]
        public object AuthImplicit(string projectId)
        {
            // If you don't specify credentials when constructing the client, the
            // client library will look for credentials in the environment.
            var credential = GoogleCredential.GetApplicationDefault();
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets(projectId);
            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }
            return null;
        }
        // [END auth_cloud_implicit]

        // [START auth_cloud_explicit]
        // Some APIs, like Storage, accept a credential in their Create()
        // method.
        public object AuthExplicit(string projectId, string jsonPath)
        {
            // Explicitly use service account credentials by specifying 
            // the private key file.
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets(projectId);
            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }
            return credential;
        }
        // [END auth_cloud_explicit]

        // [START auth_cloud_explicit_compute_engine]
        // Some APIs, like Storage, accept a credential in their Create()
        // method.
        public object AuthExplicitComputeEngine(string projectId)
        {
            // Explicitly request service account credentials from the compute
            // engine instance.
            GoogleCredential credential =
                GoogleCredential.FromComputeCredential();
            var storage = StorageClient.Create(credential);
            // Make an authenticated API request.
            var buckets = storage.ListBuckets(projectId);
            foreach (var bucket in buckets)
            {
                Console.WriteLine(bucket.Name);
            }
            return null;
        }
        // [END auth_cloud_explicit_compute_engine]
    }
}
