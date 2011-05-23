using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Noea.TogSim.Model
{
    public class TrainEngine : ITrainEngine
    {
        ITrainSet _train;
        Queue _tracks = new Queue();
        double _trackLength;
        double _trackLeft;
        long _prevTime;
        double _elapTime;

        int _interval;
        double _updateFreq;
        bool _isRunning;

        double _acceleration;

        public double Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        public TrainEngine(ITrainSet train, ITrack track)
        {
            Init(train, track);
        }

        private void Init(ITrainSet train, ITrack track)
        {
            UpdateFrequency = 13;
            _elapTime = 0;
            _train = train;
            Train.OnReverse += InvertDirection;
            _trackLength = track.Length / 2; //The train is placed in the middle
            TrackCounter tcount = new TrackCounter(track, Train.Length + _trackLength);
            track.IsBlocked = true;
            track.Train = Train;
            ArrayList trackList = new ArrayList();
            trackList.Add(tcount);
            ITrack t = track.Previous;

            while (t != null && Train.Length > _trackLength)
            {
                t.Train = Train;
                t.IsBlocked = true;
                trackList.Add(new TrackCounter(t, Train.Length - _trackLength));
                _trackLength += t.Length;
                t = t.Previous;
            }
            _trackLeft = _trackLength - Train.Length;

            QueueInvert(trackList.ToArray());
        }
        private void QueueInvert()
        {
            QueueInvert(Tracks.ToArray());
        }
        private void QueueInvert(Object[] trackList)
        {
            Tracks.Clear();
            for (int i = trackList.Length - 1; i >= 0; i--)
            {
                Tracks.Enqueue(trackList[i]);
            }

        }
        public ITrainSet Train
        {
            get { return _train; }
        }
        public Queue Tracks
        {
            get { return _tracks; }
        }
        public int Interval
        {
            get { return _interval; }
        }
        public double UpdateFrequency
        {
            get { return _updateFreq; }
            set
            {
                _updateFreq = value;
                if (_updateFreq <= 0)
                {
                    _interval = Timeout.Infinite;
                }
                else
                {
                    _interval = (int)(1000 / _updateFreq);
                }
            }
        }
        public long PrevTime
        {
            get { return _prevTime; }
            set { _prevTime = value; }
        }
        public double ElapsedTime
        {
            get { return _elapTime; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }
        public double TrackPos
        {
            get
            {
                double pos;

                while (!Monitor.TryEnter(this, 10))
                {
                    throw new Exception("Position locked");
                }
                {
                    if (Train.CurrentTrack.GetNext(Train.PreviousTrack) == Train.CurrentTrack.Next)
                    {
                        pos = Train.CurrentTrack.Length - _trackLeft;
                    }
                    else
                    {
                        pos = _trackLeft;
                    }
                    //Error correction to model
                    if (pos < 0) pos = 0;
                    if (pos > Train.CurrentTrack.Length) pos = Train.CurrentTrack.Length;

                }
                Monitor.Exit(this);
                return pos;
            }
        }
        public ITrack[] TracksList()
        {
            ITrack[] trackArray = new ITrack[Tracks.Count];
            Object[] tcArray = Tracks.ToArray();
            for (int i = 0; i < tcArray.Length; i++)
            {
                trackArray[tcArray.Length - 1 - i] = ((TrackCounter)tcArray[i]).Track;
            }
            return trackArray;
        }
        public void Start()
        {
            _isRunning = true;
        }
        public void Start(double updateFrequency)
        {
            UpdateFrequency = updateFrequency;
            Start();
        }
        public void Stop()
        {
            _isRunning = false;
        }

        public void UpdatePosition(Object obj)
        {
            //Console.WriteLine("Tog {0} opdateres", Train.Id);
            long now;
            if (obj is DateTime)
            {
                now = ((DateTime)obj).Ticks;
            }
            else
            {
                now = DateTime.Now.Ticks;
            }

            double secs = (now - PrevTime) / 1e7;
            UpdatePosition(secs);
            PrevTime = now;
        }

        public void UpdatePosition(double secs)
        {
            while (!Monitor.TryEnter(this, 10))
                Console.WriteLine("UpdatePosition venter på monitor");
            { // Start monitor blok. {} is not mandatory. 
                _elapTime += secs;
                _train.ActualSpeed += this.Acceleration * secs;
                if (Math.Abs(_train.ActualSpeed) > 0.1)
                {
                    double distance = Train.ActualSpeed * secs;

                    //Console.WriteLine("Engine.Update: Flyt tog Secs: " + secs + " dist: " + distance);
                    _trackLeft -= distance;
                    bool found = false;
                    while (!found && Tracks.Count > 0)
                    {
                        if (_trackLeft <= 0)
                        {
                            RemoveTrack();
                            AddTracks();
                            TrackCounter tc = (TrackCounter)Tracks.Peek();
                            _trackLeft += tc.Track.Length;
                        }
                        else
                        {
                            found = true;
                        }
                    }

                    AddTracks();
                }
            }//End monitor blok 
            Monitor.Exit(this);
        }

        private void RemoveTrack()
        {
            TrackCounter tc = (TrackCounter)Tracks.Dequeue();
            tc.Track.IsBlocked = false;
            tc.Track.Train = null;
            _trackLength -= tc.Track.Length;
        }
        private void AddTracks()
        {
            try
            {
                while (Train.Length > _trackLength)
                {
                    AddOneTrack();
                }
            }
            catch
            {
                Train.Reverse = !Train.Reverse;
            }
        }
        private void AddOneTrack()
        {
            ITrack t;

            t = Train.CurrentTrack.GetNext(Train.PreviousTrack);
            if (t != null)
            {
                Tracks.Enqueue(new TrackCounter(t, Train.Length + t.Length));
                t.Train = Train;
                t.IsBlocked = true;
                _trackLength += t.Length;
                Train.CurrentTrack = t;
            }
            else
            {
                throw new Exception("No tracks available");
            }
        }

        //Handler for change in direction
        public void InvertDirection(ITrainSet ts, ITrainEventArgs args)
        {
            if (Tracks.Count <= 1)
            {
                ITrack tmp = Train.PreviousTrack;
                Train.PreviousTrack = Train.CurrentTrack;
                Train.CurrentTrack = tmp;

                AddTracks();
            }
            else
            {
                TrackCounter tc = (TrackCounter)Tracks.Peek();
                Object[] tracks = Tracks.ToArray();
                TrackCounter c, p;
                c = (TrackCounter)tracks[0];
                p = (TrackCounter)tracks[1];
                Train.CurrentTrack = c.Track;
                Train.PreviousTrack = p.Track;

            }
            QueueInvert();
            _trackLeft = _trackLength - Train.Length - _trackLeft;
        }

    }

    class TrackCounter
    {
        ITrack _track;
        double _counter;

        public TrackCounter(ITrack track, double c)
        {
            _track = track;
            _counter = c;
        }

        public ITrack Track
        {
            get { return _track; }
        }
        public double Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }
    }
}
