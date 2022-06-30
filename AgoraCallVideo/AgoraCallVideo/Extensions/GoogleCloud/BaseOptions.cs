using CommandLine;

namespace AgoraCallVideo.Extensions.GoogleCloud
{
    public class BaseOptions
    {
        string _projectId;
        [Option('p', Default = null, HelpText = "Your Google project id.")]
        public string ProjectId
        {
            get
            {
                if (null == _projectId)
                {
                    _projectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
                    if (null == _projectId)
                    {
                        _projectId = Google.Api.Gax.Platform.Instance()?.GceDetails?.ProjectId;
                        if (null == _projectId)
                        {
                            throw new ArgumentNullException("ProjectId");
                        }
                    }
                }
                return _projectId;
            }
            set
            {
                _projectId = value;
            }
        }

        [Option('j', Default = null, HelpText = "Path to a credentials json file.")]
        public string JsonPath { get; set; }

        [Option('c', Default = false, HelpText = "Pull credentials from compute engine metadata.")]
        public bool Compute { get; set; }
    }

    [Verb("hand", HelpText = "Authenticate using the Google.Cloud.Storage library.  "
        + "The preferred way of authenticating hand-coded wrapper libraries.")]
    class HandOptions : BaseOptions { }

    [Verb("cloud", HelpText = "Authenticate using the Google.Cloud.Language library.  "
        + "The preferred way of authenticating gRPC-based libraries.")]
    class CloudOptions : BaseOptions { }

    [Verb("api", HelpText = "Authenticate using the Google.Apis.Storage library.")]
    class ApiOptions : BaseOptions { }

    [Verb("http", HelpText = "Authenticate using and make a rest HTTP call.")]
    class HttpOptions : BaseOptions { }

    /// <summary>
    /// Each library supports 3 methods of authenticating.
    /// </summary>
    interface AuthLibrary
    {
        object AuthImplicit(string projectId);
        object AuthExplicit(string projectId, string jsonPath);
        object AuthExplicitComputeEngine(string projectId);
    }
}
