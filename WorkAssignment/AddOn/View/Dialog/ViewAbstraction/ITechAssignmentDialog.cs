using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Plugin_WorkAssignment.ModelAPI;

namespace Plugin_WorkAssignment.View.Dialog.ViewAbstraction
{
    public interface ITechAssignmentDialog
    {
        BindingList<TECHNICIAN_ASSIGNMENT> TechAssignments { get; set; }

        string ActivityId { get; }

        DateTime WorkDate { get; }

        string Shop { get; }

        /// <summary>
        /// Save and refresh the full page via javascript
        /// </summary>
        string Refresh { set; }
    }
}
