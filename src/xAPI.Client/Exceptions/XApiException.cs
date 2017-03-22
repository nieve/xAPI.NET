﻿using System;

namespace xAPI.Client.Exceptions
{
    /// <summary>
    /// The base class for exceptions generated by the xAPI client. Note that
    /// other exceptions can occur (mainly: Json.NET or network-related exceptions).
    /// </summary>
    public abstract class XApiException : Exception
    {
        /// <summary>
        /// Creates a new instance of XApiException.
        /// </summary>
        /// <param name="message">The exception's message.</param>
        public XApiException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of XApiException.
        /// </summary>
        /// <param name="message">The exception's message.</param>
        /// <param name="innerException">The exception's inner exception.</param>
        public XApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}