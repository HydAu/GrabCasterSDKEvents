#region License
// -----------------------------------------------------------------------
// Copyright (c) Antonino Crudele.  All Rights Reserved.  
// This work is registered with the UK Copyright Service.
// Registration No:284695248
// Licensed under the Reciprocal Public License 1.5 (RPL1.5) 
// See License.txt in the project root for license information.
// -----------------------------------------------------------------------
#endregion
namespace Events
{
    using Framework.Contracts;
    using Framework.Contracts.Globals;

    [EventContract("{2D239B2E-4489-49CC-A6C5-880081D8DA6C}", "Run Process", "Run Process", true)]
    public class RunProcessEvent : IEventType
    {
        [EventPropertyContract("ProcessFileNamePath", "Specify the process file name and path.")]
        public string ProcessFileNamePath { get; set; }

        public EventActionContext context { get; set; }
        public SetEventActionEvent SetEventActionEvent { get; set; }

        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        [EventActionContract("{C711BAB4-1DD6-41C2-8529-7E73C42A19C5}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent SetEventActionEvent, EventActionContext context)
        {
            try
            {
            }

            catch
            {
            }
        }
    }
}