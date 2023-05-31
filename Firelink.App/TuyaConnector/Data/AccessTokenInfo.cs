using Newtonsoft.Json;

namespace TuyaConnector.Data
{
    /// <summary>
    /// Access Token Info DTO.
    /// </summary>
    public class AccessTokenInfo : IAccessToken, IIdentifiable
    {
        public AccessTokenInfo()
        {
            _issuedAt = DateTime.UtcNow;
        }
        private readonly DateTime _issuedAt;
        /// <summary>
        /// Gets or sets the token string.
        /// </summary>
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        [JsonProperty("expire_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the UID.
        /// </summary>
        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        public bool IsExpired()
        {
            return _issuedAt.AddHours(1) >= DateTime.UtcNow;
        }


    }
}
