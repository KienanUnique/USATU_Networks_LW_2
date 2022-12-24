using ChatLibrary;

namespace SuperSimpleTcp
{
    /// <summary>
    /// Arguments for authentication error events
    /// </summary>
    public class AuthenticationErrorEventArgs
    {
        internal AuthenticationErrorEventArgs(string serverIpPort, AnswerOnAuthenticationTypes authenticationError)
        {
            IpPort = serverIpPort;
            AuthenticationError = authenticationError;
        }

        /// <summary>
        /// The IP address and port number of the server.
        /// </summary>
        public string IpPort { get; }

        /// <summary>
        /// The reason for the AuthenticationError.
        /// </summary>
        public AnswerOnAuthenticationTypes AuthenticationError { get; }
    }
}