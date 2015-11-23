// --------------------------------------------------------------------------------------------------
// <copyright file = "EventHubEvent.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
// The MIT License (MIT)
// 
// Copyright (c) 2013 - 2015 Nino Crudele
// Blog: http://ninocrudele.me
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.SDK.EventHubEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// The event hub event.
    /// </summary>
    [EventContract("{F249290E-0231-44A9-A348-1CC7FCC33C7F}", "Event Hub Event", "Send a message to Azure Event Hub.", true)]
    public class EventHubEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the event hub name.
        /// </summary>
        [EventPropertyContract("EventHubName", "EventHubName")]
        public string EventHubName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "Event Hub connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        public SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [EventActionContract("{FA452E1A-95E9-4076-A1EE-1B41E9561824}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                if (!InternalEventUpStream.InstanceLoaded)
                {
                    InternalEventUpStream.CreateEventUpStream(this.ConnectionString, this.EventHubName);
                    InternalEventUpStream.InstanceLoaded = true;
                }

                InternalEventUpStream.SendMessage(this.DataContext);
                setEventActionEvent(this, context);
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// The internal event up stream.
    /// </summary>
    internal static class InternalEventUpStream
    {
        /// <summary>
        /// The builder.
        /// </summary>
        private static ServiceBusConnectionStringBuilder builder;

        /// <summary>
        /// The event hub client.
        /// </summary>
        private static EventHubClient eventHubClient;

        /// <summary>
        /// The instance loaded.
        /// </summary>
        public static bool InstanceLoaded { get; set; }

        /// <summary>
        /// The create event up stream.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="eventHubName">
        /// The event hub name.
        /// </param>
        public static void CreateEventUpStream(string connectionString, string eventHubName)
        {
            try
            {
                builder = new ServiceBusConnectionStringBuilder(connectionString) { TransportType = TransportType.Amqp };
                eventHubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void SendMessage(byte[] message)
        {
            try
            {
                var data = new EventData(message);
                eventHubClient.SendAsync(data);
            }
            catch
            {
                // ignored
            }
        }
    }
}