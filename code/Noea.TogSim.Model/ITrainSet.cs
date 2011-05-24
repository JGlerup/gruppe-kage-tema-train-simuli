using System;
using System.Collections;
using System.Text;

namespace Noea.TogSim.Model
{
    public interface ITrainSet
    {
        //  Properties
        /// <summary>
        /// Identification of train set
        /// </summary>
        int Id { get;}
        /// <summary>
        /// Description of the train set
        /// </summary>
        string Description { get; set;}
        /// <summary>
        /// The number of cars
        /// </summary>
        int Count { get;}
        /// <summary>
        /// The total length of the train set in meters
        /// </summary>
        double Length { get;}
        /// <summary>
        /// The first car in the train set, relative to the driving direction 
        /// </summary>
        ICar FirstCar { get;}
        /// <summary>
        /// The last car in the train set, relative to the driving direction 
        /// </summary>
        ICar LastCar { get;}
        /// <summary>
        /// Collection of cars in the train set
        /// </summary>
        IList Cars { get;}
        /// <summary>
        /// Requested speed of the train set
        /// </summary>
        double RequestedSpeed { get; set;}
        /// <summary>
        /// Current speed of the train set
        /// </summary>
        double ActualSpeed { get; set;}
        /// <summary>
        /// Current acceleration of the train set
        /// </summary>
        double Acceleration { get; set;}
        /// <summary>
        /// Current deacceleration of the train set
        /// </summary>
        double Deacceleration { get; set;}
        /// <summary>
        /// Maximum speed that is allowed for the train set
        /// </summary>
        double MaxSpeed { get;}
        /// <summary>
        /// Driving direction, default=false;
        /// </summary>
        bool Reverse { get; set;}
        /// <summary>
        /// Location of the first car
        /// </summary>
        ITrack CurrentTrack { get;set;}
        /// <summary>
        /// Previous location of first car. Used when determing next track 
        /// </summary>
        ITrack PreviousTrack { get;set;}
        /// <summary>
        /// Controller for the train. Similar to the person controlling the locomotive.
        /// </summary>
        ILocoDriver LocoDriver { get; set;}
        /// <summary>
        /// The train engine. Responsible for moving the train on the _trackRows
        /// </summary>
        ITrainEngine Engine { get; set;}
        //	Methods
        /// <summary>
        /// Add a new car to the train set
        /// </summary>
        /// <param name="c">the new car</param>
        void Add(ICar c);
        /// <summary>
        /// Insert a new car at a given position
        /// </summary>
        /// <param name="index">posiion of the car</param>
        /// <param name="c">the new car</param>
        void Insert(int index, ICar c);
        /// <summary>
        /// Remove a given car from the train set
        /// </summary>
        /// <param name="c">the car to be removed</param>
        void Remove(ICar c);
        /// <summary>
        /// Remove the car at a given position
        /// </summary>
        /// <param name="index">the position of the car</param>
        void Remove(int index);
        /// <summary>
        /// Find the position of a given car
        /// </summary>
        /// <param name="c">the car to be found</param>
        /// <returns>the position</returns>
        int IndexOf(ICar c);
        /// <summary>
        /// Find the car at a given position
        /// </summary>
        /// <param name="index">the position</param>
        /// <returns>the car</returns>
        ICar Get(int index);

        event ReverseHandler OnReverse;
        event TrackHandler OnTrack;
        event ChangeHandler OnChange;
    }
    public delegate void ReverseHandler(ITrainSet train, ITrainEventArgs e);
    public delegate void TrackHandler(ITrainSet train, ITrainEventArgs e);
    public delegate void ChangeHandler(ITrainSet train, ITrainEventArgs e);
    public interface ITrainEventArgs
    {
        ITrainSet Train { get;}
        Object OldValue { get;}
        Object NewValue { get;}
    }

    public interface ICar
    {
        //	Properties
        /// <summary>
        /// Identification of the car
        /// </summary>
        int Id { get;}
        /// <summary>
        /// Description of the car
        /// </summary>
        string Description { get; set;}
        /// <summary>
        /// Max. speed, that is allowed for the car
        /// </summary>
        double MaxSpeed { get; set;}
        /// <summary>
        /// Total length of the car
        /// </summary>
        double Length { get; set;}
    }

