// --------------------------------------------------------------------------------------------------
// <copyright file = "FileEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.SDK.FileEvent
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Serialization;

    /// <summary>
    /// The file event.
    /// </summary>
    [EventContract("{D438C746-5E75-4D59-B595-8300138FB1EA}", "Write File", "Write the content in a file in a specific folder.", true)]
    public class FileEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        [EventPropertyContract("OutputDirectory", "When the file has to be created")]
        public string OutputDirectory { get; set; }

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
        [EventActionContract("{1FBD0C6E-1A49-4BEF-8876-33A21B23C933}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                Debug.WriteLine("In FileEvent Event.");
                File.WriteAllBytes(this.OutputDirectory + Guid.NewGuid() + ".txt", this.DataContext);
                this.DataContext = Serialization.ObjectToByteArray(true);
                setEventActionEvent(this, context);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FileEvent error > " + ex.Message);
                setEventActionEvent(this, null);
            }
        }
    }
}