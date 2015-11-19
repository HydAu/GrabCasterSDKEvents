// --------------------------------------------------------------------------------------------------
// <copyright file = "AzureBlobEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.SDK.AzureBlobEvent
{
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Handles the Azure Blob event.
    /// </summary>
    [EventContract("{C185F004-62E4-45A4-97B1-BD0D382FFE33}", "Azure Blob Event", "Show a messagebox", true)]
    public class AzureBlobEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the Azure storage account.
        /// </summary>
        /// <value>
        /// The Azure storage account.
        /// </value>
        [EventPropertyContract("StorageAccount", "Azure StorageAccount")]
        public string StorageAccount { get; set; }

        /// <summary>
        /// Gets or sets the Azure BLOB container.
        /// </summary>
        /// <value>
        /// The Azure BLOB container.
        /// </value>
        [EventPropertyContract("BlobContainer", "Azure Blob Container")]
        public string BlobContainer { get; set; }

        /// <summary>
        /// Gets or sets the Azure BLOB block reference.
        /// </summary>
        /// <value>
        /// The Azure BLOB block reference.
        /// </value>
        [EventPropertyContract("BlobBlockReference", "Azure Blob BlockReference")]
        public string BlobBlockReference { get; set; }

        /// <summary>
        /// Gets or sets the event context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        /// <value>
        /// The set event action event.
        /// </value>
        public SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
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
        [EventActionContract("{346FC437-8464-4566-8AD6-A7E4B29A7EBC}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                this.Context = context;
                this.SetEventActionEvent = setEventActionEvent;

                var storageAccount = CloudStorageAccount.Parse(this.StorageAccount);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(this.BlobContainer);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                var blockBlob = container.GetBlockBlobReference(this.BlobBlockReference);
                blockBlob.UploadFromByteArray(this.DataContext, 0, this.DataContext.Length);

                setEventActionEvent(this, context);
            }
            catch
            {
                // ignored
            }
        } // Execute
    } // AzureBlobEvent
} // namespace