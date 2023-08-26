using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands._Helper.ChartBuilder
{
    using System;

    public class BarChartUrlBuilder
    {
        private int[] values;
        private string[] itemNames;
        private string title;
        private string chartType = "bvg"; // Default chart type
        private string chartSize = "400x300"; // Default chart size
        private string chartDataFormat = "t"; // Default data format
        private string colors = ""; // Default colors

        public BarChartUrlBuilder(int[] values, string[] itemNames)
        {
            if (values.Length != itemNames.Length)
            {
                throw new ArgumentException("Values and itemNames arrays must have the same length.");
            }

            this.values = values;
            this.itemNames = itemNames;
        }

        public BarChartUrlBuilder SetType(string chartType)
        {
            this.chartType = chartType;
            return this;
        }

        public BarChartUrlBuilder SetSize(string chartSize)
        {
            this.chartSize = chartSize;
            return this;
        }

        public BarChartUrlBuilder SetDataFormat(string dataFormat)
        {
            this.chartDataFormat = dataFormat;
            return this;
        }

        public BarChartUrlBuilder SetColors(string colors)
        {
            this.colors = colors;
            return this;
        }

        public BarChartUrlBuilder SetTitle(string title)
        {
            this.title = title;
            return this;
        }

        public string Build()
        {
            string valueString = string.Join(",", values);
            string itemNameString = string.Join("|", itemNames);

            string chartUrl = $"https://chart.googleapis.com/chart" +
                              $"?cht={chartType}" +
                              $"&chs={chartSize}" +
                              $"&chd={chartDataFormat}:{valueString}" +
                              $"&chl={Uri.EscapeDataString(itemNameString)}";

            return chartUrl;
        }
    }

}
