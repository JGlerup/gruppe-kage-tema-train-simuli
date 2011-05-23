using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Noea.TogSim.Model
{
    public class LocoDriver_StPattern : ILocoDriver, ILocoDriverExtended
    {
        public const double Unfinite = -1.0;
        ITrainSet _train;
        double _acceleration;
        double _deacceleration = 0;
        double _slowSpeed;
        double _currentSpeed;
        double _desiredSpeed;
        long _interval;
        double _updateFreq;
        Timer _timer;
        LocoState _currentState;
        LocoState _previousState;


        DateTime _startAccTime;


        public LocoDriver_StPattern(ITrainSet train)
        {
            Init(train, 3, 3, 0, 10);
        }
        public LocoDriver_StPattern(ITrainSet train, double acceleration, double deacceleration, double slowSpeed, double interval)
        {
            Init(train, acceleration, deacceleration, slowSpeed, interval);
        }

        private void Init(ITrainSet train, double acceleration, double deacceleration, double slowSpeed, double interval)
        {
            _train = train;
            _train.OnTrack += TrackHandler;
            Acceleration = acceleration;
            Deacceleration = deacceleration;
            SlowSpeed = slowSpeed;
            UpdateFrequency = interval;
            _currentState = new RunningState();
            PreviousState = null;
            //Console.WriteLine("State interval er: {0}", Interval);
           // _timer = new Timer(new TimerCallback(this.UpdateState), null, 0, Interval);
        }

        public ITrainSet Train
        {
            get { return _train; }
        }
        public double Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        public double Deacceleration
        {
            get { return _deacceleration; }
            set { _deacceleration = value; }
        }
        public double SlowSpeed
        {
            get { return _slowSpeed; }
            set { _slowSpeed = value; }
        }
        public string StateDescription
        {
            get { return CurrentState.ToString(); }
        }
        private LocoState CurrentState
        {
            get { return _currentState; }
            set
            {
                LocoState state = value;
                if (!CurrentState.Equals(state))
                {
             //       _timer.Change(0, 0);
                    PreviousState = CurrentState;
                    _currentState = state;
                    //Console.WriteLine("Skifter til " + state.Description);
               //     _timer.Change(Interval, Interval);
                }
            }
        }
        private LocoState PreviousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }

        public long Interval
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
                    _interval = (long)(1000 / _updateFreq);
                }
            }
        }
        public void UpdateState(Object obj)
        {
            CurrentState = CurrentState.Handle(this);
        }

        //public void TrackHandler(ITrainSet train, ITrainEventArgs args)
        //{
        //    ITrack track = Train.CurrentTrack;
        //    //Console.WriteLine("TrackHandler " + DateTime.Now+ " train(" + Train.Id + ") er på spor: " + Train.CurrentTrack.Id);
        //    bool found = false;
        //    int pos = 0;
        //    while (!found && pos < track.Signals.Count)
        //    {
        //        ISignal s = (ISignal)track.Signals[pos];
        //        if (s.State == SimpleSignal.Stop)
        //        {
        //            Console.WriteLine("" + DateTime.Now + ": Stop signal fundet. Stopper tog");
        //            found = true;
        //            Stop();
        //        }
        //        pos++;
        //    }
        //}
        //private LocoState State
        //{
        //    get { return _state; }
        //    set { _state = value; }
        //}
        public void TrackHandler(ITrainSet train, ITrainEventArgs args)
        {
            if (HaveStopSignal())
            {
                CurrentState = StateFactory.WaitSignalState;
            }
            if (HaveTrainInFront())
            {
                CurrentState = StateFactory.WaitBlockedTrackState;
            }            
        }

        private bool HaveTrainInFront()
        {
            //PQC: Rettet her.
            ITrack currentTrack = Train.CurrentTrack.GetNext(Train.PreviousTrack);
            ITrack previousTrack = Train.CurrentTrack;


            bool found = false;
            int count = 0;

            while (count < 2 && !found && currentTrack.GetNext(previousTrack) != null)
            {
                if (currentTrack.IsBlocked)
                {
                    Console.WriteLine("Train: "+Train.Id+"-> Tog nr. "+currentTrack.Train.Id+" blokerer spor: "+currentTrack.Id+" count: "+count);
                    
                    found = true;
                    
                }
                ITrack temp = currentTrack;
                currentTrack = temp.GetNext(previousTrack);
                previousTrack = temp;
                count++;
            }
            return found;
        }


        private bool HaveStopSignal()
        {
            ITrack currentTrack = Train.CurrentTrack;
            ITrack previousTrack = Train.PreviousTrack;
            bool found = false;
            int count = 0;
            while (count < 3 && !found && currentTrack.GetNext(previousTrack) != null)
            {
                if (currentTrack.Signals.Count > 0)
                {
                    found = HaveStopSignal(currentTrack, previousTrack);
                }
                count++;
                ITrack temp = currentTrack;
                currentTrack = temp.GetNext(previousTrack);
                previousTrack = temp;
            }
            return found;
        }
        private bool HaveStopSignal(ITrack currentTrack, ITrack previousTrack)
        {
            bool found = false;
            int pos = 0;
            while (!found && pos < currentTrack.Signals.Count)
            {
                ISignal s = (ISignal)currentTrack.Signals[pos];
                if (s.State == SimpleSignal.Stop)
                {
                    found = true;
                    if (s is OneViewSignal && ((OneViewSignal)s).TrackDirection != currentTrack.GetNext(previousTrack))
                    {
                        found = false;
                    }
                }
                pos++;
            }
            return found;
        }

        public void Stop()
        {
            CurrentState = new StoppedState();
            CurrentState.Handle(this);
        }
        public void Go()
        {
            CurrentState = new RunningState();
            CurrentState.Handle(this);
        }
        abstract class LocoState
        {
            protected String _description;
            public LocoState()
            {
                Console.WriteLine(this);
            }
            public String Description
            {
                get { return _description; }
            }
            public override string ToString()
            {
                return _description;
            }
            public abstract LocoState Handle(LocoDriver_StPattern ld);
        }

        //******************************HERTIL**************************
        //
        //**************************************************************
        class StateFactory
        {
            private static StoppedState _stoppedState;
            private static StoppingState _stoppingState;
            private static RunningState _runningState;
            private static AccelerateState _accelerateState;
            private static WaitSignalState _waitSignalState;
            private static WaitBlockedTrackState _waitBlockedTrackState;

            public static WaitBlockedTrackState WaitBlockedTrackState
            {
                get
                {
                    if (_waitBlockedTrackState == null) _waitBlockedTrackState = new WaitBlockedTrackState();
                    return _waitBlockedTrackState;

                }
            }

            public static StoppedState StoppedState
            {
                get
                {
                    if (_stoppedState == null) _stoppedState = new StoppedState();
                    return _stoppedState;
                }
            }
            public static StoppingState StoppingState
            {
                get
                {
                    if (_stoppingState == null) _stoppingState = new StoppingState();
                    return _stoppingState;
                }

            }
            public static RunningState RunningState
            {
                get
                {
                    if (_runningState == null) _runningState = new RunningState();
                    return _runningState;
                }
            }
            public static AccelerateState AccelerateState
            {
                get
                {
                    if (_accelerateState == null) _accelerateState = new AccelerateState();
                    return _accelerateState;
                }
            }
            public static WaitSignalState WaitSignalState
            {
                get
                {
                    if (_waitSignalState == null) _waitSignalState = new WaitSignalState();
                    return _waitSignalState;
                }
            }
        }
        class StoppedState : LocoState
        {
            public StoppedState()
                : base()
            {
                base._description = "Stopped";
            }
            public override LocoState Handle(LocoDriver_StPattern ld)
            {
                if (ld.Train.ActualSpeed <= 0.2)
                {
                    ld.Train.ActualSpeed = 0;
                    return this;
                }
                else
                {
                    return StateFactory.StoppingState;
                }
            }
        }
        class RunningState : LocoState
        {
            public RunningState()
                : base()
            {
                base._description = "Running";
            }
            public override LocoState Handle(LocoDriver_StPattern ld)
            {
                if (Math.Abs(ld.Train.ActualSpeed - ld.Train.RequestedSpeed) > 0.2)
                {
                    return StateFactory.AccelerateState;
                }
                else
                {
                    ld.Train.ActualSpeed = ld.Train.RequestedSpeed;
                    return this;
                }
            }
        }
        class AccelerateState : LocoState
        {
            public AccelerateState()
                : base()
            {
                base._description = "Accelerate";
            }
            public override LocoState Handle(LocoDriver_StPattern ld)
            {
                if (Math.Abs(ld.Train.ActualSpeed - ld.Train.RequestedSpeed) > 0.2)
                {
                    double acc = Math.Abs(ld.Train.RequestedSpeed - ld.Train.ActualSpeed) / 2;
                    if (ld.Train.ActualSpeed > ld.Train.RequestedSpeed)
                    {
                        ld.Train.Engine.Acceleration = (ld.Train.Deacceleration < acc) ? -ld.Deacceleration : -acc;
                    }
                    else
                    {
                        ld.Train.Engine.Acceleration = (ld.Acceleration < acc) ? ld.Acceleration : acc;
                    }
                    return this;
                }
                else
                {
                    ld.Train.Engine.Acceleration = 0.0;
                    ld.Train.ActualSpeed = ld.Train.RequestedSpeed;
                }
                    return ld.PreviousState;
            }
        }
        class StoppingState : LocoState
        {
            public StoppingState()
                : base()
            {
                base._description = "Stopping";
            }
            public override LocoState Handle(LocoDriver_StPattern ld)
            {
                if (ld.Train.ActualSpeed > 0.2)
                {
                    double acc = ld.Train.ActualSpeed / 2;
                    ld.Train.Engine.Acceleration = (ld.Train.Deacceleration < acc) ? -ld.Deacceleration : -acc;
                    return this;
                }
                else
                {
                    ld.Train.Engine.Acceleration = 0.0;
                    ld.Train.ActualSpeed = 0.0;
                }
                Console.WriteLine("Stopping returnerer {0}", ld.PreviousState.Description);
                return ld.PreviousState;
            }
        }
        class WaitSignalState : LocoState
        {
            LocoState _previous=null;
            public WaitSignalState()
                : base()
            {
                base._description = "Wait for signal";
            }
            public override LocoState Handle(LocoDriver_StPattern ld)
            {
                LocoState nextState;
                if (!ld.PreviousState.Equals(StateFactory.StoppingState) && !ld.PreviousState.Equals(this))
                {
                    _previous = ld.PreviousState;
                    Console.WriteLine("_previous: {0}", _previous.Description);
                }
                ITrack track = ld.Train.CurrentTrack;
                if (ld.HaveStopSignal())
                {
                    //Console.WriteLine("Stop signal fundet");
                    //ld.Train.ActualSpeed = 0;
                    //ld.Train.Acceleration = 0;
                    if (ld.Train.ActualSpeed > 0)
                    {
                        nextState = StateFactory.StoppingState;
                    }
                    else
                    {
                        nextState = this;
                    }
                }
                else
                {
                    nextState = _previous;
                }
                Console.WriteLine("WaitForSignal returnerer {0}", nextState.Description);
                return nextState;
            }
        }
        class WaitBlockedTrackState : LocoState
        {
            LocoState _previous = null;
            public WaitBlockedTrackState()
                : base()
            {
                base._description = "Wait for Train to moooove the arse";
            }
            public override LocoState Handle(LocoDriver_StPattern ld)
            {
                LocoState nextState;
                if (!ld.PreviousState.Equals(StateFactory.StoppingState) && !ld.PreviousState.Equals(this))
                {
                    _previous = ld.PreviousState;
                    Console.WriteLine("_previous: {0}", _previous.Description);
                }
                ITrack track = ld.Train.CurrentTrack;
                if (ld.HaveTrainInFront())
                {
                    //Console.WriteLine("Tog foran! noo");
                    //ld.Train.ActualSpeed = 0;
                    //ld.Train.Acceleration = 0;
                    if (ld.Train.ActualSpeed > 0)
                    {
                        nextState = StateFactory.StoppingState;
                    }
                    else
                    {
                        nextState = this;
                    }
                }
                else
                {
                    nextState = _previous;
                }
                Console.WriteLine("WaitBlockTrack returnerer {0}", nextState.Description);
                return nextState;
            }
        }
    }
}