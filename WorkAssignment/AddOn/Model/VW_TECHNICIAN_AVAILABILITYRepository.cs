// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="VW_TECHNICIAN_AVAILABILITYRepository.cs">
// COPYRIGHT NOTICE

// SOFTWARE CONTAINING TRADE SECRETS

// Copyright 2012 Pipeline Group, Inc.  (PIPELINE GROUP, INC ). All rights reserved.

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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/branches/Release/81/WebApp/Plugin_WorkAssignment/Model/VW_TECHNICIAN_AVAILABILITYRepository.cs $
//   $LastChangedRevision: 9794 $ 
//   $LastChangedDate: 2013-12-16 14:13:21 -0800 (Mon, 16 Dec 2013) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Provides access to technician's availability from an Oracle database  
// </summary> 
// ------------------------------------------------------------- 

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Collections.Generic;
using Plugin_WorkAssignment.ModelAPI;
using Microsoft.Practices.Unity;
using Logging;
using System.Data.Objects;
using Utilities.ErrorHandling;
using Utilities;
	
namespace Plugin_WorkAssignment.Model
{   
	/// <summary>
	/// VW_TECHNICIAN_AVAILABILITY Repository
	/// </summary>
	public partial class VW_TECHNICIAN_AVAILABILITYRepository : EFRepository<VW_TECHNICIAN_AVAILABILITY>, IVW_TECHNICIAN_AVAILABILITYRepository
	{

        [InjectionMethod]
        public void Initialize(ILogger<VW_TECHNICIAN_AVAILABILITYRepository> logger)
        {
            _logger = logger;

            _logger.Debug("VW_TECHNICIAN_UTILIZATIONRepository::Initialize method.");
        }


        public IList<VW_TECHNICIAN_AVAILABILITY> GetAll(string shopId, DateTime workdate)
        {
            _logger.Debug("VW_TECHNICIAN_AVAILABILITYRepository::GetAll method.");

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace(
                    "Call parameters {shop = {0}, workdate = {1}}",
                    shopId,
                    workdate);
            }

            List<VW_TECHNICIAN_AVAILABILITY> result = null;
            try
            {
                var query = this.Where(x => x.SHOP == shopId
                    && (x.WORK_DATE.Day == workdate.Day)
                    && (x.WORK_DATE.Month == workdate.Month)
                    && (x.WORK_DATE.Year == workdate.Year)
                   );

                if (_logger.IsTraceEnabled == true)
                {
                    // Log SQL
                    string message = ((ObjectQuery)query).ToTraceString();
                    _logger.Trace("SQL Statement: " + message);
                }

                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, ex);
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of  available techs = {0}", ObjectSerializingHelper.Serialize(result));
            }

            return result;
        }

        private static ILogger<VW_TECHNICIAN_AVAILABILITYRepository> _logger = null;
	}
}