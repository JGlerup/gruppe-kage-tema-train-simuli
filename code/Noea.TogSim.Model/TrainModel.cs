using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Noea.TogSim.Model
{
    public class TrainModel
    {
        ITrack _startTrack;
        ArrayList _trains;
        ArrayList _sensors;
        ITrainEngineDispatch _trainDispatcher;
        public TrainModel()
        {
            _trains = new ArrayList();
            _sensors = new ArrayList();
            _startTrack = GenerateTestRailroad();

            ITrainSet train;
            train = GenerateTestTrain(1, "Tog 1", 6, 35, _startTrack);
            _trains.Add(train);
            train = GenerateTestTrain(3, "My Train", 4, 10, _startTrack.Next.Next.Next.Next.Next);
            _trains.Add(train);
            //train = GenerateTestTrain(2, "Tog 2", 3, 10, _startTrack.Next.Next.Next.Next);
            //_trains.Add(train);

            /**** The dispatcher is instansed here *****/
            _trainDispatcher = new FixedTimeDispatch(_trains);

            //_trainDispatcher.Create();
        }
        private static ITrainSet GenerateTestTrain(int id, String desc, int numCars, float speed, ITrack startTrack)
        {
            ITrainSet train = new TrainSet(id, desc, startTrack.Next.Next.Next);


            for (int i = 0; i < numCars; i++)
            {
                train.Add(new Car(i, "vogn " + i, 40, 8));
            }
            //*****  Loco driver here: LocoDriver_StPattern or LocoDriver_StMachSwitch
            /**** TrainDriver is instanced here *****/
            train.LocoDriver = new LocoDriver_StPattern(train);
       
            train.Engine = new TrainEngine(train, train.CurrentTrack);
            train.RequestedSpeed = speed;
            return train;
        }



        private ITrack GenerateTestRailroad()
        {
            SwitchTrack start = new SwitchTrack(0, false, 50, 30, null, null, SwitchTrack.Left);

            ITrack prev = start;
            ITrack next = null;
            for (int i = 1; i < 12; i++)
            {
                next = new SimpleTrack(i, false, 50, 30, null, prev);
                prev.Next = next;
                if (prev == start) start.LeftTrack = next;
                prev = next;
            }
            
            next.Next = start;
            start.Previous = next;
            start.TrunkTrack = next;

            _startTrack = start;

            ITrack right = new SimpleTrack(12, false, 20, null, start);
            prev = right;
            for (int i = 13; i < 20; i++)
            {
                next = new SimpleTrack(i, false, 50, -30, null, prev);
                prev.Next = next;
                prev = next;
            }
            start.RightTrack = right;
            start.LeftTrack = start.Next;
            ISignal signal = new SimpleSignal(1, SimpleSignal.Go);
            start.RightTrack.Next.Signals.Add(signal);
            SwitchSensor sensor = new SwitchSensor(1, false);
            _sensors.Add(sensor);
            SimpleSignalControl sc = new SimpleSignalControl((SimpleSignal)signal);
            sensor.OnChange += sc.ActOnSensor;
            signal = new OneViewSignal(2, SimpleSignal.Go, start.LeftTrack.Next.Next.Next);
            start.LeftTrack.Next.Next.Signals.Add(signal);

            sensor = new SwitchSensor(2, false);
            sc = new SimpleSignalControl((SimpleSignal)signal);
            sensor.OnChange += sc.ActOnSensor;
            start.LeftTrack.Next.Next.Next.Next.Sensors.Add(sensor);
            return start;
        }

        public void Dispose()
        {
            foreach (ITrainSet t in TrainSets)
            {
                t.Engine.Stop();
            }
            _trainDispatcher.Dispose();
        }

        public void Start()
        {
            _trainDispatcher.Create();

            foreach (ITrainSet train in _trains)
            {
                train.LocoDriver.Go();
            }
        }
        public ITrack StartTrack
        {
            get { return _startTrack; }
        }
        public ArrayList TrainSets
        {
            get { return _trains; }
        }
        public ArrayList Sensors
        {
            get { return _sensors; }
        }

    }
}
