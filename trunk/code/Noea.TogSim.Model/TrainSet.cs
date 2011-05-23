using System;
using System.Collections;
using System.Text;

namespace Noea.TogSim.Model
{
	public class TrainSet : ITrainSet
	{

		int _id;
		string _description;
		double _length;
		ICar _firstCar;
		ICar _lastCar;
		ArrayList _cars;
		double _requestedSpeed;
		double _actualSpeed;
        double _acceleration;
        double _deacceleration;
		double _maxSpeed;
		ITrack _currentTrack;
		ITrack _previousTrack;
		bool _reverse;

		ILocoDriver _locoDriver;
		ITrainEngine _engine;


		public TrainSet(int id)
		{
			Init(id, "No description", null, false);
		}
		public TrainSet(int id, string description)
		{
			Init(id, description, null, false);
		}
		public TrainSet(int id, string description, ITrack currentTrack)
		{
			Init(id, description, currentTrack, false);
		}
		public TrainSet(int id, string description, ITrack currentTrack, bool reverse)
		{
			Init(id, description, currentTrack, reverse);
		}
		private void Init(int id, string description, ITrack currentTrack, bool reverse)
		{
			//Console.WriteLine("Opretter TrainSet: " + id);
			_id = id;
			_description = description;
			_length = -1;
			_firstCar = null;
			_lastCar = null;
			_cars = new ArrayList();
			_actualSpeed = 0;
            Acceleration = 9.81;
            Deacceleration = 19.62;
			_maxSpeed = -1;
			_currentTrack = currentTrack;
			_reverse = reverse;
		}

		#region ITrainSet Members

		public int Id
		{
			get { return _id; }
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		public int Count
		{
			get { return Cars.Count; }
		}

		public double Length
		{
			get
			{
				if (_length < 0)
				{
					_length = 0;
					foreach (ICar c in Cars)
					{
						_length += c.Length;
					}
				}
				return _length;
			}
		}

		public ICar FirstCar
		{
			get { return _firstCar; }
		}

		public ICar LastCar
		{
			get { return _lastCar; }
		}

		public IList Cars
		{
			get { return _cars; }
		}

		public double ActualSpeed
		{
			get
			{
				return _actualSpeed;
			}
			set
			{
				if (value > MaxSpeed)
				{
					throw new Exception("Can not set speed higher than MaxSpeed (" + MaxSpeed + "m/s)");
				}
				else
				{
					double newValue = value;
					double oldValue = _actualSpeed;
					if (!oldValue.Equals(newValue))
					{
						_actualSpeed = newValue;
						//Console.WriteLine("ActualHandler " + DateTime.Now + " train(" + Id + ") kører med hastighed: " + ActualSpeed);
						ITrainEventArgs args = new TrainEventArgs(this, oldValue, newValue);
						ChangeHandler tempCngEvent = OnChange;
						if (tempCngEvent != null)
						{
							tempCngEvent(this, args);
						}
					}
				}
			}
		}

		public double RequestedSpeed
		{
			get
			{
				return _requestedSpeed;
			}
			set
			{
				if (value > MaxSpeed)
				{
					throw new Exception("Can not set speed higher than MaxSpeed (" + MaxSpeed + "m/s)");
				}
				else
				{
					double newValue = value;
					double oldValue = _requestedSpeed;
					if (!oldValue.Equals(newValue))
					{
						_requestedSpeed = newValue;
						ITrainEventArgs args = new TrainEventArgs(this, oldValue, newValue);
						ChangeHandler tempCngEvent = OnChange;
						if (tempCngEvent != null)
						{
							tempCngEvent(this, args);
						}
					}
				}
			}
		}

		public double MaxSpeed
		{
			get
			{
				if (Cars.Count < 1) throw new Exception("Max. speed undefined. No cars in train set");
				if (_maxSpeed < 0)
				{
					_maxSpeed = ((ICar)Cars[0]).MaxSpeed;
					for (int i = 1; i < Cars.Count; i++)
					{
						if (_maxSpeed < Get(i).MaxSpeed) _maxSpeed = Get(i).MaxSpeed;
					}
				}
				return _maxSpeed;
			}
		}


		public bool Reverse
		{
			get
			{
				return _reverse;
			}
			set
			{
				bool newValue = (Boolean)value;
				bool oldValue = _reverse;
				if (oldValue != newValue)
				{
					_reverse = newValue;
					ITrainEventArgs args = new TrainEventArgs(this, oldValue, newValue);
					ReverseHandler tempEvent = OnReverse;
					if (tempEvent != null)
					{

						tempEvent(this, args);
					}
					ChangeHandler tempCngEvent = OnChange;
					if (tempCngEvent != null)
					{
						tempCngEvent(this, args);
					}
				}
			}
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

		public ITrack CurrentTrack
		{
			get { return _currentTrack; }
			set
			{
				ITrack newValue = value;
				ITrack oldValue = _currentTrack;
				if (!oldValue.Equals(newValue))
				{
					_previousTrack = _currentTrack;
					_currentTrack = newValue;
						//Console.WriteLine(""+DateTime.Now+ " train(" + Id + ") er på spor: " + _currentTrack.Id);

					ITrainEventArgs args = new TrainEventArgs(this, oldValue, newValue);
					TrackHandler tempEvent = OnTrack;
					if (tempEvent != null)
					{
						tempEvent(this, args);
					}
					ChangeHandler tempCngEvent = OnChange;
					if (tempCngEvent != null)
					{
						tempCngEvent(this, args);
					}
				}
			}

		}
		public ITrack PreviousTrack
		{
			get { return _previousTrack; }
            set { _previousTrack = value; }
		}
		public event ReverseHandler OnReverse;
		public event TrackHandler OnTrack;
		public event ChangeHandler OnChange;

		public void Add(ICar c)
		{
			_length = -1;
			_maxSpeed = -1;
			Cars.Add(c);
		}

		public void Insert(int index, ICar c)
		{
			_length = -1;
			_maxSpeed = -1;
			Cars.Insert(index, c);
		}

		public void Remove(ICar c)
		{
			_length = -1;
			_maxSpeed = -1;
			Cars.Remove(c);
		}

		public void Remove(int index)
		{
			_length = -1;
			_maxSpeed = -1;
			Cars.RemoveAt(index);
		}

		public int IndexOf(ICar c)
		{
			return Cars.IndexOf(c);
		}

		public ICar Get(int index)
		{
			return (ICar)Cars[index];
		}

		public ILocoDriver LocoDriver
		{
			get { return _locoDriver; }
			set { _locoDriver = value; }
		}

		public ITrainEngine Engine
		{
			get { return _engine; }
			set { _engine = value; }
		}
		#endregion
	}
	public class TrainEventArgs : ITrainEventArgs
	{
		#region ITrainEventArgs Members

		ITrainSet _train;
		Object _oldValue;
		Object _newValue;

		public TrainEventArgs(ITrainSet s, object oldv, object newv)
		{
			_train = s;
			_oldValue = oldv;
			_newValue = newv;
		}
		public ITrainSet Train
		{
			get { return _train; }
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
}
