using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Logging;
using Microsoft.Practices.Unity;
using Plugin_WorkAssignment.ServicesAPI;
using Plugin_WorkAssignment.ModelAPI;
using System.Data;
using Utilities;
using System.Reflection;
using System.Drawing;

namespace Plugin_WorkAssignment.View.Presenter
{
    public class TechUtilizationGridControlPresenter : ITechUtilizationGridControlPresenter
    {
        #region Public methods
        /// <summary>
        /// Set logger for class
        /// </summary>
        /// <param name="logger">Logger for class</param>
        /// <param name="workAssignService">Service to retrieve technician information</param>
        [InjectionMethod]
        public void Initialize(
            ILogger<TechUtilizationGridControlPresenter> logger,
            IWorkAssignService workAssignService)
        {
            _logger = logger;
            _workAssignService = workAssignService;

            _logger.Debug("TechUtilizationGridControlPresenter::Initialize method.");
        }

        /// <summary>
        /// Retrieve daily technician utilization
        /// </summary>
        /// <exception cref="DataRepositoryException">Data accessing error</exception>
        public void GetDailyTechUtilizations()
        {
            _logger.Debug("TechUtilizationGridControlPresenter::GetDailyTechUtilizations method.");

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace(
                    "Call parameters {this.View.CurrentShop = {0}, this.View.CurrentWorkDate = {1}}",
                    this.View.CurrentShop,
                    this.View.CurrentWorkDate);
            }