    public interface ITrack
    {
        //	Properties
        /// <summary>
        /// Identification of the track
        /// </summary>
        int Id { get;}
        /// <summary>
        /// Indicates that a train set is on the track
        /// </summary>
        bool IsBlocked { get; set;}
        /// <summary>
        /// Length of the track. For curved track, it is the middle line. Det er det osse for lige spor ;o)
        /// </summary>
        double Length { get; set;}
        /// <summary>
        /// Angle of the arc. For straight _trackRows, it is 0.
        /// </summary>
        double Angle { get; set;}
        /// <summary>
        /// Detemine whether a track is a part of a switchtrack
        /// </summary>
        bool IsPartOfSwitchTrack { get; set; }
        /// <summary>
        ///  Next track. Undependent of the trains driving direction. For blind ends it is null. 
        /// </summary>
        ITrack Next { get; set;}
        /// <summary>
        ///  Previous track. Undependent of the trains driving direction. For blind ends it is null
        /// </summary>
        ITrack Previous { get; set;}
        /// <summary>
        /// List of next tracks
        /// </summary>
        ITrack[] NextList { get;}
        /// <summary>
        /// List of previous tracks
        /// </summary>
        /// 
        
        ITrack[] PreviousList { get;}
        /// <summary>
        /// Train, that is blocking the track
        /// </summary>
        ITrainSet Train { get; set;}
        /// <summary>
        /// Collection of signals, that is attached to the track
        /// </summary>
        IList Signals { get;}
        /// <summary>
        /// Collection of sensors, that is attached to the track
        /// </summary>
        IList Sensors { get;}

        //	Method
        /// <summary>
        /// Get next track, dependent of train driving direction. For blind ends it is null.
        /// </summary>
        /// <param name="previousTrack"></param>
        /// <returns>Next track in driving direction</returns>
        ITrack GetNext(ITrack previousTrack);
  
        /// <summary>
        /// Attach signal to the track. The signal is notified when IsBlocked changes
        /// </summary>
        /// <param name="s">The signal to be attached</param>
        void AddSignal(ISignal s);
        /// <summary>
        /// Remove signal from track
        /// </summary>
        /// <param name="s">Signal to be removed</param>
        void RemoveSignal(ISignal s);
        /// <summary>
        /// Remove signal from track
        /// </summary>
        /// <param name="index">Position of signal to be removed</param>
        void RemoveSignal(int index);
        /// <summary>
        /// Get signal at position
        /// </summary>
        /// <param name="index">Position</param>
        /// <returns>Signal at position</returns>
        ISignal GetSignal(int index);
        event BlockHandler OnBlock;
        event TrackChangeHandler OnChange;
    }


    public delegate void BlockHandler(ITrack track, ITrackEventArgs e);
    public delegate void TrackChangeHandler(ITrack track, ITrackEventArgs e);
    public interface ITrackEventArgs
    {
        ITrack Track { get;}
        Object OldValue { get;}
        Object NewValue { get;}
    }
    public delegate void SignalHandler(ISignal signal, ISignalEventArgs e);
    public interface ISignal
    {
        // Properties
        /// <summary>
        /// Identification of the signal
        /// </summary>
        int Id { get;}
        /// <summary>
        /// State of the signal
        /// </summary>
        ISignalState State { get; set;}
        /// <summary>
        /// Associated signal. Used for pre-signaling
        /// </summary>
        ISignal AssociatedSignal { get; set;}
        event SignalHandler OnChange;
    }
    /// <summary>
    /// Valid states for signals
    /// </summary>
    public interface ISignalEventArgs
    {
        ISignal Signal { get;}
        Object OldValue { get;}
        Object NewValue { get;}
    }
    public interface ISignalState
    {
        /// <summary>
        /// Numeric value of the state
        /// </summary>
        int Value { get;}
        /// <summary>
        /// Textual description of the state
        /// </summary>
        string Description { get; set;}

    }

    /// <summary>
    /// Eventhandler for ISensor
    /// </summary>
    /// <param name="sensor">Handled sensor</param>
    /// <param name="e">Event</param>
    public delegate void SensorHandler(ISensor sensor, ISensorEventArgs e);
    /// <summary>
    ///	To be implemented by sensors. Sensors registrates events on the _trackRows, ie. that it is blocked 
    /// </summary>
    public interface ISensor
    {
        /// <summary>
        /// Identification of the sensor
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Track where the sensor is mounted. 
        /// </summary>
        Object Value { get; set;}
        /// <summary>
        /// OnChange event, to fired when the state changes
        /// </summary>
        event SensorHandler OnChange;
    }

    public interface ISensorEventArgs
    {
        ISensor Sensor { get;}
        Object OldValue { get;}
        Object NewValue { get;}
    }

