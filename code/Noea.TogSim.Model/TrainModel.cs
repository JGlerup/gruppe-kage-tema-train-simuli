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
            train = GenerateTestTrain(2, "Tog 2", 3, 40, _startTrack.Next.Next.Next.Next.Next.Next.Next.Next);
            _trains.Add(train);

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
            SwitchTrack start = new SwitchTrack(0, false, 50, 0, null, null, SwitchTrack.Left);

            ITrack prev = start;
            ITrack next = null;
            for (int i = 1; i < 7; i++)
            {
                next = new SimpleTrack(i, false, 50, 30, null, prev);
                prev.Next = next;
                if (prev == start) start.LeftTrack = next;
                prev = next;
            }

            for (int i = 7; i < 9; i++)
            {
                next = new SimpleTrack(i, false, 50, 0, null, prev);
                prev.Next = next;
                if (prev == start) start.LeftTrack = next;
                prev = next;
            }

            for (int i = 9; i < 15; i++)
            {
                next = new SimpleTrack(i, false, 50, 30, null, prev);
                prev.Next = next;
                if (prev == start) start.LeftTrack = next;
                prev = next;
            }

            SwitchTrack nextOne = new SwitchTrack(15, false, 50, 0, start, prev, SwitchTrack.Left);
            //SwitchTrack nextOne = next;
            next = nextOne;
            prev.Next = next;
            if (prev == start) start.LeftTrack = next;
            prev = next;

            next.Next = start;
            start.Previous = next;
            start.TrunkTrack = next;
            nextOne.TrunkTrack = next;

            _startTrack = start;

            ITrack right = new SimpleTrack(16, false, 50, -30, null, start);
            prev = right;

            for (int i = 17; i < 22; i++)
            {
                next = new SimpleTrack(i, false, 50, -30, null, prev);
                prev.Next = next;
                prev = next;
            }

            for (int i = 22; i < 24; i++)
            {
                next = new SimpleTrack(i, false, 50, 0, null, prev);
                prev.Next = next;
                prev = next;
            }

            for (int i = 24; i < 29; i++)
            {
                next = new SimpleTrack(i, false, 50, -30, null, prev);
                prev.Next = next;
                prev = next;
            }

            //next = new SimpleTrack(29, false, 50, -30, nextOne, prev);
            //prev.Next = next;
            //prev = next;

            start.RightTrack = right;
            start.LeftTrack = start.Next;
            //ISignal signal = new SimpleSignal(1, SimpleSignal.Go);
            //start.RightTrack.Next.Signals.Add(signal);
            //SwitchSensor sensor = new SwitchSensor(1, false);
            //_sensors.Add(sensor);
            //SimpleSignalControl sc = new SimpleSignalControl((SimpleSignal)signal);
            //sensor.OnChange += sc.ActOnSensor;
            //signal = new OneViewSignal(2, SimpleSignal.Go, start.LeftTrack.Next.Next.Next);
            //start.LeftTrack.Next.Next.Signals.Add(signal);

            //sensor = new SwitchSensor(2, false);
            //sc = new SimpleSignalControl((SimpleSignal)signal);
            //sensor.OnChange += sc.ActOnSensor;
            //start.LeftTrack.Next.Next.Next.Next.Sensors.Add(sensor);
            return start;
//            SwitchTrack start = new SwitchTrack(0, false, 50, 30, null, null, SwitchTrack.Left);
//
//            ITrack prev = start;
//            ITrack next = null;
//            for (int i = 1; i < 12; i++)
//            {
//                next = new SimpleTrack(i, false, 50, 30, null, prev);
//                prev.Next = next;
//                if (prev == start) start.LeftTrack = next;
//                prev = next;
//            }
//            
//            next.Next = start;
//            start.Previous = next;
//            start.TrunkTrack = next;
//
//            _startTrack = start;
//
//            ITrack right = new SimpleTrack(12, false, 50, null, start);
//            prev = right;
//            for (int i = 13; i < 20; i++)
//            {
//                next = new SimpleTrack(i, false, 50, -30, null, prev);
//                prev.Next = next;
//                prev = next;
//            }
//            start.RightTrack = right;
//            start.LeftTrack = start.Next;
//            ISignal signal = new SimpleSignal(1, SimpleSignal.Go);
//            start.RightTrack.Next.Signals.Add(signal);
//            SwitchSensor sensor = new SwitchSensor(1, false);
//            _sensors.Add(sensor);
//            SimpleSignalControl sc = new SimpleSignalControl((SimpleSignal)signal);
//            sensor.OnChange += sc.ActOnSensor;
//            signal = new OneViewSignal(2, SimpleSignal.Go, start.LeftTrack.Next.Next.Next);
//            start.LeftTrack.Next.Next.Signals.Add(signal);
//
//            sensor = new SwitchSensor(2, false);
//            sc = new SimpleSignalControl((SimpleSignal)signal);
//            sensor.OnChange += sc.ActOnSensor;
//            start.LeftTrack.Next.Next.Next.Next.Sensors.Add(sensor);
//            return start;
        }

        private void GenerateSignals(SwitchTrack switchTrack)
        {
            ;
        }



        private void GenerateSensors(SwitchTrack switchTrack)
        {
            ISensor sensor1 = new SwitchSensor(switchTrack.TrunkTrack.Id, false);
            ISensor sensor2 = new SwitchSensor(switchTrack.RightTrack.Id, false);
            ISensor sensor3 = new SwitchSensor(switchTrack.LeftTrack.Id, false);
            switchTrack.TrunkTrack.Sensors.Add(sensor1);
            switchTrack.TrunkTrack.Sensors.Add(sensor2);
            switchTrack.TrunkTrack.Sensors.Add(sensor3);

            if (!switchTrack.RightTrack.Next.Equals(null) && !switchTrack.RightTrack.Next.IsPartOfSwitchTrack && switchTrack.RightTrack.Next.Sensors.Count == 0)
                GenerateSimpleTrackSensor(switchTrack.RightTrack.Next);
            if (!switchTrack.LeftTrack.Next.Equals(null) && !switchTrack.LeftTrack.Next.IsPartOfSwitchTrack && switchTrack.LeftTrack.Next.Sensors.Count == 0)
                GenerateSimpleTrackSensor(switchTrack.LeftTrack.Next);
        }

        private void GenerateSimpleTrackSensor(ITrack simpleTrack)
        {
            ISensor sensor = new SwitchSensor(simpleTrack.Id, false);
            SimpleSignalControl sc = new SimpleSignalControl((SimpleSignal)Signal);
            simpleTrack.Sensors.Add(sensor);
            if (!simpleTrack.Next.Equals(null) && !simpleTrack.Next.IsPartOfSwitchTrack)
            {
                simpleTrack = simpleTrack.Next;
                GenerateSimpleTrackSensor(simpleTrack);
            }
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
