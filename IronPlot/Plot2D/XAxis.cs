﻿// Copyright (c) 2010 Joe Moorhouse

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Navigation;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace IronPlot
{
    public enum XAxisPosition { Top, Bottom };

    public class XAxis : Axis2D
    {
        // The y position of the axis in Axis Canvas coordinates. 
        internal double yPosition = 0;
        // The y position of the opposite axis in Axis Canvas coorindates.
        internal double yPositionOpposite;

        internal static DependencyProperty XAxisPositionProperty =
            DependencyProperty.Register("XAxisPositionProperty",
            typeof(XAxisPosition), typeof(XAxis), new PropertyMetadata(XAxisPosition.Bottom));

        public XAxisPosition Position { get { return (XAxisPosition)GetValue(XAxisPositionProperty); } }

        public XAxis() : base() { }

        internal override void PositionLabels(bool cullOverlapping)
        {
            TextBlock currentTextBlock;
            int missOut = 0, missOutMax = 0;
            double currentRight, lastRight = Double.NegativeInfinity;
            // Go through ticks in order of increasing Canvas coordinate.
            for (int i = 0; i < Ticks.Length; ++i)
            {
                // Miss out labels if these would overlap.
                currentTextBlock = tickLabels[i];
                currentRight = Scale * Ticks[i] - Offset + currentTextBlock.DesiredSize.Width / 2.0;
                currentTextBlock.SetValue(Canvas.LeftProperty, currentRight - currentTextBlock.DesiredSize.Width);
                if ((XAxisPosition)GetValue(XAxisPositionProperty) == XAxisPosition.Bottom)
                {
                    currentTextBlock.SetValue(Canvas.TopProperty, yPosition + Math.Max(TickLength, 0.0));
                }
                else
                {
                    currentTextBlock.SetValue(Canvas.TopProperty, yPosition - Math.Max(TickLength, 0.0) - currentTextBlock.DesiredSize.Height);
                }
                if ((currentRight - currentTextBlock.DesiredSize.Width * 1.25) < lastRight)
                {
                    ++missOut;
                }
                else
                {
                    lastRight = currentRight;
                    missOutMax = Math.Max(missOut, missOutMax);
                    missOut = 0;
                }
            }
            missOutMax = Math.Max(missOutMax, missOut);
            missOut = 0;
            if (cullOverlapping)
            {
                for (int i = 0; i < Ticks.Length; ++i)
                {
                    if ((missOut < missOutMax) && (i > 0))
                    {
                        missOut += 1;
                        tickLabels[i].Text = "";
                    }
                    else missOut = 0;
                }
            }
            // Cycle through any now redundant TextBlocks and make invisible.
            for (int i = Ticks.Length; i < tickLabels.Count; ++i)
            {
                tickLabels[i].Text = "";
            }
        }

        internal override double LimitingTickLabelSizeForLength(int index)
        {
            return tickLabels[index].DesiredSize.Width;
        }

        protected override double LimitingTickLabelSizeForThickness(int index)
        {
            return tickLabels[index].DesiredSize.Height;
        }

        protected override double LimitingAxisLabelSizeForLength()
        {
            return axisLabel.DesiredSize.Width;
        }

        protected override double LimitingAxisLabelSizeForThickness()
        {
            return axisLabel.DesiredSize.Height;
        }

        internal override Point TickStartPosition(int i)
        {
            return new Point(Ticks[i] * Scale - Offset, yPosition);
        }

        internal override void RenderAxis()
        {
            XAxisPosition position = (XAxisPosition)GetValue(XAxisPositionProperty);
            Point tickPosition;

            StreamGeometryContext lineContext = axisLineGeometry.Open();
                Point axisStart = new Point(Min * Scale - Offset - axisLine.StrokeThickness / 2, yPosition);
                lineContext.BeginFigure(axisStart, false, false);
                lineContext.LineTo(new Point(Max * Scale - Offset + axisLine.StrokeThickness / 2, yPosition), true, false);
            lineContext.Close();
            
            if (TicksVisible)
            {
                StreamGeometryContext ticksContext = axisTicksGeometry.Open();
                for (int i = 0; i < Ticks.Length; ++i)
                {
                    if (position == XAxisPosition.Bottom)
                    {
                        tickPosition = TickStartPosition(i);
                        ticksContext.BeginFigure(tickPosition, false, false);
                        tickPosition.Y = tickPosition.Y + TickLength;
                        ticksContext.LineTo(tickPosition, true, false);
                    }
                    if (position == XAxisPosition.Top)
                    {
                        tickPosition = TickStartPosition(i);
                        ticksContext.BeginFigure(tickPosition, false, false);
                        tickPosition.Y = tickPosition.Y - TickLength;
                        ticksContext.LineTo(tickPosition, true, false);
                    }
                }
                ticksContext.Close();
            }
        }

        internal override Transform1D GraphToAxesCanvasTransform()
        {
            return new Transform1D(Scale, Offset);
        }

        internal override Transform1D GraphToCanvasTransform()
        {
            return new Transform1D(Scale, Offset + AxisMargin.LowerMargin);
        }

        internal override double GraphToCanvas(double canvas)
        {
            return GraphTransform(canvas) * Scale - Offset - AxisMargin.LowerMargin;
        }

        internal override double CanvasToGraph(double graph)
        {
            return CanvasTransform(graph / Scale + (Offset + AxisMargin.LowerMargin) / Scale);
        }
    }
}
