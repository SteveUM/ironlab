using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace IronPlot.PlotCommon
{
    internal class PlotSettingsFactory
    {

        public static readonly string[] LineStyle = new string[] { "", "-", "--", "-.", ":", "solid", "dashed", "dash_dot", "dotted", "None", " " };

        /// <summary>
        /// Maps the line type to its corresponding option string value.
        /// </summary>
        /// 
        public static readonly Dictionary<string, QuickStrokeDash> LineStyleMap = new Dictionary<string, QuickStrokeDash>()
        {
            {LineStyle[0], QuickStrokeDash.None},    {LineStyle[1], QuickStrokeDash.Solid},   {LineStyle[2], QuickStrokeDash.Dash},
            {LineStyle[3], QuickStrokeDash.DashDot}, {LineStyle[4], QuickStrokeDash.Dot},     {LineStyle[5], QuickStrokeDash.Solid},
            {LineStyle[6], QuickStrokeDash.Dash},    {LineStyle[7], QuickStrokeDash.DashDot}, {LineStyle[8], QuickStrokeDash.Dot},
            {LineStyle[9], QuickStrokeDash.None},    {LineStyle[10], QuickStrokeDash.None}
        };

        public static readonly string[] LineColor = new string[] { "", "b", "g", "r", "c", "m", "y", "k", "w", "blue", "green", "red", 
            "cyan", "magenta", "yellow", "black", "white"};


        /// <summary>
        /// Maps the color string value to its corresponding color object.
        /// </summary>
        /// <see cref="http://matplotlib.org/api/colors_api.html"/>
        /// 
        public static readonly Dictionary<string, SolidColorBrush> LineColorMap = new Dictionary<string, SolidColorBrush>()
        {
            {LineColor[0], Brushes.Black},  {LineColor[1], Brushes.Blue},     {LineColor[2], Brushes.Green}, 
            {LineColor[3], Brushes.Red},    {LineColor[4], Brushes.Cyan},     {LineColor[5], Brushes.Magenta},
            {LineColor[6], Brushes.Yellow}, {LineColor[7], Brushes.Black},    {LineColor[8], Brushes.White},
            {LineColor[9], Brushes.Blue},   {LineColor[10], Brushes.Green},   {LineColor[11], Brushes.Red},
            {LineColor[12], Brushes.Cyan},  {LineColor[13], Brushes.Magenta}, {LineColor[14], Brushes.Yellow},
            {LineColor[15], Brushes.Black}, {LineColor[16], Brushes.White}
        };

        /// <summary>
        /// Defines the supported Marker symbols used in a line plot.
        /// </summary>
        /// <see cref="http://matplotlib.org/1.5.1/examples/lines_bars_and_markers/marker_reference.html"/>
        /// 
        public static readonly string[] MarkerId = new string[] {"", ".", ",", "o", "v", "^", "<", ">", "1", "2", "3", "4", "s", "p",
            "*", "h", "H", "+", "x", "D", "d", "|", "_", "#0", "#1", "#2", "#3", "#4", "#5", "#6", "#7", "8"
        
        };

        public static readonly Dictionary<MarkersType, string> MarkerStyleMap = new Dictionary<MarkersType, string>() 
        { 
            { MarkersType.None, MarkerId[0]},           { MarkersType.Point, MarkerId[1]},          { MarkersType.Pixel, MarkerId[2]},
            { MarkersType.Circle, MarkerId[3]},         { MarkersType.TriangleDown,  MarkerId[4]},  { MarkersType.TriangleUp,  MarkerId[5]},
            { MarkersType.TriangleLeft, MarkerId[6]},   { MarkersType.TriangleRight, MarkerId[7]},  { MarkersType.TriDown,  MarkerId[8]},
            { MarkersType.TriUp, MarkerId[9]},          { MarkersType.TriLeft,  MarkerId[10]},      { MarkersType.TriRight,  MarkerId[11]},
            { MarkersType. Square, MarkerId[12]},       { MarkersType.Pentagon, MarkerId[13]},      { MarkersType.Star,  MarkerId[14]},
            { MarkersType. Hexagon1,  MarkerId[15]},    { MarkersType. Hexagon2,  MarkerId[16]},    { MarkersType. Plus, MarkerId[17]},
            { MarkersType. X,  MarkerId[18]},           { MarkersType. Diamond,  MarkerId[19]},     { MarkersType.ThinDiamond,  MarkerId[20]},
            { MarkersType.VLine,  MarkerId[21]},        { MarkersType.HLine, MarkerId[22]},

            //new
            { MarkersType.TickLeft,  MarkerId[MarkerId.IndexOf("#0")]},     
            { MarkersType.TickRight,  MarkerId[MarkerId.IndexOf("#1")]},    
            { MarkersType.TickUp,  MarkerId[MarkerId.IndexOf("#2")]},
            { MarkersType.TickDown,  MarkerId[MarkerId.IndexOf("#3")]},     
            { MarkersType.CaretLeft,  MarkerId[MarkerId.IndexOf("#4")]},   
            { MarkersType.CaretRight,  MarkerId[MarkerId.IndexOf("#5")]},   
            { MarkersType.CaretUp,  MarkerId[MarkerId.IndexOf("#6")]},
            { MarkersType.CaretDown,  MarkerId[MarkerId.IndexOf("#7")]},    
            { MarkersType.Octagon,  MarkerId[MarkerId.IndexOf("8")]}
        };


        public static bool IsMarker(string c)
        {
            return !string.IsNullOrEmpty(MarkerStyleMap.Where(kvp => kvp.Value == c).Select(n => n.Value).FirstOrDefault());
        }

        public static bool IsStroke(string c)
        {
            //return !string.IsNullOrEmpty(LineStyleMap.Where(kvp => kvp.Value == c).Select(n => n.Value).FirstOrDefault());
            return LineStyleMap.ContainsKey(c);
        }

        public static bool IsColor(string c)
        {
            return LineColorMap.ContainsKey(c);
        }

        private static string GetChar(string s)
        {
            string c = string.Empty;
            if (s.Length > 0)
            {
                c = s[0].ToString();
            }
            return c;
        }

        /// <summary>
        /// Converts plot line settings string to a set of internal representations through the use of a Tuple object.
        /// </summary>
        /// <param name="settings">Line settings parameter</param>
        /// <returns>Tuple with settings based on settings string</returns>
        public static Tuple<QuickStrokeDash, MarkersType, SolidColorBrush> PlotSettings(string settings = "-")
        {
            QuickStrokeDash lineStyleResult = QuickStrokeDash.Solid;
            MarkersType markerResult = MarkersType.None;
            SolidColorBrush brushResult = Brushes.Black;

            while (settings.Length > 0)
            {
                string c = GetChar(settings);
                settings = settings.Substring(1);

                if (IsStroke(c))
                {
                    if (c == "-" && settings.Length >= 1 && (settings[0] == '-' || settings[0] == '.'))
                    {
                        c += GetChar(settings);
                        settings = settings.Substring(1);
                    }
                    //lineStyleResult = (QuickStrokeDash)PlotSettingsFactory.LineStyleMap.Where(kvp => kvp.Value == c).Select(n => n.Key).FirstOrDefault();
                    lineStyleResult = LineStyleMap[c];
                }

                if (IsMarker(c))
                {
                    markerResult = MarkerGeometries.MarkerType(c);
                }

                if (IsColor(c))
                {
                    brushResult = LineColorMap.Where(kvp => kvp.Key == c).Select(n => n.Value).FirstOrDefault();
                }
            }

            return new Tuple<QuickStrokeDash, MarkersType, SolidColorBrush>(lineStyleResult, markerResult, brushResult);
        }

        public static string PlotSettingsToString(QuickStrokeDash line, MarkersType marker, SolidColorBrush color)
        {
            string lineStr = LineStyleMap.Where(kvp => kvp.Value == line).Select(n => n.Key).FirstOrDefault();  //LineStyleMap[line];
            string markerStr = MarkerStyleMap[marker];
            string colorStr = (string)LineColorMap.Where(kvp => kvp.Value == color).Select(n => n.Key).FirstOrDefault();
            return lineStr + markerStr + colorStr;
        }

    }
}
