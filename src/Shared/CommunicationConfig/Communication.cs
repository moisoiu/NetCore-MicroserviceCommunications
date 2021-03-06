﻿using CommunicationConfig.Enums;
using CommunicationConfig.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CommunicationConfig
{
    public static class Communication
    {
        /// <summary>
        /// Checks Db for active communication mode
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns>This returns</returns>
        public static CommunicationMode GetCommunicationMode(IServiceCollection services, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationContext>();
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);


            using (var context = new ConfigurationContext(optionsBuilder.Options))
            {
                var communicationMode = context.Communication.FirstOrDefault(x => x.IsActive);
                if (communicationMode == null)
                {
                    throw new NoCommunicationModeActiveException("No active communication found, check Db");
                }

                if (!Enum.IsDefined(typeof(CommunicationMode), communicationMode.Id))
                {
                    throw new InvalidCastException($"No Enum defined for {communicationMode.Id}");
                }

                return (CommunicationMode)communicationMode.Id;
            }
        }
    }

    /// <summary>
    /// The exception that is thrown when no active communication mode is found
    /// </summary>
    internal class NoCommunicationModeActiveException : Exception
    {
        public NoCommunicationModeActiveException()
        {

        }

        public NoCommunicationModeActiveException(string message) : base(message)
        {
        }

        public NoCommunicationModeActiveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoCommunicationModeActiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
