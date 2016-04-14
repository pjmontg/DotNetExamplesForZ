// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="IWorkAssignService.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/branches/Release/81/WebApp/Plugin_WorkAssignment/ServicesAPI/IWorkAssignService.cs $
//   $LastChangedRevision: 8541 $ 
//   $LastChangedDate: 2013-09-10 09:44:22 -0700 (Tue, 10 Sep 2013) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Interface for interacting with work assignments
// </summary> 
// -------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.ModelAPI;
using ServicesAPI.ViewModels;

namespace Plugin_WorkAssignment.ServicesAPI
{
    public interface IWorkAssignService
    {
        #region Service methods
        /// <summary>
        /// Get specified day's technician utilizations for a specific shop
        /// </summary>
        /// <param name="workDate">Date to look at technician's utilization</param>
        /// <param name="shop">Shop technician's belong to</param>
        /// <returns>Specified day's technician utilizations for a specific shop</returns>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        List<VW_TECHNICIAN_UTILIZATION> GetTechUtilization(
            DateTime? workDate,
            string shop);

        /// <summary>
        /// Gets sum of assigned hours and available hours for all technician's in specified shop
        /// during the week of the specified date
        /// </summary>
        /// <param name="workDate">Date to use to find week of data</param>
        /// <param name="shop">Shop technician's belong to</param>
        /// <returns>Sum of assigned hours and available hours for all technician's in specified shop
        /// during the week of the specified date</returns>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        List<VW_TECHNICIAN_ASSIGN_AVAIL> GetTechSumAssignAvail(
            DateTime? workDate,
            string shop);

        /// <summary>
        /// Sets if overtime is required for an activity or not
        /// </summary>
        /// <param name="activityID">Activity to set whether overtime is required or not</param>
        /// <param name="isOTRequired">True if overtime is required and false if that is not the case</param>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        Status SetOTRequired(
            decimal activityID,
            bool isOTRequired);

        List<TECHNICIAN> GetTechnicians(string shopId);

        IList<ACTIVITY> GetActivityAssignments(string shopId);

        IList<ACTIVITY> GetAllActivityAssignments();

        IList<ACTIVITY> GetActivityAssignmentsByWorkOrder(string shopId, DateTime? workdate, string areaFilter);

        IList<VW_TECHNICIAN_AVAILABILITY> GetAvailableTechs(string shopId, DateTime workdate);

        
        #endregion

        IList<TECHNICIAN_ASSIGNMENT> GetActivityAssignmentsByActIdAndShop(string activityId, string shopId, DateTime workDate);

        void UpdateTechAssignments(IList<TECHNICIAN_ASSIGNMENT> currentAssignments, string activityId, string shopId, DateTime workDate);

        void UpdateActivityRemainingEarlyStartDate(string actId, string shopId, DateTime? newDate);
    }
}