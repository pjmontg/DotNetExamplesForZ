<%@ Assembly Name="Plugin_WorkAssignment" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TechUtilizationGridControl.ascx.cs"
    Inherits="Plugin_WorkAssignment.Controls.TechUtilizationGridControl" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">



        function resizeLeftChart(pane) {
            var techUtilGridChart = $find('<%=TechUtilizationGridChart.ClientID %>');

            techUtilGridChart.set_height(pane.get_height() - 40);
            techUtilGridChart.set_width(pane.get_width());
        }
    </script>
</telerik:RadCodeBlock>
<telerik:RadHtmlChart runat="server" ID="TechUtilizationGridChart">
    <ChartTitle Text="Resource Utilization for Day">
    </ChartTitle>
    <Legend>
    <Appearance Visible="false">
    </Appearance>
    </Legend>
    <PlotArea>
        <Series>
            <telerik:ColumnSeries Stacked="false" DataFieldY="SCHEDULED_HRS" Name="Scheduled Hours"
                ColorField="ScheduledColor">
                <Appearance FillStyle-BackgroundColor="#2b952b">
                </Appearance>
                <LabelsAppearance Position="InsideBase">
                    <TextStyle Bold="true" />
                </LabelsAppearance>
                <TooltipsAppearance DataFormatString="{0} hrs. scheduled" />
            </telerik:ColumnSeries>
            <telerik:LineSeries DataFieldY="AVAILABLE_HRS" Name="Available Hours">
                 <TooltipsAppearance DataFormatString="{0} hrs. available" />
                <Appearance FillStyle-BackgroundColor="#1c69a5" />
                <MarkersAppearance MarkersType="Square" BackgroundColor="#1c69a5" />
                
                <LabelsAppearance Visible="true" Position="Above">
                    <TextStyle Bold="true" />
                </LabelsAppearance>
            </telerik:LineSeries>
        </Series>
        <XAxis AxisCrossingValue="0" Color="#a0a0a0" MajorTickType="Outside" MinorTickType="Outside"
            Reversed="false" DataLabelsField="TECH_NAME">
            <MajorGridLines Color="#EFEFEF" Width="1" />
            <MinorGridLines Color="#F7F7F7" Width="1" />
        </XAxis>
        <YAxis AxisCrossingValue="0" Color="#a0a0a0" MajorTickSize="1" MajorTickType="Outside"
            MinorTickSize="1" MinorTickType="Outside" Reversed="false" Step="5">
             <MajorGridLines Color="#e0e0e0" Width="1" />
            <MinorGridLines Color="#f0f0f0" Width="1" />
        </YAxis>
    </PlotArea>
</telerik:RadHtmlChart>
<div class="customLegendWrapper">
               <div class="customLegend">
                    <div class="available">
                         <div>
                         </div>
                         Available Hours</div>
                    <div class="under-scheduled">
                         <div>
                         </div>
                         Under Scheduled</div>
                    <div class="on-target-scheduled">
                         <div>
                         </div>
                         On Target</div>
                    <div class="over-scheduled">
                         <div>
                         </div>
                         Over Scheduled</div>
               </div>
          </div>