using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationConfig.Enums
{
    public enum CommunicationMode
    {
        AkkaNet = 1,
        AzureEventBus = 2,
        AzureServiceBus = 3,
        Dapr = 4,
        gRPC = 5,
        Kafka = 6,
        NServiceBus = 7,
        Refit = 8,
        SagaWorkFlow = 9,
        OData = 10
    }
}
