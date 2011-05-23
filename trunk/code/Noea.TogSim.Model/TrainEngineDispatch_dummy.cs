using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Noea.TogSim.Model
{
    /// <summary>
    /// Here is 3 kinds of di
    /// </summary>
    public abstract class TrainEngineDispatch : ITrainEngineDispatch
    {
        #region ITrainEngineDispatch Members

        ArrayList _trains;

        public ArrayList Trains
        {
            get { return _trains; }
            set { _trains = value; }
        }

        public TrainEngineDispatch()
        {
        }
        public TrainEngineDispatch(ArrayList trains)
        {
            _trains = trains;
        }

        public abstract void Create();

        public void Create(System.Collections.ArrayList trains)
        {
            Trains = trains;
            Create();
        }
        public abstract void Dispose();

        #endregion
    }
    /// <summary>
    /// Bemærk denne implementation vil blokere gui opdatering, og kan ikke anbefales.
    /// </summary>
    public class DummyDispatch : TrainEngineDispatch
    {
        bool _doRun = true;

        public DummyDispatch() : base() { }
        public DummyDispatch(ArrayList trains) : base(trains) { }

        public override void Create()
        {
            while (_doRun)
            {
                foreach (ITrainSet t in this.Trains)
                {
                    t.Engine.UpdatePosition(null); //Brug ved real tid
                    //t.Engine.UpdatePosition(2); //Flyt tid 2 sekunder. Brug ved simulering.
                    t.LocoDriver.UpdateState(null);
                }
            }
        }
        public override void Dispose()
        {
            _doRun = false;
        }
    }
}
