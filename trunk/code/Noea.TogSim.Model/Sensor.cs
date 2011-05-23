using System;
using System.Collections;
using System.Text;

namespace Noea.TogSim.Model
{
	public class SwitchSensor:ISensor
	{
		bool _value;
        int _id;

        public int Id
        {
            get { return _id; }
        }

		public SwitchSensor(int id,bool value)
		{
            _id = id;
			_value = value;
		}
		#region ISensor Members

		public Object Value
		{
			get { return _value; }
			set {
				bool newValue = (Boolean) value;
				bool oldValue = _value;
				if (oldValue != newValue)
				{
					_value = newValue;
					SensorHandler tempEvent = OnChange;
					if (tempEvent != null)
					{
						SensorEventArgs args = new SensorEventArgs(this, oldValue, newValue);
						tempEvent(this, args);
					}
				}
			}
		}

		public event SensorHandler OnChange;

		#endregion
}
	public class SensorEventArgs : ISensorEventArgs
	{
		#region ISensorEventArgs Members

		ISensor _sensor;
		Object _oldValue;
		Object _newValue;

		public SensorEventArgs(ISensor s, object oldv, object newv)
		{
			_sensor = s;
			_oldValue = oldv;
			_newValue = newv;
		}
		public ISensor Sensor
		{
			get { return _sensor; }
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
/*	public class SwitchSensor:ISensor
	{
		ITrack _track;
		bool _value;

		public SwitchSensor(ITrack track, bool value)
		{
			_track = track;
			_value = value;
		}
		#region ISensor Members
		public ITrack Track
		{
			get { return _track; }
		}

		public bool Value
		{
			get { return _value; }
			set {
				bool newValue = value;
				bool oldValue = _value;
				if (oldValue != newValue)
				{
					_value = newValue;
					SensorHandler tempBlockEvent = OnChange;
					//Console.WriteLine(tempBlockEvent);
					if (tempBlockEvent != null)
					{
						SensorEventArgs args = new SensorEventArgs(Track, oldValue, newValue);
						tempBlockEvent(this, args);
					}
				}
			}
		}

		public event SensorHandler OnChange;

		#endregion
}
	public class SensorEventArgs : ISensorEventArgs
	{
		#region ISensorEventArgs Members

		ITrack _track;
		Object _oldValue;
		Object _newValue;

		public SensorEventArgs(ITrack track, object oldv, object newv)
		{
			_track = track;
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
}*/
}
