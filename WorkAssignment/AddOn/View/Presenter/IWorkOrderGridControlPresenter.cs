using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.View.ViewAbstraction;

namespace Plugin_WorkAssignment.View.Presenter
{
    public interface IWorkOrderGridControlPresenter
    {
        IWorkOrderGridControl View { get; set; }
        void GetWorkOrders();

        /// <summary>
        /// Sets if overtime is required for an activity
        /// </summary>
        void ProcessOTReq();

        /// <summary>
        /// Gets the display name for a value from the data models
        /// </summary>
        void RetrieveDisplayName();
    }
}
