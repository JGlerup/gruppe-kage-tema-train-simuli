using System;
using System.Collections.Generic;
using System.Text;

namespace Noea.TogSim.Model
{
    public abstract class Signal : ISignal
    {

        int _id;

        public Signal(int id)
        {
            _id = id;
        }

        #region ISignal Members

        public int Id
        {
            get { return _id; }
        }

        public abstract ISignalState State
        {
            get;
            set;

        }

        public abstract ISignal AssociatedSignal
        {
            get;
            set;
        }
        public virtual event SignalHandler OnChange;
        #endregion
    }
    public class SignalEventArgs : ISignalEventArgs
    {
        #region ISensorEventArgs Members

        ISignal _signal;
        Object _oldValue;
        Object _newValue;

        public SignalEventArgs(ISignal s, object oldv, object newv)
        {
            _signal = s;
            _oldValue = oldv;
            _newValue = newv;
        }
        public ISignal Signal
        {
            get { return _signal; }
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
    public class SimpleSignal : Signal
    {
        ISignalState _signalState;
        ISignal _associatedSignal;

        static ISignalState _goState = new LightSignalState(0, "Go", "green");
        static ISignalState _stopState = new LightSignalState(1, "Stop", "red");
        static ISignalState _waitState = new LightSignalState(2, "Wait", "yellow", true);

        public SimpleSignal(int Id, ISignalState state)
            : base(Id)
        {
            _signalState = state;
        }

        public SimpleSignal(int Id, ISignalState state, ISignal asignal)
            : base(Id)
        {
            _signalState = state;
            _associatedSignal = asignal;
        }
        public override ISignalState State
        {
            get
            {
                return _signalState;
            }
            set
            {
                //Console.WriteLine("Signal sættes til " + value);
                ISignalState newValue = value;
                ISignalState oldValue = _signalState;
                if (oldValue != newValue)
                {
                    _signalState = newValue;
                    SignalHandler tempEvent = OnChange;

                    //Console.WriteLine(OnChange.GetInvocationList());
                    if (tempEvent != null)
                    {
                        SignalEventArgs args = new SignalEventArgs(this, oldValue, newValue);
                        tempEvent(this, args);
                    }
                }
            }
        }

        public override ISignal AssociatedSignal
        {
            get
            {
                return _associatedSignal;
            }
            set
            {
                _associatedSignal = value;
            }
        }
        static public ISignalState Go
        {
            get { return _goState; }
        }
        static public ISignalState Stop
        {
            get { return _stopState; }
        }
        static public ISignalState Wait
        {
            get { return _waitState; }
        }
        public void SetGo()
        {
            State = _goState;
            //Console.WriteLine("Signal " + Id + ": GO");
        }

        public void SetStop()
        {
            State = _stopState;
            //Console.WriteLine("Signal " + Id + ": STOP");
        }

        public void SetWait()
        {
            State = _waitState;
            //Console.WriteLine("Signal " + Id + ": Wait");
        }
        public override event SignalHandler OnChange;
    }

    class OneViewSignal : SimpleSignal
    {
        ITrack _trackDirection;
        public OneViewSignal(int Id, ISignalState state, ITrack trackDirection)
            : base(Id, state)
        {
            TrackDirection = trackDirection;
        }

        public OneViewSignal(int Id, ISignalState state, ITrack trackDirection, ISignal asignal)
            : base(Id, state, asignal)
        {
            TrackDirection = trackDirection;
        }

        public ITrack TrackDirection
        {
            get { return _trackDirection; }
            set { _trackDirection = value; }
        }

    }
    public class SignalState : ISignalState
    {
        #region ISignalState Members

        private int _id;
        private string _desc;

        public SignalState(int id, string desc)
        {
            _id = id;
            _desc = desc;
        }

        public int Value
        {
            get
            {
                return _id;
            }

        }

        public string Description
        {
            get
            {
                return _desc;
            }
            set
            {
                _desc = value;
            }
        }

        #endregion
    }

    public class LightSignalState : SignalState
    {
        string _color;
        bool _blinking;
        public LightSignalState(int id, string desc)
            : base(id, desc)
        {
            _color = "black";
            _blinking = false;
        }

        public LightSignalState(int id, string desc, string color)
            : base(id, desc)
        {
            _color = color;
            _blinking = false;
        }

        public LightSignalState(int id, string desc, string color, bool blinking)
            : base(id, desc)
        {
            _color = color;
            _blinking = blinking;
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool Blinking
        {
            get { return _blinking; }
            set { _blinking = value; }
        }


    }

    public class SimpleSignalControl
    {
        SimpleSignal _signal1;
        private SimpleSignal _signal2;

        public SimpleSignalControl(SimpleSignal s1, SimpleSignal s2)
        {
            _signal1 = s1;
            _signal2 = s2;
        }
        public void ActOnSensor(ISensor sensor, ISensorEventArgs args)
        {
            if ((bool)args.NewValue)
            {
                _signal1.SetStop();
                _signal2.SetStop();
            }
            else
            {
                _signal1.SetGo();
                _signal2.SetGo();
            }
        }
    }

}
