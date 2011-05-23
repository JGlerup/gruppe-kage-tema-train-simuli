using System;
namespace Noea.TogSim.Model
{
    interface ILocoDriverExtended
    {
        double Acceleration { get; set; }
        double Deacceleration { get; set; }
        void Go();
        long Interval { get; }
        double SlowSpeed { get; set; }
        string StateDescription { get; }
        void Stop();
        void TrackHandler(ITrainSet train, ITrainEventArgs args);
        ITrainSet Train { get; }
        double UpdateFrequency { get; set; }
        void UpdateState(object obj);
    }
}
