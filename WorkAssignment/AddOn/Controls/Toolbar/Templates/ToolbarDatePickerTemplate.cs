using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI.Calendar;

namespace Plugin_WorkAssignment.Controls.Toolbar.Templates
{
    public class ToolbarDatePickerTemplate : ITemplate
    {
        public SelectedDateChangedEventHandler SelectedDateChanged { get; set; }

        public DateTime? SelectedDate
        {
            get
            {
                return this._datePicker.SelectedDate;
            }

            set
            {
                this._datePicker.SelectedDate = value;
            }
        }

        public void InstantiateIn(Control container)
        {
           
            _datePicker = new RadDatePicker();
            _datePicker.EnableEmbeddedSkins = false;
            _datePicker.Skin = "Mystique";
            _datePicker.CssClass = "schedule-date-picker";
            _datePicker.ToolTip = "Schedule Date";
            _datePicker.AutoPostBack = true;
            _datePicker.SelectedDateChanged += this.SelectedDateChanged;
            _datePicker.Calendar.DayRender += new Telerik.Web.UI.Calendar.DayRenderEventHandler(OnDayRender);
            container.Controls.Add(_datePicker);

        }

        protected void OnDayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
        {
            RadCalendar calendar = sender as RadCalendar;
            // Disable Staurdays and Sundays
            if (e.Day.Date.DayOfWeek == DayOfWeek.Saturday || e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                string calendarSkin = calendar.Skin != "" ? calendar.Skin : "Default";
                string otherMonthCssClass = "rcOutOfRange";

                // clear the default cell content (anchor tag) as we need to disable the hover effect for this cell
                e.Cell.Text = "";
                e.Cell.CssClass = otherMonthCssClass; //set new CssClass for the disabled calendar day cells (e.g. look like other month days here)
                // render a span element with the processed calendar day number instead of the removed anchor -- necessary for the calendar skinning mechanism
                Label label = new Label();
                label.Text = e.Day.Date.Day.ToString();
                e.Cell.Controls.Add(label);
                // disable the selection for the specific day
                RadCalendarDay calendarDay = new RadCalendarDay();
                calendarDay.Date = e.Day.Date;
                calendarDay.IsSelectable = false;
                calendarDay.ItemStyle.CssClass = otherMonthCssClass;
                calendar.SpecialDays.Add(calendarDay);
            }
        }

        private RadDatePicker _datePicker = new RadDatePicker();
    }
}