// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="PluginUninstaller.cs">
// COPYRIGHT NOTICE

// SOFTWARE CONTAINING TRADE SECRETS

// Copyright 2014 Pipeline Group, Inc.  (PIPELINE GROUP, INC ). All rights reserved.

// This software and documentation constitute an unpublished work and contain valuable
// trade secrets and proprietary information belonging to the PIPELINE GROUP, INC .
// None of the foregoing material may be copied, duplicated or disclosed without the 
// express written permission of the PIPELINE GROUP, INC .

// PIPELINE GROUP, INC  EXPRESSLY DISCLAIMS ANY AND ALL WARRANTIES CONCERNING THIS 
// SOFTWARE AND DOCUMENTATION, INCLUDING ANY WARRANTIES OF MERCHANTABILITY AND/OR FITNESS
// FOR ANY PARTICULAR PURPOSE, AND WARRANTIES OF PERFORMANCE, AND ANY WARRANTY THAT MIGHT 
// OTHERWISE ARISE FROM COURSE OF DEALING OR USAGE OF TRADE.
// NO WARRANTY IS EITHER EXPRESS OR IMPLIED WITH RESPECT TO THE USE OF THE SOFTWARE OR 
// DOCUMENTATION. 

// Under no circumstances shall PIPELINE GROUP, INC  be liable for incidental, special, indirect, direct 
// or consequential damages or loss of profits, interruption of business, or related expenses 
// which may arise from use of software or documentation, including but not limited to those 
// resulting from defects in software and/or documentation, or loss or inaccuracy of data 
// of any kind.

// </copyright>
// <author>$Author: pmontgomery $</author>
// <remarks>
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Manager/PluginUninstaller.cs $
//   $LastChangedRevision: 10641 $ 
//   $LastChangedDate: 2014-02-19 15:14:06 -0800 (Wed, 19 Feb 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Class for Plugin Uninstaller
// </summary> 
// -------------------------------------------------------------


using System;
using System.Collections.Generic;
using ServicesAPI.Plugin;
using Logging;
using PluginAPI.Command;

namespace PluginAPI.Manager
{
    internal class PluginUninstaller
    {
        private IPluginService _pluginService;
        private ILogger<IPluginManager> _logger;
        Lazy<IPlugin, IPluginMetadata> _plugin;

        internal PluginUninstaller(Lazy<IPlugin, IPluginMetadata> plugin, IPluginService pluginService, ILogger<IPluginManager> logger)
        {
            _logger = logger;
           
            _pluginService = pluginService;
            _plugin = plugin;
        }

        internal void Uninstall()
        {
            if (null != _plugin)
            {
                List<IPluginInstallerCommand> commands = new List<IPluginInstallerCommand>(_plugin.Value.InstallerCommands);

                commands.Insert(0, new RegisterPluginCommand
                {
                    PluginName = _plugin.Metadata.UniqueName,
                    PluginVersion = _plugin.Metadata.Version
                });

                RollbackAllCommands(commands);
            }
        }

        private void RollbackAllCommands(List<IPluginInstallerCommand> commands)
        {
            Stack<IPluginInstallerCommand> executedCommands = new Stack<IPluginInstallerCommand>();

            foreach (IPluginInstallerCommand command in commands)
            {
                executedCommands.Push(command);
            }

            foreach (IPluginInstallerCommand command in executedCommands)
            {
                command.Rollback(_pluginService);
            }
        }
    }
}
