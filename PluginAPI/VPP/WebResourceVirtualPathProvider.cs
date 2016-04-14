// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="WebResourceVirtualPathProvider.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/trunk/WebApp/PluginAPI/VPP/WebResourceVirtualPathProvider.cs $
//   $LastChangedRevision: 10641 $ 
//   $LastChangedDate: 2014-02-19 15:14:06 -0800 (Wed, 19 Feb 2014) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Class for Web Resource Virtual Path Provider
// </summary> 
// -------------------------------------------------------------


using System.Collections.Generic;
using System.Web.Hosting;
using System.Reflection;

namespace PluginAPI.VPP
{
    public class WebResourceVirtualPathProvider : VirtualPathProvider
    {
        private Assembly _assembly;
        private string _baseVirtualPath;
        private HashSet<string> _pagePaths;

        public WebResourceVirtualPathProvider(Assembly assembly, string baseVirtualPath, HashSet<string> pagePaths)
        {
            this._assembly = assembly;
            this._baseVirtualPath = baseVirtualPath;
            this._pagePaths = pagePaths;

        }
        public override bool FileExists(string virtualPath)
        {
            string trimmedBasePath = _baseVirtualPath;
            if (_baseVirtualPath[0] == '~')
            {
                trimmedBasePath = _baseVirtualPath.Substring(1);
            }
            if (virtualPath.Contains(trimmedBasePath))
            {
                int index = virtualPath.IndexOf(trimmedBasePath);
                virtualPath = virtualPath.Substring(index + trimmedBasePath.Length);

                if (_pagePaths.Contains(virtualPath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return Previous.FileExists(virtualPath);
            }

        }

        public override VirtualFile GetFile(string virtualPath)
        {
            string trimmedBasePath = _baseVirtualPath;
            if (_baseVirtualPath[0] == '~')
            {
                trimmedBasePath = _baseVirtualPath.Substring(1);
            }
            if (virtualPath.Contains(trimmedBasePath))
            {
                return new WebResourceVirtualFile(virtualPath, _baseVirtualPath, _assembly);
            }
            else
            {
                return Previous.GetFile(virtualPath);
            }


        }
    }
}
