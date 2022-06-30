using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Storage.v1;

namespace AgoraCallVideo.Extensions.GoogleCloud
{
    /// <summary>
    /// Authenticates with Google.Apis.* libraries.
    /// </summary>
    class ApiLibrary : AuthLibrary
    {
        // [START auth_api_implicit]
        public object AuthImplicit(string projectId)
        {
            GoogleCredential credential =
                GoogleCredential.GetApplicationDefault();
            // Inject the Cloud Storage scope if required.
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    StorageService.Scope.DevstorageReadOnly
                });
            }
            var storage = new StorageService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DotNet Google Cloud Platform Auth Sample",
            });
            var request = new BucketsResource.ListRequest(storage, projectId);
            var requestResult = request.Execute();
            foreach (var bucket in requestResult.Items)
            {
                Console.WriteLine(bucket.Name);
            }
            return null;
        }
        // [END auth_api_implicit]

        // [START auth_api_explicit]
        public object AuthExplicit(string projectId, string jsonPath)
        {
            var credential = GoogleCredential.FromFile(jsonPath);
            // Inject the Cloud Storage scope if required.
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    StorageService.Scope.DevstorageReadOnly
                });
            }
            var storage = new StorageService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DotNet Google Cloud Platform Auth Sample",
            });
            var request = new BucketsResource.ListRequest(storage, projectId);
            var requestResult = request.Execute();
            foreach (var bucket in requestResult.Items)
            {
                Console.WriteLine(bucket.Name);
            }
            return null;
        }
        // [END auth_api_explicit]

        // [START auth_api_explicit_compute_engine]
        public object AuthExplicitComputeEngine(string projectId)
        {
            // Explicitly use service account credentials by specifying the 
            // private key file.
            GoogleCredential credential =
                GoogleCredential.FromComputeCredential();
            // Inject the Cloud Storage scope if required.
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    StorageService.Scope.DevstorageReadOnly
                });
            }
            var storage = new StorageService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DotNet Google Cloud Platform Auth Sample",
            });
            var request = new BucketsResource.ListRequest(storage, projectId);
            var requestResult = request.Execute();
            foreach (var bucket in requestResult.Items)
            {
                Console.WriteLine(bucket.Name);
            }
            return null;
        }
        // [END auth_api_explicit_compute_engine]
    }
}
