﻿namespace TuyaConnector.Data
{
    public interface IAccessToken
    {
        /// <summary>
        /// Gets or sets the token string.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        public long? ExpireTime { get; set; }
        public bool IsExpired();
    }
}