    /// <summary>
    /// To be implemented by LocoDrivers. 
    /// An AI locodriver should react on events in the model, i.e a signal
    /// A human locodriver implementation should react on event in an user interface, such as breaking.
    /// </summary>
    public interface ILocoDriver
    {
        /// <summary>
        /// Set, gets current acceleration. May be used by a human locodriver, or AI state machine 
        /// </summary>
        double Acceleration { get; set; }
        /// <summary>
        /// Set, gets current deacceleration. May be used by a human locodriver, or AI state machine 
        /// </summary>
        double Deacceleration { get; set; }
        /// <summary>
        /// Gets the train set associated with the LocoDriver
        /// </summary>
        ITrainSet Train { get; }
        /// <summary>
        /// Gets a description string for the current state of the train set, i.e 'Running' og 'Wait for signal'
        /// </summary>
        string StateDescription { get; }
        /// <summary>
        /// Request a state update. This method should be called by a scheduling mechanism.
        /// </summary>
        /// <param name="obj">Object that can be used to pass stateinformation. I.e to used by a timer delegate</param>
        void UpdateState(Object obj);
        /// <summary>
        /// Returns suggested update interval for the state machine
        /// </summary>
        long Interval {get;}
        /// <summary>
        /// Event handler for change of current track. Used by AI locodriver to react on signals
        /// </summary>
        /// <param name="train">Actual train set</param>
        /// <param name="args">EventArg with further information</param>
        void Go();
        /// <summary>
        /// Set the locodriver into stopped state.
        /// </summary>
        void Stop();
    }
    /// <summary>
    /// Implements an engine for primary for updating position and speed of a trainset
    /// A external schedule/dispathing mechanism should call one of the class' UpdatePosition methods
    /// </summary>
    public interface ITrainEngine
    {
        /// <summary>
        /// Interval between updating the position
        /// </summary>
        int Interval { get; }
        /// <summary>
        /// Time stamp for last update of the position
        /// </summary>
        long PrevTime { get; set; }
        /// <summary>
        /// Simulation time elapsed for this Engine.
        /// </summary>
        double ElapsedTime { get;}
        /// <summary>
        /// get, set acceleration. When updating position, the speed should be updated according to acceleration
        /// </summary>
        double Acceleration { get;set;}
        /// <summary>
        /// UpdatePosition updates position on the track and the speed of the trainset.
        /// If the train set has moved to the next track, the TrainSet's CurrentTrack property must be updated.
        /// </summary>
        /// <param name="obj">Object with new current time</param>
        void UpdatePosition(Object obj);
        /// <summary>
        /// UpdatePosition updates position on the track and the speed of the trainset.
        /// If the train set has moved to the next track, the TrainSet's CurrentTrack property must be updated.
        /// </summary>
        /// <param name="secs">Elapsed time since last update</param>
        void UpdatePosition(double secs);
        /// <summary>
        /// Returns a Queue Object with the tracks, that train set blocks. 
        /// The front element in the queue is the first to be unblocked, which also is the last track in the driving direction
        /// </summary>
        System.Collections.Queue Tracks { get; }
        /// <summary>
        /// Gets the train set associated with the TrainEngine
        /// </summary>
        ITrainSet Train { get; }
        /// <summary>
        /// Gets front position of the trainset on current track
        /// </summary>
        double TrackPos { get;}
        /// <summary>
        /// Event handler for handling a change in the driving direction. 
        /// The queue has to be inverted.
        /// </summary>
        /// <param name="ts">Actual train set</param>
        /// <param name="args">Further information</param>
        void InvertDirection(ITrainSet ts, ITrainEventArgs args);

        /// <summary>
        /// Start the engine
        /// </summary>
        void Start();
        /// <summary>
        /// Start the engine
        /// </summary>
        /// <param name="updateFrequency">Requested update frequency = calls to UpdatePosition pr. sec. </param>
        void Start(double updateFrequency);
        /// <summary>
        /// Stops the engine
        /// </summary>
        void Stop();
        /// <summary>
        /// Get, sets requested update frequency = calls to UpdatePosition pr. sec.
        /// </summary>
        double UpdateFrequency { get; set; }
        /// <summary>
        /// Gets an array with blocked tracks by the train set.
        /// Index =0 references current track.
        /// </summary>
        /// <returns>a array with tracks</returns>
        ITrack[] TracksList();
    }
    public interface ITrainEngineDispatch
    {
        /// <summary>
        /// Create and starts the dispather
        /// </summary>
        void Create();
        /// <summary>
        /// Create and starts the dispather for a list of train sets
        /// </summary>
        void Create(ArrayList trains);
        /// <summary>
        /// Close and dispose the dispather
        /// </summary>
        void Dispose();
    }
}