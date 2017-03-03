﻿using Maskott.xAPI.Client.Resources;
using System;
using System.Configuration;

namespace Maskott.xAPI.Client.Tests
{
    public static class Config
    {
        public static bool MockHttpClient
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["MockHttpClient"];
                return bool.Parse(setting);
            }
        }

        public static Uri EndpointUri
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["EndpointUri"];
                return new Uri(setting);
            }
        }

        public static XApiVersion Version
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["Version"];
                return XApiVersion.Parse(setting);
            }
        }

        public static string BasicUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["BasicUsername"];
            }
        }

        public static string BasicPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["BasicPassword"];
            }
        }

        public static string OAuthClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["OAuthClientId"];
            }
        }

        public static string OAuthClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["OAuthClientSecret"];
            }
        }
    }
}