            //DataTable result = new DataTable();
            List<VW_TECHNICIAN_UTILIZATION> result = null;
            try
            {
                result = _workAssignService.GetTechUtilization(
                    this.View.CurrentWorkDate,
                    this.View.CurrentShop);

                //this.View.DailyTechUtilization = this.TransposeData(techUtils);
                List<UtilizationChartData> stackedResults = new List<UtilizationChartData>();
                string color;
                foreach (VW_TECHNICIAN_UTILIZATION tech in result)
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
                        TECH_NAME = tech.TECH_NAME,
                        ScheduledColor = color
                    });
                }
                this.View.DailyTechUtilization = stackedResults;
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

        /*
        /// <summary>
        /// Setup display of data
        /// </summary>
        public void Display()
        {
            _logger.Debug("TechUtilizationGridControlPresenter::GetDailyTechUtilizations method.");

            if (_logger.IsTraceEnabled == true)
            {
                _logger.Trace(
                    "Call parameters {this.View.CurrentShop = {0}, this.View.CurrentWorkDate = {1}, this.View.IsInitialLoad = {2}, this.View.PreviousShop = {3}, this.View.PreviousWorkDate = {4}}",
                    this.View.CurrentShop,
                    this.View.CurrentWorkDate,
                    this.View.IsInitialLoad,
                    this.View.PreviousShop,
                    this.View.PreviousWorkDate);
            }

            // If shop or work date changed or initial hit, refresh data
            if ((this.View.IsInitialLoad == true) || ((this.View.CurrentShop != this.View.PreviousShop) ||
                    (Nullable.Compare(this.View.CurrentWorkDate, this.View.PreviousWorkDate) != 0)))
            {
                // Update previous data with new data
                this.View.ForceRefresh = true;
                this.View.PreviousShop = this.View.CurrentShop;
                this.View.PreviousWorkDate = this.View.CurrentWorkDate;

                if (_logger.IsTraceEnabled == true)
                {
                    _logger.Trace("Output of this.View.ForceRefresh = {0}", true);
                    _logger.Trace("Output of this.View.PreviousShop = {0}", this.View.PreviousShop);
                    _logger.Trace("Output of this.View.PreviousWorkDate = {0}", ObjectSerializingHelper.Serialize(this.View.PreviousWorkDate));
                }
            }
            else
            {
                this.View.ForceRefresh = false;

                if (_logger.IsTraceEnabled == true)
                {
                    _logger.Trace("Output of this.View.ForceRefresh = {0}", false);
                }
            }
        }

        /// <summary>
        /// Processes the look of utilization percentage of technicians
        /// </summary>
        public void ProcessUtilizationPercentage()
        {
            _logger.Debug("TechUtilizationGridControlPresenter::ProcessUtilizationPercentage method.");

            // If this row is for utilization percentage, that is the utilization percentage cell to color
            if ((this.View.CurrentData.Cells.Count > 0) && (this.View.CurrentData.Cells[2].Text == PERCENTAGE))
            {
                int count = this.View.CurrentData.Cells.Count - 3;
                for (int i = 0; i < count; i++)
                {
                    // Convert text into decimal percentage
                    int index = i + 3;
                    if (Convert.ToDecimal(this.View.CurrentData.Cells[index].Text) > 100)
                    {
                        this.View.CurrentData.Cells[index].BackColor = Color.Red;
                        this.View.CurrentData.Cells[index].Font.Bold = true;
                    }
                    else if (Convert.ToDecimal(this.View.CurrentData.Cells[index].Text) < 100)
                    {
                        this.View.CurrentData.Cells[index].BackColor = Color.LightGreen;
                        this.View.CurrentData.Cells[index].Font.Bold = true;
                    }
                    else
                    {
                        this.View.CurrentData.Cells[index].BackColor = Color.LightGray;
                        this.View.CurrentData.Cells[index].Font.Bold = true;
                    }
                }
            }
        }
         */
        #endregion

        #region Public properties
        /// <summary>
        /// Daily tech utilization view presenter performs business logic for
        /// </summary>
        public ITechUtilizationGridControl View { get; set; }
        #endregion

        /*
        #region Private methods
        /// <summary>
        /// Transpose (pivot) data for table
        /// </summary>
        /// <param name="techUtils">Input data</param>
        /// <returns>Transposed (pivoted) data</returns>
        private object TransposeData(List<VW_TECHNICIAN_UTILIZATION> techUtils)
        {
            _logger.Debug("TechUtilizationGridControlPresenter::TransposeData method.");

            // Need to return something if there is no data
            if (techUtils.Count == 0)
            {
                return new Object[0];
            }
            DataTable data = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;
            int count = 0;
            foreach (VW_TECHNICIAN_UTILIZATION rec in techUtils)
            {
                // Use reflection to get property names, must get properties only the first time
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();

                    //data.Columns.Add(new DataColumn("Headers", Type.GetType("System.String")));
                    foreach (PropertyInfo pi in oProps)
                    {
                        // Only process for these columns
                        if ((pi.Name != TECH_NAME) &&
                            (pi.Name != AVAILABLE_HRS) &&
                            (pi.Name != SCHEDULED_HRS) &&
                            (pi.Name != UTILIZATION_PCT))
                        {
                            continue;
                        }

                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        // Each column in the input list represents a row except for the first
                        // which will represent the columns
                        data.Rows.Add(data.NewRow());
                    }

                    // First row is not needed
                    data.Rows.RemoveAt(0);
                }

                if (count == 0)
                {
                    // Row 0 and Column 0 will have nothing in it
                    data.Columns.Add(" ");
                }
                foreach (PropertyInfo pi in oProps)
                {
                    switch (pi.Name)
                    {
                        case TECH_NAME:
                            {
                                // TECH_NAME now represents the columns
                                object headerValue = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                                data.Columns.Add(headerValue as string);
                            }
                            break;
                        case AVAILABLE_HRS:
                            {
                                data.Rows[0][0] = "Avail Hrs";
                                data.Rows[0][count + 1] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                            }
                            break;
                        case SCHEDULED_HRS:
                            {
                                data.Rows[1][0] = "Sched Hrs";
                                data.Rows[1][count + 1] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                            }
                            break;
                        case UTILIZATION_PCT:
                            {
                                data.Rows[2][0] = PERCENTAGE;
                                data.Rows[2][count + 1] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                            }
                            break;
                    }
                }
                count++;
            }
            return data;
        }
        #endregion
         */

        /*
        #region Private variables
        #region Private const variables
        /// <summary>
        /// Literal for %
        /// </summary>
        private const string PERCENTAGE = "%";

        /// <summary>
        /// Literal for TECH_NAME
        /// </summary>
        private const string TECH_NAME = "TECH_NAME";

        /// <summary>
        /// Literal for AVAILABLE_HRS
        /// </summary>
        private const string AVAILABLE_HRS = "AVAILABLE_HRS";

        /// <summary>
        /// Literal for SCHEDULED_HRS
        /// </summary>
        private const string SCHEDULED_HRS = "SCHEDULED_HRS";

        /// <summary>
        /// Literal for UTILIZATION_PCT
        /// </summary>
        private const string UTILIZATION_PCT = "UTILIZATION_PCT";
        #endregion
         */

        #region Private static variables
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static ILogger<TechUtilizationGridControlPresenter> _logger;

        /// <summary>
        /// Service to retrieve technician information
        /// </summary>
        private static IWorkAssignService _workAssignService;
        #endregion
        //#endregion
    }
}