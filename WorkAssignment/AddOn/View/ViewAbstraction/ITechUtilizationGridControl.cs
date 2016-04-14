using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telerik.Web.UI;

namespace Plugin_WorkAssignment.View.ViewAbstraction
{
    public interface ITechUtilizationGridControl
    {
        #region Properties
        /// <summary>
        /// Currently selected shop
        /// </summary>
        string CurrentShop { get; }

        /// <summary>
        /// Currently selected work date
        /// </summary>
        DateTime? CurrentWorkDate { get; }

        /// <summary>
        /// Daily tech utilization list
        /// </summary>
        object DailyTechUtilization { set; }
        
        /*
        /// <summary>
        /// Shop value before postback processing
        /// </summary>
        string PreviousShop { get; set; }

        /// <summary>
        /// Work date value before postback processing
        /// </summary>
        DateTime? PreviousWorkDate { get; set; }

        /// <summary>
        /// True if page is initially loaded (no postbacks)
        /// </summary>
        bool IsInitialLoad { get; }

        /// <summary>
        /// True means to refresh data
        /// </summary>
        bool ForceRefresh { set; }

        /// <summary>
        /// The current data being bound to the grid
        /// </summary>
        GridDataItem CurrentData { get; }
         */
        #endregion
    }
}
