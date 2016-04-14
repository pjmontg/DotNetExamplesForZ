// -------------------------------------------------------------
// <copyright company="Pipeline Group, Inc." file="WorkAssignService.cs">
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
//   $HeadURL: https://pipesvn.pipelinenetwork.com/svn/STP/Product/branches/Release/81/WebApp/Plugin_WorkAssignment/Services/WorkAssignService.cs $
//   $LastChangedRevision: 9794 $ 
//   $LastChangedDate: 2013-12-16 14:13:21 -0800 (Mon, 16 Dec 2013) $ 
//   $LastChangedBy: pmontgomery $ 
// </remarks>  
// <summary>  
//   Service for interacting with work assignments
// </summary> 
// -------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.ServicesAPI;
using Microsoft.Practices.Unity;
using Logging;
using Utilities;
using Plugin_WorkAssignment.ModelAPI;
using Unity.Extensions;
using Utilities.Web;
using Utilities.Extensions;
using ServicesAPI.ViewModels;
using System.Data;

namespace Plugin_WorkAssignment.Services
{
    public class WorkAssignService : IWorkAssignService
    {
        #region Public service methods
        /// <summary>
        /// Sets logger
        /// </summary>
        /// <param name="logger">Logger for logging</param>
        [InjectionMethod]
        public void Initialize(
            ILogger<WorkAssignService> logger)
        {
            _logger = logger;

            _logger.Debug("WorkAssignService::Initialize method.");
        }

        private static Dictionary<decimal, string> _techIdToNameMap;

        private static Dictionary<decimal, string> TechIdToNameMap
        {
            get
            {
                if (null == _techIdToNameMap)
                {
                    _techIdToNameMap = new Dictionary<decimal, string>();
                    ITECHNICIANRepository techRepo = GlobalUnityContainer.Container.Resolve<ITECHNICIANRepository>();
                    using (techRepo.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
                    {
                        List<TECHNICIAN> techs = techRepo.All().ToList();
                        foreach (TECHNICIAN tech in techs)
                        {
                            _techIdToNameMap.Add(tech.ID, tech.NAME);
                        }
                    }
                }
                return _techIdToNameMap;
            }
        }

        /// <summary>
        /// Get specified day's technician utilizations for a specific shop
        /// </summary>
        /// <param name="workDate">Date to look at technician's utilization</param>
        /// <param name="shop">Shop technician's belong to</param>
        /// <returns>Specified day's technician utilizations for a specific shop</returns>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        public List<VW_TECHNICIAN_UTILIZATION> GetTechUtilization(
            DateTime? workDate,
            string shop)
        {
            _logger.Debug("WorkAssignService::GetTechUtilization method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {workDate = {0}, shop = {1}}",
                    workDate,
                    shop);
            }

            List<VW_TECHNICIAN_UTILIZATION> results = null;
            IVW_TECHNICIAN_UTILIZATIONRepository repository = GlobalUnityContainer.Container.Resolve<IVW_TECHNICIAN_UTILIZATIONRepository>();
            using (repository.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                results = repository.GetAll(workDate, shop);
            }

            if (results == null)
            {
                results = new List<VW_TECHNICIAN_UTILIZATION>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of technician's utilizations = {0}", ObjectSerializingHelper.Serialize(results));
            }
            return results;
        }

