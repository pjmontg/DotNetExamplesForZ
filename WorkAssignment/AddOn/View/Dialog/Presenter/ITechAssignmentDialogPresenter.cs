using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.View.Dialog.ViewAbstraction;
using System.Collections;

namespace Plugin_WorkAssignment.View.Dialog.Presenter
{
    public interface ITechAssignmentDialogPresenter
    {
        ITechAssignmentDialog View { get; set; }

        void LoadTechAssignments();
        void UpdateTechAssignmentEntry();

        object GetTechnicians();

        void AddNewAssignment(Hashtable assignment);

        void DeleteAssignment(Decimal techId);

        void UpdateAssignment(decimal techId, decimal hours);

        string TranslateTechId(string id);
    }
}
