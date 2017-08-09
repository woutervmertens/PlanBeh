using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace PlanBeh.Models
{
    public class Arrow : Shape
    {
        public static readonly DependencyProperty ArrowHeadLengthProperty =
            DependencyProperty.Register("ArrowHeadLength", typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowHeadWidthProperty =
            DependencyProperty.Register("ArrowHeadWidth", typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DotSizeProperty =
            DependencyProperty.Register("DotSize", typeof(double), typeof(Arrow),
                new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(Point), typeof(Arrow),
                new FrameworkPropertyMetadata(new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(Point), typeof(Arrow),
                new FrameworkPropertyMetadata(new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsRender));

        public double ArrowHeadLength
        {
            get
            {
                return (double)GetValue(ArrowHeadLengthProperty);
            }
            set
            {
                SetValue(ArrowHeadLengthProperty, value);
            }
        }

        public double ArrowHeadWidth
        {
            get
            {
                return (double)GetValue(ArrowHeadWidthProperty);
            }
            set
            {
                SetValue(ArrowHeadWidthProperty, value);
            }
        }

        public double DotSize
        {
            get
            {
                return (double)GetValue(DotSizeProperty);
            }
            set
            {
                SetValue(DotSizeProperty, value);
            }
        }

        public Point Start
        {
            get
            {
                return (Point)GetValue(StartProperty);
            }
            set
            {
                SetValue(StartProperty, value);
            }
        }

        public Point End
        {
            get
            {
                return (Point)GetValue(EndProperty);
            }
            set
            {
                SetValue(EndProperty, value);
            }
        }

        protected override Geometry DefiningGeometry {
            get
            {
                LineGeometry geometry = new LineGeometry();
                geometry.StartPoint = this.Start;
                geometry.EndPoint = this.End;

                GeometryGroup group = new GeometryGroup();
                group.Children.Add(geometry);

                GenerateArrowHeadGeometry(group);

                return group;
            }
        }

        private void GenerateArrowHeadGeometry(GeometryGroup geometryGroup)
        {
            EllipseGeometry ellipse = new EllipseGeometry(this.Start, DotSize, DotSize);
            geometryGroup.Children.Add(ellipse);

            Vector startDir = this.End - this.Start;
            startDir.Normalize();
            Point basePoint = this.End - (startDir * ArrowHeadLength);
            Vector crossDir = new Vector(-startDir.Y, startDir.X);

            Point[] arrowHeadPoints = new Point[3];
            arrowHeadPoints[0] = this.End;
            arrowHeadPoints[1] = basePoint - (crossDir * (ArrowHeadWidth / 2));
            arrowHeadPoints[2] = basePoint + (crossDir * (ArrowHeadWidth / 2));

            PathFigure arrowHeadFig = new PathFigure();
            arrowHeadFig.IsClosed = true;
            arrowHeadFig.IsFilled = true;
            arrowHeadFig.StartPoint = arrowHeadPoints[0];
            arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[1], true));
            arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[2], true));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(arrowHeadFig);

            geometryGroup.Children.Add(pathGeometry);
        }
    }
}