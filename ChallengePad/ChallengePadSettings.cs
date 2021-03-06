﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengePad
{
    public class ChallengePadSettings
    {
        public string DbConnectionString { get; set; } = Environment.GetEnvironmentVariable("DATABASE") ?? "Server=localhost; Port=5432;Database=ChallengePadDb;User Id = docker; Password=docker;Timeout=15;SslMode=Disable;";
        public string RedisConfiguration { get; set; } = Environment.GetEnvironmentVariable("REDIS") ?? "localhost";
        public string Title { get; set; } = Environment.GetEnvironmentVariable("TITLE") ?? "ChallengePad";
        public string PadPrefix { get; set; } = Environment.GetEnvironmentVariable("PAD_PREFIX") ?? "https://demo.codimd.org/ChallengePad";
        public string PadSuffix { get; set; } = Environment.GetEnvironmentVariable("PAD_SUFFIX") ?? "";
        public string? OAuthClientId { get; set; } = Environment.GetEnvironmentVariable("OAUTH_CLIENT_ID");
        public string? OAuthClientSecret { get; set; } = Environment.GetEnvironmentVariable("OAUTH_CLIENT_SECRET");
        public string? OAuthAuthorizationEndpoint { get; set; } = Environment.GetEnvironmentVariable("OAUTH_AUTHORIZATION_ENDPOINT");
        public string? OAuthTokenEndpoint { get; set; } = Environment.GetEnvironmentVariable("OAUTH_TOKEN_ENDPOINT");
        public string? OAuthUserInformationEndpoint { get; set; } = Environment.GetEnvironmentVariable("OAUTH_USER_INFORMATION_ENDPOINT");
        public string? OAuthScope { get; set; } = Environment.GetEnvironmentVariable("OAUTH_SCOPE");
        public string? GuestPSK { get; set; } = Environment.GetEnvironmentVariable("GUEST_PSK");

        public void ValidateSettings()
        {
            if (HasOAuthSettings && !HasCompleteOAuthSettings)
                throw new StartupException("OAuthSettings are incomplete");

            if (HasCompleteOAuthSettings || HasGuestPSKSettings)
                return;

            throw new StartupException("No authentication scheme defined");
        }

        public bool HasOAuthSettings =>
            OAuthClientId != null ||
            OAuthClientSecret != null ||
            OAuthAuthorizationEndpoint != null ||
            OAuthTokenEndpoint != null ||
            OAuthUserInformationEndpoint != null ||
            OAuthScope != null;

        public bool HasCompleteOAuthSettings =>
            OAuthClientSecret != null &&
            OAuthAuthorizationEndpoint != null &&
            OAuthTokenEndpoint != null &&
            OAuthUserInformationEndpoint != null &&
            OAuthScope != null;

        public bool HasGuestPSKSettings => GuestPSK != null;
    }
}
