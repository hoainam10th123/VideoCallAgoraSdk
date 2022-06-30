using Google.Apis.Auth.OAuth2;
using Google.Cloud.Language.V1;
using Grpc.Auth;

namespace AgoraCallVideo.Extensions.GoogleCloud
{
    /// <summary>
    /// Authenticates with Google.Cloud.* libraries.
    /// Specifically calls the language API, but all the machine learning APIs,
    /// And some other APIs like Pub/Sub also follow this pattern.
    /// </summary>
    public class CloudLibrary : AuthLibrary
    {
        // [START auth_cloud_explicit]
        // Other APIs, like Language, accept a channel in their Create()
        // method.
        public object AuthExplicit(string projectId, string jsonPath)
        {
            LanguageServiceClientBuilder builder = new LanguageServiceClientBuilder
            {
                CredentialsPath = jsonPath
            };

            LanguageServiceClient client = builder.Build();
            AnalyzeSentiment(client);
            return 0;
        }
        // [END auth_cloud_explicit]

        // [START auth_cloud_explicit_compute_engine]
        // Other APIs, like Language, accept a channel in their Create()
        // method.
        public object AuthExplicitComputeEngine(string projectId)
        {
            LanguageServiceClientBuilder builder = new LanguageServiceClientBuilder
            {
                ChannelCredentials = GoogleCredential
                .FromComputeCredential()
                .ToChannelCredentials()
            };

            LanguageServiceClient client = builder.Build();
            AnalyzeSentiment(client);
            return 0;
        }
        // [END auth_cloud_explicit_compute_engine]

        public object AuthImplicit(string projectId)
        {
            var client = LanguageServiceClient.Create();
            AnalyzeSentiment(client);
            return 0;
        }

        void AnalyzeSentiment(LanguageServiceClient client)
        {
            string text = "Hello World!";
            var response = client.AnalyzeSentiment(new Document()
            {
                Content = text,
                Type = Document.Types.Type.PlainText
            });
            var sentiment = response.DocumentSentiment;
            Console.WriteLine($"Score: {sentiment.Score}");
            Console.WriteLine($"Magnitude: {sentiment.Magnitude}");
        }
    }
}
