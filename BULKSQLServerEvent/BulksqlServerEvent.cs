// --------------------------------------------------------------------------------------------------
// <copyright file = "BulksqlServerEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.SDK.BULKSQLServerEvent
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Serialization;

    /// <summary>
    /// The bulksql server event.
    /// </summary>
    [EventContract("{767D579B-986B-47B1-ACDF-46738434043F}", "BulksqlServerEvent Event", "Receive a Sql Server recordset to perform a bulk insert.",
        true)]
    public class BulksqlServerEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the table name destination.
        /// </summary>
        [EventPropertyContract("TableNameDestination", "TableName")]
        public string TableNameDestination { get; set; }

        /// <summary>
        /// Gets or sets the bulk select query destination.
        /// </summary>
        [EventPropertyContract("BulkSelectQueryDestination", "BulkSelectQueryDestination")]
        public string BulkSelectQueryDestination { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [EventPropertyContract("ConnectionString", "ConnectionString")]
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
        [EventActionContract("{F469BD5B-B352-40D6-BD33-591EF96E8F6C}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                using (var destinationConnection = new SqlConnection(this.ConnectionString))
                {
                    destinationConnection.Open();
                    using (var bulkCopy = new SqlBulkCopy(this.ConnectionString))
                    {
                        bulkCopy.DestinationTableName = this.TableNameDestination;
                        try
                        {
                            object obj = Serialization.ByteArrayToDataTable(this.DataContext);
                            var dataTable = (DataTable)obj;
                            
                            // Write from the source to the destination.
                            bulkCopy.WriteToServer(dataTable);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }

                setEventActionEvent(this, context);
            }
            catch
            {
                // ignored
            }
        }
    }
}