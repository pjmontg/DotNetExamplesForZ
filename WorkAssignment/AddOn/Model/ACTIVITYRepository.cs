// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="ACTIVITYRepository.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/branches/Release/81/WebApp/Plugin_WorkAssignment/Model/ACTIVITYRepository.cs $
//   $LastChangedRevision: 9794 $ 
//   $LastChangedDate: 2013-12-16 14:13:21 -0800 (Mon, 16 Dec 2013) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Provides access to activities from an Oracle database  
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
	/// ACTIVITY Repository
	/// </summary>
	public partial class ACTIVITYRepository : EFRepository<ACTIVITY>, IACTIVITYRepository
	{
        [InjectionMethod]
        public void Initialize(ILogger<ACTIVITYRepository> logger)
        {
            _logger = logger;

            _logger.Debug("ACTIVITYRepository::Initialize method.");
        }

        public List<ACTIVITY> GetAllByWoNum(string wonum, string areaFilter)
        {
            _logger.Debug("ACTIVITYRepository::GetAllByWoNum method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shop = {0}, areaFilter = {1}}",
                    wonum,
                    areaFilter);
            }

            List<ACTIVITY> result = null;
            try
            {
                var query = this.Where(x => x.CUSTOM_ATTR_STRING_21 == wonum
                    && x.DATA_FOUND_IND == "Y").Include("TECHNICIAN_ASSIGNMENT").Include("TECHNICIAN_ASSIGNMENT.TECHNICIAN");

                if (string.IsNullOrEmpty(areaFilter) == false)
                {
                    query = query.Where(x => x.AREA == areaFilter);
                }

                query = query.OrderBy(x => x.ACT_NUM);

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
                _logger.Trace("Output of  activities = {0}", ObjectSerializingHelper.Serialize(result));
            }

            return result;
        }

        public List<ACTIVITY> GetAllByShopIdAndDate(string shopId, DateTime? workdate)
        {
            _logger.Debug("ACTIVITYRepository::GetAllByShopIdAndDate method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shop = {0}, workdate = {1}}",
                    shopId,
                    workdate);
            }

            List<ACTIVITY> result = null;
            try
            {
                var query = this.Where(x => x.SHOP == shopId
                   && (x.REMAIN_EARLY_START.Value.Day == workdate.Value.Day)
                   && (x.REMAIN_EARLY_START.Value.Month == workdate.Value.Month)
                   && (x.REMAIN_EARLY_START.Value.Year == workdate.Value.Year)
                   && x.DATA_FOUND_IND == "Y" && x.DELETED_IND == "N").OrderBy(x => x.ACT_NUM);

              //  if (_logger.IsTraceEnabled == true)
            //    {
                    // Log SQL
                    string message = ((ObjectQuery)query).ToTraceString();
                    _logger.Info("SQL Statement: " + message);
               // }

                result = query.ToList();
            }
            catch (Exception ex)
            {
                throw new DataBaseException(ex.Message, ex);
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of  activities = {0}", ObjectSerializingHelper.Serialize(result));
            }

            return result;
        }

        public List<ACTIVITY> GetAllByShopId(string shopId)
        {
            _logger.Debug("ACTIVITYRepository::GetAll method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shop = {0}}",
                    shopId);
            }

            List<ACTIVITY> result = null;
            try
            {
                var query = this.Where(x => x.SHOP == shopId).OrderBy(x => x.ACT_NUM);

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
                _logger.Trace("Output of  activities = {0}", ObjectSerializingHelper.Serialize(result));
            }

            return result;

        }

        private static ILogger<ACTIVITYRepository> _logger = null;
	}
}