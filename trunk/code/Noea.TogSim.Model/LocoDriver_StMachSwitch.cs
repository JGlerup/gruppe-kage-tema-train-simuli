using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Noea.TogSim.Model
{
    public class LocoDriver_StMachSwitch : ILocoDriver, ILocoDriverExtended
    {
        enum STATE
        {
            STOPSIGNAL_AHEAD_STATE, 
            LINE_IS_CLEAR_STATE,
        }

        private STATE _state = STATE.LINE_IS_CLEAR_STATE;
        private ITrainSet _train;

        private double _acceleration = 0;
        private double _deacceleration = 0;
        private long _interval = 0;
        
        public LocoDriver_StMachSwitch(ITrainSet train)
        {
            _train = train;
            _train.OnTrack += TrackHasChanged;
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
        
        public string StateDescription
        {
            get
            {
                string stateDescription;
                if (_state == STATE.LINE_IS_CLEAR_STATE)
                {
                    stateDescription = "LINE_IS_CLEAR_STATE";
                }
                else
                {
                    stateDescription = "STOPSIGNAL_AHEAD_STATE";
                }
                return stateDescription;
            }
        }
        
        public long Interval
        {
            get { return _interval; }
        }

        public void UpdateState(Object obj)
        {
            switch(_state)
            {
                case STATE.LINE_IS_CLEAR_STATE :
                    if (StopSignalAhead())
                    {
                        _state = STATE.STOPSIGNAL_AHEAD_STATE;
                    }
                    else
                    {
                        _train.ActualSpeed = _train.RequestedSpeed;

                    }
                    break;
                case STATE.STOPSIGNAL_AHEAD_STATE :
                    if (StopSignalAhead())
                    {
                        if (_train.ActualSpeed > 0)
                        {
                            _train.ActualSpeed = 0;
                        }
                    }
                    else
                    {
                        _state = STATE.LINE_IS_CLEAR_STATE;
                    }
                    break;
            }            
        }

        public void TrackHasChanged(ITrainSet train, ITrainEventArgs args)
        {
            if (StopSignalAhead())
            {
                _state = STATE.STOPSIGNAL_AHEAD_STATE;
            }
        }

        public void Stop()
        {
            _state = STATE.STOPSIGNAL_AHEAD_STATE;
        }
        public void Go()
        {
            _state = STATE.LINE_IS_CLEAR_STATE;

        }

        public bool StopSignalAhead()
        {
            ITrack currentTrack = Train.CurrentTrack;
            ITrack previousTrack = Train.PreviousTrack;
            bool found = false;
            int count = 0;
            while (count < 3 && !found && currentTrack.GetNext(previousTrack) != null)
            {
                if (currentTrack.Signals.Count > 0)
                {
                    found = TrackHasStopSignal(currentTrack, previousTrack);
                }
                count++;
                ITrack temp = currentTrack;
                currentTrack = temp.GetNext(previousTrack);
                previousTrack = temp;
            }
            return found;
        }
        
        public bool TrackHasStopSignal(ITrack currentTrack, ITrack previousTrack)
        {
            bool found = false;
            int index = 0;
            while (!found && index < currentTrack.Signals.Count)
            {
                ISignal s = (ISignal)currentTrack.Signals[index];
                if (s.State == SimpleSignal.Stop)
                {
                    found = true;
                    if (s is OneViewSignal && ((OneViewSignal)s).TrackDirection != currentTrack.GetNext(previousTrack))
                    {
                        found = false;
                    }
                }
                index++;
            }
            return found;
        }

        #region ILocoDriverExtended Members


        public double SlowSpeed
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void TrackHandler(ITrainSet train, ITrainEventArgs args)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public double UpdateFrequency
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
  


}