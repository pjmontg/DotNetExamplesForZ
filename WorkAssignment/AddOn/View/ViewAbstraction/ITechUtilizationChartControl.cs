using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.ModelAPI;

namespace Plugin_WorkAssignment.View.ViewAbstraction
{
    public interface ITechUtilizationChartControl
    {
        #region Public properites
        /// <summary>
        /// Weeks worth utilizations totals of a shop
        /// </summary>
        List<UtilizationChartData> WeeklyTechUtilization { set; }

        /// <summary>
        /// Currently selected shop
        /// </summary>
        string CurrentShop { get; }

        /// <summary>
        /// Currently selected work date
        /// </summary>
        DateTime? CurrentWorkDate { get; }
        #endregion
    }
}
