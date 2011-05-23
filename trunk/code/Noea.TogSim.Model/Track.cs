using System;
using System.Collections;
using System.Text;

namespace Noea.TogSim.Model
{
    public class SimpleTrack : ITrack
    {
        int _id;
        bool _blocked;
        double _length;
        double _angle;
        ITrack _nextTrack;
        ITrack _previousTrack;
        ITrainSet _train;
        ArrayList _signals;
        ArrayList _sensors;


        public SimpleTrack(int id)
        {
            Init(id, false, 0, 0, null, null, null, null);
        }
        public SimpleTrack(int id, bool blocked)
        {
            Init(id, blocked, 0, 0, null, null, null, null);
        }
        public SimpleTrack(int id, bool blocked, double length)
        {
            Init(id, blocked, length, 0, null, null, null, null);
        }
        public SimpleTrack(int id, bool blocked, double length, ITrack next, ITrack prev)
        {
            Init(id, blocked, length, 0, next, prev, null, null);
        }
        public SimpleTrack(int id, bool blocked, double length, double angle, ITrack next, ITrack prev)
        {
            Init(id, blocked, length, angle, next, prev, null, null);
        }
        public SimpleTrack(int id, bool blocked, double length, double angle, ITrack next, ITrack prev, ISignal sig)
        {
            Init(id, blocked, length, angle, next, prev, sig, null);
        }
        private void Init(int id, bool blocked, double length, double angle, ITrack next, ITrack prev, ISignal sig, ISensor sensor)
        {
            _id = id;
            _blocked = blocked;
            _length = length;
            _angle = angle;
            _nextTrack = next;
            _previousTrack = prev;
            _train = null;
            _signals = new ArrayList();
            _sensors = new ArrayList();
            if (sig != null) Signals.Add(sig);
            if (sensor != null) Sensors.Add(sensor);
        }

        #region ITrack Members

        public int Id
        {
            get { return _id; }
        }


        public bool IsBlocked
        {
            get
            {
                return _blocked;
            }
            set
            {
                bool newValue = (Boolean)value;
                bool oldValue = _blocked;
                if (oldValue != newValue)
                {
                    _blocked = newValue;
                    BlockHandler tempBlockEvent = OnBlock;
                    if (tempBlockEvent != null)
                    {
                        ITrackEventArgs args = new TrackEventArgs(this, oldValue, newValue);
                        tempBlockEvent(this, args);
                    }
                    foreach (ISensor s in Sensors)
                    {
                        if (s is SwitchSensor) s.Value = newValue;
                    }
                    FireChange(oldValue, newValue);
                }
            }
        }

