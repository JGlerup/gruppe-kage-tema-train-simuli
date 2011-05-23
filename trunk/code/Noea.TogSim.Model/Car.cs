using System;
using System.Collections.Generic;
using System.Text;

namespace Noea.TogSim.Model
{
	public class Car : ICar
	{

		private int _id;
		private string _desc;
		private double _speed;
		private double _length;

		public Car(int id)
		{
			Init(id,null, 0, 0);
		}

		public Car (int id, string desc)
		{
			Init(id, desc, 0, 0);
		}

		public Car (int id, string desc, double speed, double length)
		{
			Init(id, desc, speed, length);
		}


		private void  Init(int id, string desc, double speed, double length)
		{
			_id=id;
			_desc=desc;
			_speed=speed;
			_length=length;
		}
 

		#region ICar Members

		public int Id
		{
			get { return _id; }
		}

		public string Description
		{
			get
			{
				return _desc;
			}
			set
			{
				_desc=value;
			}
		}

		public double MaxSpeed
		{
			get
			{
				return _speed;
			}
			set
			{
				_speed=value;
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
				_length=value;
			}
		}

		#endregion
}
}
