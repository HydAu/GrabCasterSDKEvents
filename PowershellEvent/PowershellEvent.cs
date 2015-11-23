// --------------------------------------------------------------------------------------------------
// <copyright file = "PowershellEvent.cs" company="Nino Crudele">
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
namespace GrabCaster.SDK.PowershellEvent
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The PowerShell event.
    /// </summary>
    [EventContract("{F9A0B69C-64D3-4120-A52D-09D2E014EA91}", "Execute a Powershell Event", "Execute a Powershell Event",
        true)]
    public class PowershellEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the script event.
        /// </summary>
        [EventPropertyContract("ScriptEvent", "Script to execute")]
        public string ScriptEvent { get; set; }

        /// <summary>
        /// Gets or sets the script file event.
        /// </summary>
        [EventPropertyContract("ScriptFileEvent", "Script from file")]
        public string ScriptFileEvent { get; set; }

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
        [EventActionContract("{979A5EE0-C029-4518-98C2-CFB4526F2C86}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                var script = string.Empty;
                if (!string.IsNullOrEmpty(this.ScriptFileEvent))
                {
                    script = File.ReadAllText(this.ScriptFileEvent);
                }
                else
                {
                    script = this.ScriptEvent;
                }

                var powerShellScript = PowerShell.Create();
                powerShellScript.AddScript(script);

                // TODO 1020
                powerShellScript.AddParameter("DataContext", this.DataContext);
                powerShellScript.Invoke();
                var outVar = powerShellScript.Runspace.SessionStateProxy.PSVariable.GetValue("DataContext");
                if (outVar != null)
                {
                    this.DataContext = Encoding.UTF8.GetBytes(outVar.ToString());
                    setEventActionEvent(this, context);
                }

                // TODO 1030
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}