        /// <summary>
        /// Gets sum of assigned hours and available hours for all technician's in specified shop
        /// during the week of the specified date
        /// </summary>
        /// <param name="workDate">Date to use to find week of data</param>
        /// <param name="shop">Shop technician's belong to</param>
        /// <returns>Sum of assigned hours and available hours for all technician's in specified shop
        /// during the week of the specified date</returns>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        public List<VW_TECHNICIAN_ASSIGN_AVAIL> GetTechSumAssignAvail(
            DateTime? workDate,
            string shop)
        {
            _logger.Debug("WorkAssignService::GetTechSumAssignAvail method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {workDate = {0}, shop = {1}}",
                    workDate,
                    shop);
            }

            List<VW_TECHNICIAN_ASSIGN_AVAIL> results = null;
            if (workDate.HasValue == true)
            {
                IVW_TECHNICIAN_ASSIGN_AVAILRepository repository = GlobalUnityContainer.Container.Resolve<IVW_TECHNICIAN_ASSIGN_AVAILRepository>();
                using (repository.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
                {
                    // Determine Monday day at midnight and determine Saturday day at midnight
                    // TODO: Not sure how to handle this for different cultures or different work weeks.  This assumes a Monday through Friday
                    // work week
                    results = repository.GetAll(workDate.Value.GetFirstDayOfThisWeek().AddDays(1), workDate.Value.GetLastDayOfThisWeek(), shop);
                }
            }

            if (results == null)
            {
                results = new List<VW_TECHNICIAN_ASSIGN_AVAIL>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of assigned and availabe hours for technicians for a given shop during a given week = {0}",
                    ObjectSerializingHelper.Serialize(results));
            }
            return results;
        }

        /// <summary>
        /// Sets if overtime is required for an activity or not
        /// </summary>
        /// <param name="activityID">Activity to set whether overtime is required or not</param>
        /// <param name="isOTRequired">True if overtime is required and false if that is not the case</param>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        public Status SetOTRequired(
            decimal activityID,
            bool isOTRequired)
        {
            _logger.Debug("WorkAssignService::SetOTRequired method.");

            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {activityID = {0}, isOTRequired = {1}}",
                    activityID,
                    isOTRequired);
            }

            Status status = new Status();
            DataModelAPI.IACTIVITYRepository repository = GlobalUnityContainer.Container.Resolve<DataModelAPI.IACTIVITYRepository>();
            using (repository.UnitOfWork = GlobalUnityContainer.Container.Resolve<DataModelAPI.IUnitOfWork>())
            {
                DataModelAPI.ACTIVITY act = repository.GetActivity(activityID, null);
                if (act == null)
                {
                    // If not activity, this means that some other user removed the activity
                    status.Type = Status.StatusType.Concurrency;
                    status.Message = "Activity " + activityID + " does not exist anymore";
                    return status;
                }

                act.CUSTOM_ATTR_BOOL_5 = isOTRequired;

                try
                {
                    repository.UnitOfWork.Commit();
                }
                catch (UpdateException ex)
                {
                    status.Type = Status.StatusType.Concurrency;
                    Exception currentException = ex;
                    while (currentException.InnerException != null)
                    {
                        currentException = currentException.InnerException;
                    }
                    status.DetailedMessage = currentException.Message;
                    status.Message = "Cannot set if overtime is required or not into activity" + activityID;
                }
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of status = {0}",
                    ObjectSerializingHelper.Serialize(status));
            }
            return status;
        }

        public List<TECHNICIAN> GetTechnicians(string shopId)
        {
            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shopId = {0}}",
                    shopId);
            }

            List<TECHNICIAN> results = null;
            ITECHNICIANRepository repository = GlobalUnityContainer.Container.Resolve<ITECHNICIANRepository>();
            using (repository.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                results = repository.GetAllByShopId(shopId);
            }

            if (results == null)
            {
                results = new List<TECHNICIAN>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of technician's utilizations = {0}", ObjectSerializingHelper.Serialize(results));
            }
            return results;

        }

        public IList<ACTIVITY> GetActivityAssignments(string shopId)
        {
            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shopId = {0}}",
                    shopId);
            }

            IList<ACTIVITY> results = null;

            IACTIVITYRepository actRepo = GlobalUnityContainer.Container.Resolve<IACTIVITYRepository>();

            using (actRepo.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                results = actRepo.GetAllByShopId(shopId);
            }

            if (results == null)
            {
                results = new List<ACTIVITY>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of activities = {0}", ObjectSerializingHelper.Serialize(results));
            }

            return results;
        }

        public IList<ACTIVITY> GetAllActivityAssignments()
        {
            IList<ACTIVITY> results = null;

            IACTIVITYRepository actRepo = GlobalUnityContainer.Container.Resolve<IACTIVITYRepository>();

            using (actRepo.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                results = actRepo.All().ToList();
            }

            if (results == null)
            {
                results = new List<ACTIVITY>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of activities = {0}", ObjectSerializingHelper.Serialize(results));
            }

            return results;
        }

        public void UpdateActivityRemainingEarlyStartDate(string actId, string shopId, DateTime? newDate)
        {
            IACTIVITYRepository actRepo = GlobalUnityContainer.Container.Resolve<IACTIVITYRepository>();
            ITECHNICIAN_ASSIGNMENTRepository techRepo = GlobalUnityContainer.Container.Resolve<ITECHNICIAN_ASSIGNMENTRepository>();

           
             using (IUnitOfWork uow = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                actRepo.UnitOfWork = uow;
                techRepo.UnitOfWork = uow;

                decimal actIdDec = Convert.ToDecimal(actId);
                ACTIVITY act = actRepo.Where(x => x.ID == actIdDec).FirstOrDefault();
                if (act != null)
                {

                    IList<TECHNICIAN_ASSIGNMENT> techAssignments = techRepo.GetAssignments(actIdDec, shopId, act.REMAIN_EARLY_START.Value);
                    act.REMAIN_EARLY_START = newDate;
                    actRepo.Update(act);

                    foreach (TECHNICIAN_ASSIGNMENT assignment in techAssignments)
                    {
                        assignment.WORK_DATE = newDate.Value;
                        techRepo.Update(assignment);
                    }

                    uow.Commit();
                }
            }
        }

        public IList<ACTIVITY> GetActivityAssignmentsByWorkOrder(string shopId, DateTime? workdate, string areaFilter)
        {
            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shopId = {0}, workdate = {1}, areaFilter = {2}}",
                    shopId,
                    workdate,
                    areaFilter);
            }

            List<ACTIVITY> results = new List<ACTIVITY>();

            IACTIVITYRepository actRepo = GlobalUnityContainer.Container.Resolve<IACTIVITYRepository>();

            using (actRepo.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                List<ACTIVITY> initialResults = actRepo.GetAllByShopIdAndDate(shopId, workdate);

                HashSet<string> wonums = new HashSet<string>();

                foreach (ACTIVITY act in initialResults)
                {
                    wonums.Add(act.CUSTOM_ATTR_STRING_21);
                }

                foreach (string wonum in wonums)
                {
                    List<ACTIVITY> additionalResults = actRepo.GetAllByWoNum(wonum, areaFilter);
                    results.AddRange(additionalResults);
                }


            }

           

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of activities = {0}", ObjectSerializingHelper.Serialize(results));
            }

            return results;
        }


        public IList<VW_TECHNICIAN_AVAILABILITY> GetAvailableTechs(string shopId, DateTime workdate)
        {
            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {shopId = {0}, workdate = {1}}",
                    shopId,
                    workdate);
            }

            IList<VW_TECHNICIAN_AVAILABILITY> results = null;

            IVW_TECHNICIAN_AVAILABILITYRepository techAvailRepo = GlobalUnityContainer.Container.Resolve<IVW_TECHNICIAN_AVAILABILITYRepository>();

            using (techAvailRepo.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                results = techAvailRepo.GetAll(shopId, workdate);
            }

            if (results == null)
            {
                results = new List<VW_TECHNICIAN_AVAILABILITY>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of GetAvailableTechs = {0}", ObjectSerializingHelper.Serialize(results));
            }

            return results;
        }

        public IList<TECHNICIAN_ASSIGNMENT> GetActivityAssignmentsByActIdAndShop(string activityId, string shopId, DateTime workDate)
        {
            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {activityId = {0}, shopId = {1}, workDate = {2}}",
                    activityId,
                    shopId,
                    workDate);
            }

            IList<TECHNICIAN_ASSIGNMENT> results = null;

            ITECHNICIAN_ASSIGNMENTRepository repo = GlobalUnityContainer.Container.Resolve<ITECHNICIAN_ASSIGNMENTRepository>();

            using (repo.UnitOfWork = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                results = repo.GetAssignments(Convert.ToDecimal(activityId), shopId, workDate);
            }

            if (results == null)
            {
                results = new List<TECHNICIAN_ASSIGNMENT>();
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of GetAvailableTechs = {0}", ObjectSerializingHelper.Serialize(results));
            }

            return results;
        }

        public void UpdateTechAssignments(IList<TECHNICIAN_ASSIGNMENT> currentAssignments,
            string activityId, string shopId, DateTime workDate)
        {
            if (_logger.IsDebugEnabled == true)
            {
                _logger.Debug(
                    "Call parameters {activityId = {0}, shopId = {1}, workDate = {2}}",
                    activityId,
                    shopId,
                    workDate);
            }

            ITECHNICIAN_ASSIGNMENTRepository repository = GlobalUnityContainer.Container.Resolve<ITECHNICIAN_ASSIGNMENTRepository>();
           
            IACTIVITYRepository actRepo = GlobalUnityContainer.Container.Resolve<IACTIVITYRepository>();
            using (IUnitOfWork oUOW = GlobalUnityContainer.Container.Resolve<IUnitOfWork>())
            {
                repository.UnitOfWork = oUOW;
                actRepo.UnitOfWork = oUOW;
               // techRepo.UnitOfWork = oUOW;

                IList<TECHNICIAN_ASSIGNMENT> unreferencedAssignments = repository.GetAssignments(Convert.ToDecimal(activityId), shopId, workDate);

                DateTime now = DateTime.Now;

                
                List<string> techNames = new List<string>();
                foreach (TECHNICIAN_ASSIGNMENT assignment in currentAssignments) {
                    TECHNICIAN_ASSIGNMENT techAssignment = repository.GetAssignment(assignment.ACTIVITY_ID, assignment.TECHNICIAN_ID, assignment.WORK_DATE);
                    if (null == techAssignment)
                    {
                        assignment.CREATED_BY = Profile.Current.UserName;
                        assignment.CREATED_DATE = now;
                        assignment.LAST_MODIFIED_BY = Profile.Current.UserName;
                        assignment.LAST_MODIFIED_DATE = now;

                       repository.Add(assignment);
                    } else {
                        techAssignment.LAST_MODIFIED_BY = Profile.Current.UserName;
                        techAssignment.LAST_MODIFIED_DATE = now;
                        techAssignment.ASSIGNED_HRS = assignment.ASSIGNED_HRS;
                        repository.Update(techAssignment);
                        TECHNICIAN_ASSIGNMENT existingAssignment = unreferencedAssignments.FirstOrDefault(x => x.TECHNICIAN_ID == techAssignment.TECHNICIAN_ID);
                        unreferencedAssignments.Remove(existingAssignment);
                    }

                    techNames.Add(TechIdToNameMap[assignment.TECHNICIAN_ID]);

                }

               foreach (TECHNICIAN_ASSIGNMENT unrefAssignment in unreferencedAssignments) {
                    repository.Delete(unrefAssignment);
               }

                decimal actId = Convert.ToDecimal(activityId);
                ACTIVITY updatedAct = actRepo.Where(x => x.ID == actId).FirstOrDefault();
               string techs = string.Join(", ", techNames);
               updatedAct.CUSTOM_ATTR_STRING_26 = techs;
               actRepo.Update(updatedAct);
                
               oUOW.Commit();

                


            }
        }
        #endregion

        #region Private static variables
        /// <summary>
        /// Logger for logging
        /// </summary>
        private static ILogger<WorkAssignService> _logger = null;
        #endregion
    }
}