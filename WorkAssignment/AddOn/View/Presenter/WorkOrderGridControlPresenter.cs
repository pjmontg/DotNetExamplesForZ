using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Plugin_WorkAssignment.ModelAPI;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.ServicesAPI;
using Logging;
using WebServicesAPI.Configuration;
using ServicesAPI.Caching;
using ServicesAPI.Configuration;


namespace Plugin_WorkAssignment.View.Presenter
{
    public class WorkOrderGridControlPresenter : IWorkOrderGridControlPresenter
    {
        public IWorkOrderGridControl View { get; set; }

        [InjectionMethod]
        public void Initialize(
            ILogger<WorkOrderGridControlPresenter> logger,
            IWorkAssignService workAssignService,
            ICachingService cachingService, 
            IConfigurationService configurationService)
        {
            _logger = logger;
            _workAssignService = workAssignService;
            _cachingService = cachingService;
            _configurationService = configurationService;

            _logger.Debug("WorkOrderGridControlPresenter::Initialize method.");
        }

        public void GetWorkOrders()
        {
            View.Activities = _workAssignService.GetActivityAssignmentsByWorkOrder(View.CurrentShop, View.CurrentWorkDate, View.CurrentAreaFilter);
        }

        /// <summary>
        /// Sets if overtime is required for an activity
        /// </summary>
        public void ProcessOTReq()
        {
            _logger.Debug("WorkOrderGridControlPresenter::ProcessOTReq method.");

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace(
                    "Call parameters {this.View.CurrentActivityID = {0}, this.View.CurrentIsOTReq = {1}}",
                    this.View.CurrentActivityID,
                    this.View.CurrentIsOTReq);
            }

            // TODO: This does return a status and some point, a notification or something should be
            // added if the operation does not work
            _workAssignService.SetOTRequired(this.View.CurrentActivityID, this.View.CurrentIsOTReq);
        }

        /// <summary>
        /// Gets the display name for a value from the data models
        /// </summary>
        /// <exception cref="MappingNotFoundException">Mapping for activity fields cannot be retrieved</exception>
        public void RetrieveDisplayName()
        {
            _logger.Debug("WorkOrderGridControlPresenter::RetrieveDisplayName method.");

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace(
                    "Call parameters {this.View.CurrentListFieldValue = {0}, this.View.CurrentProcessedField = {1}}",
                    this.View.CurrentListFieldValue,
                    this.View.CurrentProcessedField);
            }

            Mapping mapping = null;
            mapping = _configurationService.GetActivityMapping();

            // TODO: Error handling here should be done
            string displayName = this.View.CurrentListFieldValue;
            if (string.IsNullOrEmpty(this.View.CurrentListFieldValue) == false)
            {
                string datasourceID = mapping.GetDatasourceIdByPrimaryKey(this.View.CurrentProcessedField);
                Dictionary<string, string> data = _cachingService.GetLookupItems(datasourceID);
                if ((data != null) && (data.ContainsKey(this.View.CurrentListFieldValue) == true))
                {
                    displayName = data[this.View.CurrentListFieldValue];
                }
            }
            this.View.CurrentListFieldDisplay = displayName;

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of this.View.CurrentListFieldDisplay = {0}", displayName);
            }
        }

        private static ILogger<WorkOrderGridControlPresenter> _logger;

        /// <summary>
        /// Service to retrieve technician information
        /// </summary>
        private static IWorkAssignService _workAssignService;

        /// <summary>
        /// Service to retrieve mapped field names for activity
        /// </summary>
        private static IConfigurationService _configurationService;

        /// <summary>
        /// Service to retrieve metadata configuration
        /// </summary>
        private static ICachingService _cachingService;
    }
}