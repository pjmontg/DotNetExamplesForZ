using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebControls.Grid.Templates;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Plugin_WorkAssignment.Controls.Grid.Templates
{
    public class PredStatusWorkAssignmentColumnTemplate : PredecessorStatusLinkColumnTemplate
    {
        protected override void DataBinding(object sender, EventArgs e)
        {
            Image image = (Image)sender;
            if (null != image)
            {
                GridDataItem item = (GridDataItem)image.NamingContainer;

                if ((item != null) && (null != item.NamingContainer) && (null != item.NamingContainer.NamingContainer))
                {
                    string predStatus = Convert.ToString(DataBinder.Eval(item.DataItem, DataField));
                    image.ImageUrl = getImageURl(predStatus);


                    if (String.IsNullOrEmpty(image.ImageUrl) == true)
                    {
                        image.Visible = false;
                    }
                    else
                    {
                        

                        // Set proper image icon size
                        image.Width = new Unit(24, UnitType.Pixel);
                        image.Height = new Unit(24, UnitType.Pixel);
                        image.Style["display"] = "block";
                        image.Style["margin"] = "auto";
                    }
                }
            }
        }

        private string getImageURl(String status)
        {
            String imageUrl = "";

            if (!String.IsNullOrEmpty(status))
            {
                if ("NS".Equals(status, StringComparison.InvariantCultureIgnoreCase))
                {
                    imageUrl = "~/Images/predStatusNotStarted.png";
                }
                else if ("INCOMP".Equals(status, StringComparison.InvariantCultureIgnoreCase))
                {
                    imageUrl = "~/Images/predStatusInComplete.png";
                }
                else if ("COMP".Equals(status, StringComparison.InvariantCultureIgnoreCase))
                {
                    imageUrl = "~/Images/predStatusComplete.png";
                }
                else if ("NONE".Equals(status, StringComparison.InvariantCultureIgnoreCase))
                {
                    imageUrl = "";
                }
            }
            return imageUrl;
        }
        
    }
}