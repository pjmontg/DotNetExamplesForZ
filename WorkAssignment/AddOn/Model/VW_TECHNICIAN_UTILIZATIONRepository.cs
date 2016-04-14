// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="VW_TECHNICIAN_UTILIZATIONRepository.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/branches/Release/81/WebApp/Plugin_WorkAssignment/Model/VW_TECHNICIAN_UTILIZATIONRepository.cs $
//   $LastChangedRevision: 9794 $ 
//   $LastChangedDate: 2013-12-16 14:13:21 -0800 (Mon, 16 Dec 2013) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Provides access to technician's utilizaiton from an Oracle database  
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
using Utilities;
using Utilities.ErrorHandling;
using System.Data.Objects;
	
namespace Plugin_WorkAssignment.Model
{   
	/// <summary>
	/// VW_TECHNICIAN_UTILIZATION Repository
	/// </summary>
	public partial class VW_TECHNICIAN_UTILIZATIONRepository : EFRepository<VW_TECHNICIAN_UTILIZATION>, IVW_TECHNICIAN_UTILIZATIONRepository
	{
        #region Public methods
        /// <summary>
        /// Sets logger
        /// </summary>
        /// <param name="logger">Logger for logging</param>
        [InjectionMethod]
        public void Initialize(ILogger<VW_TECHNICIAN_UTILIZATIONRepository> logger)
        {
            _logger = logger;

            _logger.Debug("VW_TECHNICIAN_UTILIZATIONRepository::Initialize method.");
        }

        /// <summary>
        /// Get all technician's utilization based on work date and shop technician belongs to
        /// </summary>
        /// <param name="workDate">Date to look at technician's utilization</param>
        /// <param name="shop">Shop technician's belong to</param>
        /// <returns>All technician's utilization based on work date and shop technician belongs to</returns>
        /// <exception cref="DataBaseException">Database error getting data</exception>
        public List<VW_TECHNICIAN_UTILIZATION> GetAll(DateTime? workDate, string shop)
        {
            _logger.Debug("VW_TECHNICIAN_UTILIZATIONRepository::GetAll method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {workDate = {0}, shop = {1}}",
                    workDate,
                    shop);
            }

            List<VW_TECHNICIAN_UTILIZATION> result = null;

            if ((string.IsNullOrEmpty(shop) == true) || (workDate.HasValue == false))
            {
                // The above should never occur since there always needs to be a shop and date
                return null;
            }

            try
            {
                DateTime date = workDate.Value.Date;
                var query = this.Where(techUtil =>
                    EntityFunctions.TruncateTime(techUtil.WORK_DATE) == EntityFunctions.TruncateTime(workDate) 
                    && techUtil.SHOP == shop);

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
                _logger.Trace("Output of technician's utilizations = {0}", ObjectSerializingHelper.Serialize(result));
            }

            return result;
        }
        #endregion

        #region Private static variables
        /// <summary>
        /// Logger for logging
        /// </summary>
        private static ILogger<VW_TECHNICIAN_UTILIZATIONRepository> _logger = null;
        #endregion
    }
}