        protected void FireChange(Object oldValue, Object newValue)
        {
            TrackChangeHandler tempChangeEvent = OnChange;
            if (tempChangeEvent != null)
            {
                ITrackEventArgs args = new TrackEventArgs(this, oldValue, newValue);
                tempChangeEvent(this, args);
            }
        }

        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
            }
        }

        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
            }
        }

        public virtual ITrack Next
        {
            get
            {
                return _nextTrack;
            }
            set
            {
                _nextTrack = value;
            }
        }

        public virtual ITrack Previous
        {
            get
            {
                return _previousTrack;
            }
            set
            {
                _previousTrack = value;
            }
        }
        public virtual ITrack[] NextList
        {
            get
            {
                ITrack[] a = { Next };
                return a;
            }
        }

        public virtual ITrack GetNext(ITrack previousTrack)
        {
            //if (previousTrack == null)
            //{
            //    if (this.Next != null) return this.Next;
            //    return this.Previous;
            //}
            if (previousTrack == null || this.Previous.Equals(previousTrack))
            {
                return this.Next;
            }
            else
            {
                return this.Previous;
            }
        }

        public ITrack[] PreviousList
        {
            get
            {
                ITrack[] a = { Previous };
                return a;
            }
        }

        public ITrainSet Train
        {
            get
            {
                return _train;
            }
            set
            {
                _train = value;
            }
        }
        public IList Signals
        {
            get { return _signals; }
        }

        public IList Sensors
        {
            get { return _sensors; }
        }


        public void AddSignal(ISignal s)
        {
            Signals.Add(s);
        }

        public void RemoveSignal(ISignal s)
        {
            Signals.Remove(s);
        }

        public void RemoveSignal(int index)
        {
            Signals.RemoveAt(index);
        }

        public ISignal GetSignal(int index)
        {
            return (ISignal)Signals[index];
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, Next: {1}, Previous: {2}"
                , this.Id, (this.Next != null) ? this.Next.Id.ToString() : "NaN", (this.Previous != null) ? this.Previous.Id.ToString() : "NaN");
        }
        public event BlockHandler OnBlock;
        public event TrackChangeHandler OnChange;

        #endregion
    }
    public class SwitchTrack : SimpleTrack, ITrack
    {
        public const int Right = 0;
        public const int Left = 1;
        int _direction;
        ITrack _rightTrack;
        ITrack _leftTrack;
        ITrack _trunkTrack;
        ITrack _next;
        ITrack _previous;

        SwitchTrack(int id)
            : base(id)
        {
        }
        public SwitchTrack(int id, bool blocked, int direction)
            : base(id, blocked)
        {
            Direction = direction;
        }
        public SwitchTrack(int id, bool blocked, double length, int direction)
            : base(id, blocked, length)
        {
            Direction = direction;
        }
        public SwitchTrack(int id, bool blocked, double length, ITrack next, ITrack prev, int direction)
            : base(id, blocked, length, next, prev)
        {
            Direction = direction;
        }
        public SwitchTrack(int id, bool blocked, double length, double angle, ITrack next, ITrack prev, int direction)
            : base(id, blocked, length, angle, next, prev)
        {
            Direction = direction;
        }
        public SwitchTrack(int id, bool blocked, double length, double angle, ITrack next, ITrack prev, ISignal sig, int direction)
            : base(id, blocked, length, angle, next, prev, sig)
        {
            Direction = direction;
        }
        public int Direction
        {
            get { return _direction; }
            set
            {
                if (!IsBlocked)
                {
                    int newValue = value;
                    int oldValue = _direction;

                    if (oldValue != newValue)
                    {
                        _direction = newValue;
                        FireChange(oldValue, newValue);
                    }
                }
                else
                {
                    throw (new Exception("Track is blocked. Direction is not changed"));
                }
            }
        }
        public ITrack RightTrack
        {
            get { return _rightTrack; }
            set { _rightTrack = value; }
        }
        public ITrack LeftTrack
        {
            get { return _leftTrack; }
            set { _leftTrack = value; }
        }
        public ITrack TrunkTrack
        {
            get { return _trunkTrack; }
            set { _trunkTrack = value; }
        }
        public override ITrack Next
        {
            get
            {
                if (_next == TrunkTrack) return _next;
                if (_direction == 0)
                {
                    return RightTrack;
                }
                else
                {
                    return LeftTrack;
                }
            }
            set
            { // Handle as a simple track
                _next = value;
                //if (_next == TrunkTrack) return _next;
                if (_direction == 0)
                {
                    RightTrack = value;
                }
                else
                {
                    LeftTrack = value;
                }
            }
        }
        //public override ITrack Previous
        //{
        //    get
        //    {
        //        if (_previous == TrunkTrack) return _previous;
        //        if (_direction == 0)
        //        {
        //            return RightTrack;
        //        }
        //        else
        //        {
        //            return LeftTrack;
        //        }
        //    }
        //    set
        //    { // Handle as a simple track
        //        _previous = value;
        //    }
        //}
        public override ITrack GetNext(ITrack previousTrack)
        {
            //Console.WriteLine("id: {0}: TrunkTrack={1} og previousTrack={2}", this.Id, this.TrunkTrack.Id, previousTrack.Id);
            if (this.Next != this.TrunkTrack) return base.GetNext(previousTrack);
            if (previousTrack != this.TrunkTrack) return TrunkTrack;
            if (this.Direction == Left) return this.LeftTrack;
            return this.RightTrack;



        }
        public void Toggle()
        {
            Direction = Math.Abs(Direction - 1); //Uha, spidsfindighederne tager ingen ende!!
        }
        public override ITrack[] NextList
        {
            get
            {
                ITrack[] result;
                if (Previous == TrunkTrack)
                {
                    ITrack[] a = { RightTrack, LeftTrack };
                    result = a;
                }
                else
                {
                    if (Previous == LeftTrack)
                    {
                        ITrack[] a = { TrunkTrack, RightTrack };
                        result = a;
                    }
                    else
                    {
                        ITrack[] a = { TrunkTrack, LeftTrack };
                        result = a;
                    }
                }
                return result;
            }
        }
        public override string ToString()
        {
            return base.ToString()+String.Format("\nTrunk: {0} Left: {1} Right: {2}",this.TrunkTrack,this.LeftTrack,this.RightTrack);
        }
    }
    public class TrackEventArgs : ITrackEventArgs
    {
        #region ITrackEventArgs Members

        ITrack _track;
        Object _oldValue;
        Object _newValue;

        public TrackEventArgs(ITrack t, object oldv, object newv)
        {
            _track = t;
            _oldValue = oldv;
            _newValue = newv;
        }
        public ITrack Track
        {
            get { return _track; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

        public object NewValue
        {
            get { return _newValue; }
        }

        #endregion
    }
    public class Railroad
    {
        public static ArrayList TrackList(ITrack start)
        {
            ArrayList trackList = new ArrayList();
            TrackTraverse(start, trackList);
            return trackList;
        }
        private static void TrackTraverse(ITrack start, ArrayList trackList)
        {

            if (start != null && !trackList.Contains(start))
            {
                //Console.WriteLine("traver spor: " + start.Id);
                trackList.Add(start);
                for (int i = 0; i < start.NextList.Length; i++)
                {
                    TrackTraverse(start.NextList[i], trackList);
                }
            }

        }

    }
}
