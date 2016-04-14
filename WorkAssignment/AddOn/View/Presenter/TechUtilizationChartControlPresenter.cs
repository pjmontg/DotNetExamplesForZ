using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Logging;
using Plugin_WorkAssignment.ServicesAPI;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.ModelAPI;
using Utilities;

namespace Plugin_WorkAssignment.View.Presenter
{
    public class TechUtilizationChartControlPresenter : ITechUtilizationChartControlPresenter
    {
        #region Public methods
        /// <summary>
        /// Set logger for class
        /// </summary>
        /// <param name="logger">Logger for class</param>
        /// <param name="workAssignService">Service to retrieve technician information such as assigned hours and available hours</param>
        [InjectionMethod]
        public void Initialize(
            ILogger<TechUtilizationChartControlPresenter> logger,
            IWorkAssignService workAssignService)
        {
            _logger = logger;
            _workAssignService = workAssignService;

            _logger.Debug("TechUtilizationChartControlPresenter::Initialize method.");
        }

        /// <summary>
        /// Retrieve total assigned hours and available hours for technician's for a shop for a given week
        /// </summary>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        public void GetWeeklyUtilizations()
        {
            _logger.Debug("TechUtilizationGridControlPresenter::GetWeeklyUtilizations method.");

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace(
                    "Call parameters {this.View.CurrentShop = {0}, this.View.CurrentWorkDate = {1}}",
                    this.View.CurrentShop,
                    this.View.CurrentWorkDate);
            }

            List<VW_TECHNICIAN_ASSIGN_AVAIL> result = null;
            try
            {
                result = _workAssignService.GetTechSumAssignAvail(this.View.CurrentWorkDate, this.View.CurrentShop);
                List<UtilizationChartData> stackedResults = new List<UtilizationChartData>();
                string color;
                foreach (VW_TECHNICIAN_ASSIGN_AVAIL tech in result)
                {
                    if (tech.SCHEDULED_HRS > tech.AVAILABLE_HRS)
                    {
                        color = "#c73929";
                    }
                    else if (tech.SCHEDULED_HRS == tech.AVAILABLE_HRS)
                    {
                        color = "#cdcdcd";
                    }
                    else
                    {
                        color = "#069e5b";
                    }


                    stackedResults.Add(new UtilizationChartData
                    {
                        AVAILABLE_HRS = tech.AVAILABLE_HRS,
                        SCHEDULED_HRS = tech.SCHEDULED_HRS,
                        SHOP = tech.SHOP,
                        WORK_DATE = tech.WORK_DATE,
                        ScheduledColor = color
                    });
                }
                this.View.WeeklyTechUtilization = stackedResults;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
                throw;
            }

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace("Output of this.View.DailyTechUtilization = {0}", ObjectSerializingHelper.Serialize(result));
            }
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Assigned hours and technicians week view presenter performs business logic for
        /// </summary>
        public ITechUtilizationChartControl View { get; set; }
        #endregion

        #region Private variables
        #region Private static variables
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static ILogger<TechUtilizationChartControlPresenter> _logger;

        /// <summary>
        /// Service to retrieve technician information such as assigned hours and available hours
        /// </summary>
        private static IWorkAssignService _workAssignService;
        #endregion
        #endregion
    }
}