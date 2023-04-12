﻿using NntpBase.Nntp.Models;
using NntpBase.Util;

namespace NntpBase.Extensions
{
    /// <summary>
    /// NntpMessageId extension methods.
    /// </summary>
    internal static class NntpMessageIdExtensions
    {
        /// <summary>
        /// Throws an <exception cref="ArgumentNullException"/> if the messageId of it's value is null.
        /// Throws an <exception cref="ArgumentException"/> if the value of the messageId is empty or 
        /// if it consists only of white-space characters.
        /// </summary>
        /// <param name="messageId">The messageId to check.</param>
        /// <param name="name">The name of the messageId.</param>
        /// <returns>The original <see cref="NntpMessageId"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static NntpMessageId ThrowIfNullOrWhiteSpace(this NntpMessageId messageId, string name)
        {
            Guard.ThrowIfNull(messageId, name);
            Guard.ThrowIfNullOrWhiteSpace(messageId.Value, name);
            return messageId;
        }
    }
}
