// --------------------------------------------------------------------------------------------------
// <copyright file = "TwilioEvent.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Copyright (c) 2013 - 2015 Nino Crudele
//    Blog: http://ninocrudele.me
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License. 
// </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.SDK.TwilioEvent
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Twilio;

    /// <summary>
    /// The twilio event.
    /// </summary>
    [EventContract("{A5765B22-4003-4463-AB93-EEB5C0C477FE}", "Twilio Event", "Twilio send text message", true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class TwilioEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the account sid.
        /// </summary>
        [EventPropertyContract("AccountSid", "AccountSid")]
        public string AccountSid { get; set; }

        /// <summary>
        /// Gets or sets the auth token.
        /// </summary>
        [EventPropertyContract("AuthToken", "AuthToken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the from.
        /// </summary>
        [EventPropertyContract("From", "From")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        [EventPropertyContract("To", "To")]
        public string To { get; set; }

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
        /// <exception cref="Exception">
        /// </exception>
        [EventActionContract("{5ABB263A-8B69-49F7-BC9E-802A0A81AA0B}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                var content = Encoding.UTF8.GetString(this.DataContext);
                var twilio = new TwilioRestClient(this.AccountSid, this.AuthToken);
                var text = content.Replace("\"", string.Empty).Replace("\\", string.Empty);
                twilio.SendMessage(this.From, this.To, text);

                // SetEventActionEvent(this, context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}