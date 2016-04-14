// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="RegisterPluginCommand.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/Command/RegisterPluginCommand.cs $
//   $LastChangedRevision: 10641 $ 
//   $LastChangedDate: 2014-02-19 15:14:06 -0800 (Wed, 19 Feb 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Register Plugin Command class
// </summary> 
// ------------------------------------------------------------- 


using System.Collections.Generic;
using System.Linq;
using ServicesAPI.Plugin;
using PluginAPI.Exception.Command;
using System.Data;
using Logging;
using Unity.Extensions;
using Microsoft.Practices.Unity;

namespace PluginAPI.Command
{
    internal class RegisterPluginCommand : IPluginInstallerCommand
    {
        public string PluginName { get; set; }
        public string PluginVersion { get; set; }

        public void CheckPreconditions(IPluginService PluginService)
        {
            if (null == PluginService || null == PluginName || null == PluginVersion)
            {
                throw new PluginInstallerCommandException("Command not properties have not been set.");
            }

            IList<PluginRegistration> registrations = null;
            try
            {
                registrations = PluginService.GetAllRegistrations();
            }
            catch (System.Exception ex)
            {
                ILogger<RegisterPluginCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<RegisterPluginCommand>>();
                logger.Fatal(ex);
                throw;
            }

            PluginRegistration existingReg = registrations.FirstOrDefault(x => x.Name == PluginName);

            if (null != existingReg)
            {
                throw new PluginInstallerCommandException("Plugin already exists");
            }
        }

        public void Execute(IPluginService PluginService)
        {
            PluginService.CreatePluginRegistration(new PluginRegistration
            {
                Name = PluginName,
                Version = PluginVersion,
                Status = PluginStatus.ENABLED
            });
        }

        public void Rollback(IPluginService PluginService)
        {
            try
            {
                PluginService.DeletePluginRegistration(new PluginRegistration
                {
                    Name = PluginName,
                    Version = PluginVersion
                });
            }
            catch (UpdateException ex)
            {
                ILogger<RegisterPluginCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<RegisterPluginCommand>>();
                logger.Warn(ex);
            }
            catch (System.Exception ex)
            {
                ILogger<RegisterPluginCommand> logger = GlobalUnityContainer.Container.Resolve<ILogger<RegisterPluginCommand>>();
                logger.Fatal(ex);
                throw;
            }
        }

        public void Validate(IPluginService PluginService)
        {
            IList<PluginRegistration> registrations = PluginService.GetAllRegistrations();
            PluginRegistration existingReg = registrations.FirstOrDefault(x => x.Name == PluginName && x.Version == PluginVersion);

            if (null == existingReg)
            {
                throw new PluginInstallerCommandException("Plugin registration did not complete correctly");
            }
        }
    }
}
