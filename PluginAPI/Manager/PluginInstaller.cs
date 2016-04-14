// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="PluginInstaller.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Manager/PluginInstaller.cs $
//   $LastChangedRevision: 10641 $ 
//   $LastChangedDate: 2014-02-19 15:14:06 -0800 (Wed, 19 Feb 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Plugin Installer
// </summary> 
// -------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Reflection;
using ServicesAPI.Plugin;
using PluginAPI.Exception;
using Logging;
using PluginAPI.Command;
using Utilities.ErrorHandling;

namespace PluginAPI.Manager
{
    internal class PluginInstaller
    {
        private string _filename;
        private string _installPath;
        private string _applicationVersion;

        private IPluginService _pluginService;

        internal PluginInstaller(string filename, IPluginService pluginService, string installPath, string applicationVersion, ILogger<IPluginManager> logger)
        {
            _logger = logger;
            _filename = filename;
            _pluginService = pluginService;
            _installPath = installPath;
            _applicationVersion = applicationVersion;

        }
#pragma warning disable 0649
        [Import]
        private Lazy<IPlugin, IPluginMetadata> _plugin;
#pragma warning restore 0649
        internal void Install(bool isEnabled)
        {
            try
            {
                Assembly uploadedAssembly = Assembly.LoadFrom(_installPath + "\\" + _filename);

                AggregateCatalog catalog = new AggregateCatalog();
                AssemblyCatalog asmCatalog = new AssemblyCatalog(uploadedAssembly);


                asmCatalog.Parts.ToArray();
                catalog.Catalogs.Add(new AssemblyCatalog(uploadedAssembly));
                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(this);

                if (null != _plugin)
                {
                    List<IPluginInstallerCommand> commands = new List<IPluginInstallerCommand>(_plugin.Value.InstallerCommands);

                    /*
                    commands.Insert(0, new RequiresVersionPreconditionCheckCommand
                    {
                        ApplicationVersion = _applicationVersion,
                        RequiresVersion = _plugin.Metadata.RequiresVersion

                    });
                    */

                    commands.Insert(0, new RegisterPluginCommand
                    {
                        PluginName = _plugin.Metadata.UniqueName,
                        PluginVersion = _plugin.Metadata.Version
                    });

                    this.ExecuteInstallCommands(commands);

                }

                container.Dispose();
            }
            catch (ReflectionTypeLoadException ex)
            {
                _logger.Error("Error loading plugin: " + _filename);
                _logger.Error(ex);
                foreach (System.Exception loaderEx in ex.LoaderExceptions)
                {
                    _logger.Error(loaderEx);
                }
                throw new PluginInstallerException(ex.Message, ex);

            }
            catch (DataRepositoryException)
            {
                // This should not be converted into a PluginInstallerException since
                // this should be get in the end by the server error mechanism to display
                // an error page
                throw;
            }
            catch (System.Exception e)
            {
                throw new PluginInstallerException(e.Message, e);
            }
        }

        private void ExecuteInstallCommands(List<IPluginInstallerCommand> commands)
        {
            Stack<IPluginInstallerCommand> executedCommands = new Stack<IPluginInstallerCommand>();

            foreach (IPluginInstallerCommand command in commands)
            {
                try
                {
                    command.CheckPreconditions(_pluginService);
                    command.Execute(_pluginService);
                    executedCommands.Push(command);
                    command.Validate(_pluginService);
                }
                catch (DataRepositoryException dre)
                {
                    _logger.Fatal(dre);
                    throw;
                }
                catch (System.Exception e)
                {
                    while (executedCommands.Count > 0)
                    {
                        IPluginInstallerCommand executedCommand = executedCommands.Pop();
                        executedCommand.Rollback(_pluginService);
                    }
                    throw new PluginInstallerException(e.Message, e);
                }
            }
        }

       
    

        private ILogger<IPluginManager> _logger;
    }
}
