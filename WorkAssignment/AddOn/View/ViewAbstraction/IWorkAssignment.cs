using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin_WorkAssignment.View.ViewAbstraction
{
    public interface IWorkAssignment
    {
        IDictionary<String, String> ShopList { set; }
        IDictionary<String, String> AreaList { set; }
    }
}
