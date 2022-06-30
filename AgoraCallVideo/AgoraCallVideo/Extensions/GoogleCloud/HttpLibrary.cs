using Google.Apis.Auth.OAuth2;

namespace AgoraCallVideo.Extensions.GoogleCloud
{
    /// <summary>
    /// Authenticates with Google.Apis.* libraries.
    /// </summary>
    class HttpLibrary : AuthLibrary
    {
        // [START auth_http_implicit]
        public object AuthImplicit(string projectId)
        {
            GoogleCredential credential =
                GoogleCredential.GetApplicationDefault();
            // Inject the Cloud Storage scope if required.
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    "https://www.googleapis.com/auth/devstorage.read_only"
                });
            }
            HttpClient http = new Google.Apis.Http.HttpClientFactory()
                .CreateHttpClient(
                new Google.Apis.Http.CreateHttpClientArgs()
                {
                    ApplicationName = "Google Cloud Platform Auth Sample",
                    GZipEnabled = true,
                    Initializers = { credential },
                });
            UriBuilder uri = new UriBuilder(
                "https://www.googleapis.com/storage/v1/b");
            uri.Query = "project=" +
                System.Web.HttpUtility.UrlEncode(projectId);
            var resultText = http.GetAsync(uri.Uri).Result.Content
                .ReadAsStringAsync().Result;
            dynamic result = Newtonsoft.Json.JsonConvert
                .DeserializeObject(resultText);
            foreach (var bucket in result.items)
            {
                Console.WriteLine(bucket.name);
            }
            return null;
        }
        // [END auth_http_implicit]

        // [START auth_http_explicit]
        public object AuthExplicit(string projectId, string jsonPath)
        {
            var credential = GoogleCredential.FromFile(jsonPath);
            // Inject the Cloud Storage scope if required.
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(new[]
                {
                    "https://www.googleapis.com/auth/devstorage.read_only"
                });
            }
            HttpClient http = new Google.Apis.Http.HttpClientFactory()
                .CreateHttpClient(
                new Google.Apis.Http.CreateHttpClientArgs()
                {
                    ApplicationName = "Google Cloud Platform Auth Sample",
                    GZipEnabled = true,
                    Initializers = { credential },
                });
            UriBuilder uri = new UriBuilder(
                "https://www.googleapis.com/storage/v1/b");
            uri.Query = "project=" +
                System.Web.HttpUtility.UrlEncode(projectId);
            var resultText = http.GetAsync(uri.Uri).Result.Content
                .ReadAsStringAsync().Result;
            dynamic result = Newtonsoft.Json.JsonConvert
                .DeserializeObject(resultText);
            foreach (var bucket in result.items)
            {
                Console.WriteLine(bucket.name);
            }
            return null;
        }

        public object AuthExplicitComputeEngine(string projectId)
        {
            throw new NotImplementedException();
        }
        // [END auth_http_explicit]
    }
}
