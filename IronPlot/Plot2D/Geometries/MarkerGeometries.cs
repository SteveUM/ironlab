using IronPlot.PlotCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IronPlot
{
    public struct GenericMarker
    {
        public double[] X;
        public double[] Y;
    }
    public enum MarkersType
    {
        None, Point, Pixel, Circle, TriangleDown, TriangleUp, TriangleLeft,
        TriangleRight, TriDown, TriUp, TriLeft, TriRight, Square, Pentagon,
        Star, Hexagon1, Hexagon2, Plus, X, Diamond, ThinDiamond,
        VLine, HLine, Octagon, TickLeft, TickRight, TickUp, TickDown, 
        CaretRight, CaretUp, CaretDown, CaretLeft
    };

    public class MarkerGeometries
    {
        public static Dictionary<MarkersType, GenericMarker> GenericMarkerLookup = new Dictionary<MarkersType, GenericMarker>();

        /// <summary>
        /// Finds the marker type enum based on the string parameter.
        /// </summary>
        /// <param name="id">Marker id as string (from PyPlot)</param>
        /// <returns></returns>
        /// <see cref="http://matplotlib.org/api/pyplot_api.html#matplotlib.pyplot.plot"/>
        /// 
        public static MarkersType MarkerType(string id)
        {
            if (string.IsNullOrEmpty(id)) { return MarkersType.None; }

            int index = PlotSettingsFactory.MarkerId.IndexOf(id);
            if (index > 0 && index < Enum.GetNames(typeof(MarkersType)).Length)
            {
                return (MarkersType)index;
            }
            else
            {
                return MarkersType.None;
            }
        }

        /// <summary>
        /// Creates the geometries for the markers identified in the enum.
        /// </summary>
        /// <see cref="MarkersType"/>
        /// 
        static MarkerGeometries()
        {
            double cos30 = Math.Cos(30.0 * Math.PI / 180.0); //0.86
            double cos45 = Math.Cos(45.0 * Math.PI / 180.0); //0.70

            GenericMarkerLookup.Add(MarkersType.TriangleDown,
                new GenericMarker() { X = new double[] { 0, 0.5, -0.5 }, Y = new double[] { 0.5 / cos30, -(1 - 0.5 / cos30), -(1 - 0.5 / cos30) } });

            GenericMarkerLookup.Add(MarkersType.TriangleUp,
                new GenericMarker() { X = new double[] { 0, 0.5, -0.5 }, Y = new double[] { -0.5 / cos30, 1 - 0.5 / cos30, 1 - 0.5 / cos30 } });

            GenericMarkerLookup.Add(MarkersType.Diamond,
                new GenericMarker() { X = new double[] { 0, cos45, 0, -cos45 }, Y = new double[] { cos45, 0, -cos45, 0 } });

            // Star geometry (5-pointed star):
            double[] x = new double[10]; double[] y = new double[10];
            double bluntness = 0.4; double r = 1 / (1 + Math.Cos(72 * Math.PI / 180.0));
            for (int i = 0; i < 10; ++i)  // 0.1 is sharp, 0.9 is blunt!
            {
                x[i] = Math.Sin(36 * i * Math.PI / 180.0) * r;
                y[i] = -Math.Cos(36 * i * Math.PI / 180.0) * r;
                if (i % 2 == 1)
                {
                    x[i] *= bluntness; y[i] *= bluntness;
                }
            }
            GenericMarkerLookup.Add(MarkersType.Star, new GenericMarker() { X = x, Y = y });

            //--- new ---
            GenericMarkerLookup.Add(MarkersType.X,
                new GenericMarker() { X = new double[] { -0.25, 0.25, 0, 0.25, -0.25, 0 }, Y = new double[] { 0.25, -0.25, 0, 0.25, -0.25, 0 } });

            GenericMarkerLookup.Add(MarkersType.Plus,
                new GenericMarker() { X = new double[] { 0, 0, 0.25, -0.25, 0, 0 }, Y = new double[] { 0.25, 0, 0, 0, 0, -0.25 } });

            GenericMarkerLookup.Add(MarkersType.ThinDiamond,
                new GenericMarker() { X = new double[] { 0, cos45 * 0.6, 0, -cos45 * 0.6 }, Y = new double[] { cos45, 0, -cos45, 0 } });

            x = new double[6];
            y = new double[6];
            r = 1 / (1 + Math.Cos(60 * Math.PI / 180.0));
            for (int i = 0; i < 6; i++)
            {
                x[i] = Math.Sin(60 * i * Math.PI / 180.0) * r;
                y[i] = -Math.Cos(60 * i * Math.PI / 180.0) * r;
            }
            GenericMarkerLookup.Add(MarkersType.Hexagon1, new GenericMarker() { X = x, Y = y });

            double[] _x = new double[y.Length]; Array.Copy(y, _x, y.Length);
            double[] _y = new double[y.Length]; Array.Copy(x, _y, y.Length);
            for (int i = 0; i < _y.Length; i++)
            {
                _y[i] = _y[i] * -1;
            }
            GenericMarkerLookup.Add(MarkersType.Hexagon2, new GenericMarker() { X = _x, Y = _y });

            x = new double[5];
            y = new double[5];
            r = 1 / (1 + Math.Cos(72 * Math.PI / 180.0));
            for (int i = 0; i < 5; i++)
            {
                x[i] = Math.Sin(72 * i * Math.PI / 180.0) * r;
                y[i] = -Math.Cos(72 * i * Math.PI / 180.0) * r;
            }
            GenericMarkerLookup.Add(MarkersType.Pentagon, new GenericMarker() { X = x, Y = y });

            GenericMarkerLookup.Add(MarkersType.TriDown,
                new GenericMarker() { X = new double[] { -0.25, 0, 0.25, 0, 0, 0 }, Y = new double[] { 0.25, 0, 0.25, 0, -0.25, 0 } });

            GenericMarkerLookup.Add(MarkersType.TriUp,
                new GenericMarker() { X = new double[] { -0.25, 0, 0.25, 0, 0, 0 }, Y = new double[] { -0.25, 0, -0.25, 0, 0.25, 0 } });

            GenericMarkerLookup.Add(MarkersType.TriLeft,
                new GenericMarker() { X = new double[] { 0.25, 0, 0.25, 0, -0.25, 0 }, Y = new double[] { 0.25, 0, -0.25, 0, 0, 0 } });

            GenericMarkerLookup.Add(MarkersType.TriRight,
                new GenericMarker() { X = new double[] { -0.25, 0, -0.25, 0, 0.25, 0 }, Y = new double[] { 0.25, 0, -0.25, 0, 0, 0 } });

            GenericMarkerLookup.Add(MarkersType.HLine,
                new GenericMarker() { X = new double[] { -0.25, 0.25 }, Y = new double[] { 0, 0 } });

            GenericMarkerLookup.Add(MarkersType.VLine,
                new GenericMarker() { X = new double[] { 0, 0 }, Y = new double[] { -0.25, 0.25 } });

            GenericMarkerLookup.Add(MarkersType.TriangleRight, //90 CW = (y, -x)
                new GenericMarker() { X = new double[] { 0.5 / cos30, -(1 - 0.5 / cos30), -(1 - 0.5 / cos30) }, Y = new double[] { 0, -0.5, 0.5 } });

            GenericMarkerLookup.Add(MarkersType.TriangleLeft, //90 CCW = (-y, x)
                new GenericMarker() { X = new double[] { -(0.5 / cos30), (1 - 0.5 / cos30), (1 - 0.5 / cos30) }, Y = new double[] { 0, 0.5, -0.5 } });

            GenericMarkerLookup.Add(MarkersType.TickLeft,
                new GenericMarker() { X = new double[] { -0.25, -cos45 }, Y = new double[] { 0, 0 } });

            GenericMarkerLookup.Add(MarkersType.TickRight,
                new GenericMarker() { X = new double[] { 0.25, cos45 }, Y = new double[] { 0, 0 } });

            GenericMarkerLookup.Add(MarkersType.TickUp,
                new GenericMarker() { X = new double[] { 0, 0 }, Y = new double[] { 0.25, cos45 } });

            GenericMarkerLookup.Add(MarkersType.TickDown,
                new GenericMarker() { X = new double[] { 0, 0 }, Y = new double[] { -0.25, -cos45 } });

            GenericMarkerLookup.Add(MarkersType.CaretRight, //90 CW = (y, -x)
                new GenericMarker() { X = new double[] { 0, -(1 - 0.25 / cos30), -(1 - 0.25 / cos30) }, Y = new double[] { 0, -0.5, 0.5 } });

            GenericMarkerLookup.Add(MarkersType.CaretUp, //90 CW = (y, -x)
                new GenericMarker() { X = new double[] { 0, -0.5, 0.5 }, Y = new double[] { 0, (1 - 0.25 / cos30), (1 - 0.25 / cos30) } });

            GenericMarkerLookup.Add(MarkersType.CaretLeft, //90 CW = (y, -x)
                new GenericMarker() { X = new double[] { 0, (1 - 0.25 / cos30), (1 - 0.25 / cos30) }, Y = new double[] { 0, 0.5, -0.5 } });

            GenericMarkerLookup.Add(MarkersType.CaretDown, //90 CW = (y, -x)
                new GenericMarker() { X = new double[] { 0, 0.5, -0.5 }, Y = new double[] { 0, -(1 - 0.25 / cos30), -(1 - 0.25 / cos30) } });

            x = new double[8];
            y = new double[8];
            r = 1 / (1 + Math.Cos(45 * Math.PI / 180.0));
            for (int i = 0; i < 8; i++)
            {
                x[i] = Math.Sin(45 * i * Math.PI / 180.0) * r;
                y[i] = -Math.Cos(45 * i * Math.PI / 180.0) * r;
            }
            GenericMarkerLookup.Add(MarkersType.Octagon, new GenericMarker() { X = x, Y = y });

        }

        internal static Geometry LegendMarkerGeometry(MarkersType markersType, double markersSize)
        {
            return LegendMarkerGeometry(markersType, markersSize, markersSize);
        }

        internal static Geometry LegendMarkerGeometry(MarkersType markersType, double width, double height)
        {
            Geometry legendMarkerGeometry = null;
            switch (markersType)
            {
                case MarkersType.None:
                    break;
                case MarkersType.Square:
                    legendMarkerGeometry = MarkerGeometries.RectangleMarker(width, height, new Point(0, 0));
                    break;
                case MarkersType.Circle:
                    legendMarkerGeometry = MarkerGeometries.EllipseMarker(width, height, new Point(0, 0));
                    break;
                case MarkersType.Point:
                    legendMarkerGeometry = MarkerGeometries.EllipseMarker(width / 2, height / 2, new Point(0, 0));
                    break;
                case MarkersType.Pixel:
                    legendMarkerGeometry = MarkerGeometries.EllipseMarker(width / 4, height / 4, new Point(0, 0));
                    break;
                case MarkersType.Star:
                case MarkersType.CaretDown:
                case MarkersType.CaretLeft:
                case MarkersType.CaretRight:
                case MarkersType.CaretUp:
                case MarkersType.Hexagon1:
                case MarkersType.Hexagon2:
                case MarkersType.Octagon:
                case MarkersType.Pentagon:
                case MarkersType.ThinDiamond:
                case MarkersType.TriangleDown:
                case MarkersType.TriangleLeft:
                case MarkersType.TriangleRight:
                case MarkersType.TriangleUp:
                case MarkersType.Diamond:
                    legendMarkerGeometry = MarkerGeometries.CustomMarker(width, height, new Point(0,0), markersType);
                    break;
                default:
                    legendMarkerGeometry = GetGenericGeometry(markersType, width, height);
                    break;
            }
            return legendMarkerGeometry;
        }

        internal static Geometry MarkersAsGeometry(Curve curve, MatrixTransform graphToCanvas, MarkersType markersType, double markersSize)
        {
            double xScale = graphToCanvas.Matrix.M11;
            double xOffset = graphToCanvas.Matrix.OffsetX;
            double yScale = graphToCanvas.Matrix.M22;
            double yOffset = graphToCanvas.Matrix.OffsetY;
            GeometryGroup markers = new GeometryGroup();
            double width = Math.Abs(markersSize);
            double height = Math.Abs(markersSize);
            Geometry markerGeometry = LegendMarkerGeometry(markersType, markersSize);
            if (markerGeometry == null) return null;
            markerGeometry.Freeze();
            for (int i = 0; i < curve.xTransformed.Length; ++i)
            {
                if (!curve.includeMarker[i]) continue;
                double xCanvas = curve.xTransformed[i] * xScale + xOffset;
                double yCanvas = curve.yTransformed[i] * yScale + yOffset;
                Geometry newMarker = markerGeometry.Clone();
                newMarker.Transform = new TranslateTransform(xCanvas, yCanvas);
                markers.Children.Add(newMarker);
            }
            markers.Freeze();
            return markers;
        }

        public static Geometry RectangleMarker(double width, double height, Point centre)
        {
            return new RectangleGeometry(new Rect(centre.X - width / 2, centre.Y - height / 2, width, height));
        }

        public static Geometry EllipseMarker(double width, double height, Point centre)
        {
            return new EllipseGeometry(new Rect(centre.X - width / 2, centre.Y - height / 2, width, height));
        }

        /// <summary>
        /// Creates a custom marker geometry based on the markers defined in the constructor. 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="centre"></param>
        /// <param name="markerType"></param>
        /// <returns></returns>
        public static Geometry CustomMarker(double width, double height, Point centre, MarkersType markerType)
        {
            //http://www.blackwasp.co.uk/WPFPathGeometry.aspx
            PathGeometry pg = new PathGeometry();
            PathFigure pf = new PathFigure() { IsClosed = true };
            GenericMarker gm = GenericMarkerLookup[markerType];

            pf.StartPoint = new Point(gm.X[0] * (height / 2), gm.Y[0] * (height / 2));
            for (int i = 0; i < gm.X.Length; i++)
            {
                pf.Segments.Add(new LineSegment(new Point(gm.X[i] * (height / 2), gm.Y[i] * (height / 2)), true));
            }
            pf.Segments.Add(new LineSegment(new Point(gm.X[0] * (height / 2), gm.Y[0] * (height / 2)), true));
            pg.Figures.Add(pf);
            Path p = new Path() { Height = height, Width = width, Data = pg };
            return pg;
        }


        public static Geometry StringMarker(string markerString, double points)
        {
            FormattedText text = new FormattedText(markerString,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Tahoma"),
                points * 96.0 / 72.0 * 2,
                Brushes.Black);
            text.TextAlignment = TextAlignment.Center;
            text.Trimming = TextTrimming.CharacterEllipsis;
            Geometry geometry = text.BuildGeometry(new Point(0, 0));
            double height = text.Extent;
            geometry = text.BuildGeometry(new Point(0, -height / 2));
            return geometry;
        }

        public static Geometry GetGenericGeometry(MarkersType markersType, double width, double height)
        {
            StreamGeometry geometry = new StreamGeometry();
            GenericMarker markerSpecification = GenericMarkerLookup[markersType];
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(markerSpecification.X[0] * width, markerSpecification.Y[0] * height), true /* is filled */, true /* is closed */);
                int n = markerSpecification.X.Length;
                for (int i = 1; i < n; ++i)
                {
                    ctx.LineTo(new Point(markerSpecification.X[i] * width, markerSpecification.Y[i] * height), true /* is stroked */, false /* is smooth join */);
                }
            }
            return geometry;
        }
    }
}
