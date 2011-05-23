using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Noea.TogSim.Model;

namespace Noea.TogSim.Gui
{
    public abstract class TrackGeometry
    {
        private ITrack _track;
        private TrackImage _image;
        private PointF _startPoint;
        private PointF _endPoint;
        private double _startAngle;
        private double _length;

        private double _scale;
        private int _orientation;


        public TrackGeometry(ITrack track, PointF startPoint, double direction, int orientation, TrackImage image)
        {
            Init(track, startPoint, direction, orientation, image);
        }

        private void Init(ITrack track, PointF startPoint, double direction, int orientation, TrackImage image)
        {
            Track = track;
            StartPoint = startPoint;
            StartAngle = direction;
            _orientation = orientation;
            Image = image;
        }

        public static TrackGeometry Create(ITrack track, PointF startPoint, double direction, int orientation, TrackImage image)
        {
            TrackGeometry result;
            if (track.Angle != 0)
            {
                result = new CurveTrack(track, startPoint, direction, orientation, image);
            }
            else
            {
                result = new StraightTrack(track, startPoint, direction, orientation, image);
            }
            return result;
        }

        public ITrack Track
        {
            get { return _track; }
            set { _track = value; }
        }

        public TrackImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public PointF StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        public PointF EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        public double StartAngle
        {
            get { return _startAngle; }
            set { _startAngle = value; }
        }

        public abstract double EndAngle
        {
            get;
        }

        public double Scale
        {
            get { return _scale; }
            //set { _scale = value; }
        }

        public double Length
        {
            get { return _length; }
            //set { _length = value; }
        }

        public int Orientation
        {
            get { return _orientation; }
            //set { _orientation = value; }
        }

        public abstract PointF GetPosition(double posMeters);
        public abstract double GetAngle(double posMeters);

        public double ToRad(double deg)
        {
            return deg * Math.PI / 180;
        }
        public double ToDeg(double rad)
        {
            return rad * 180 / Math.PI;
        }
    }

    public class StraightTrack : TrackGeometry
    {
        public StraightTrack(ITrack track, PointF startPoint, double direction, int orientation, TrackImage image)
            : base(track, startPoint, direction, orientation, image)
        {
            EndPoint = GetPosition(Track.Length);
        }
        public override double EndAngle
        {
            get { return StartAngle; }
        }
        public override double GetAngle(double posMeters)
        {
            return StartAngle;
        }
        public override PointF GetPosition(double posMeters)
        {
            PointF p = new PointF();
            p.X = StartPoint.X + (float)(posMeters * Math.Cos(StartAngle + Math.PI / 2));
            p.Y = StartPoint.Y + (float)(posMeters * Math.Sin(StartAngle + Math.PI / 2));
           // Console.WriteLine("track geometry: p(x,y)=({0},{1})", p.X, p.Y);

            return p;
        }
    }
    public class CurveTrack : TrackGeometry
    {
        private double _radius;
        private double _angle;
        private float _centerX, _centerY;

        public CurveTrack(ITrack track, PointF startPoint, double direction, int orientation, TrackImage image)
            : base(track, startPoint, direction, orientation, image)
        {
            _angle = ToRad(Track.Angle);
            _radius = Track.Length / _angle;
            _centerX = StartPoint.X - (float)(_radius * Math.Cos(StartAngle ));
            _centerY = StartPoint.Y - (float)(_radius * Math.Sin(StartAngle ));

            EndPoint = GetPosition(Track.Length);
        }
        public override double EndAngle
        {
            get { return StartAngle+_angle; }
        }
        public override double GetAngle(double posMeters)
        {
            return StartAngle+posMeters/_radius;
        }
        public override PointF GetPosition(double posMeters)
        {
            PointF p = new PointF();
            double posAngle = GetAngle(posMeters);
            p.X = _centerX + (float)(_radius * Math.Cos(posAngle));
            p.Y = _centerY + (float)(_radius * Math.Sin(posAngle));
            return p;
        }
    }
    public class TrackImage
    {
        Bitmap _texture;

        public TrackImage(Bitmap t)
        {
            _texture = t;
        }

        public Bitmap Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
    }

}
