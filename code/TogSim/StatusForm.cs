using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using Noea.TogSim.Model;
namespace Noea.TogSim.Gui.GDI
{
    /// <summary>
    /// Formen
    /// </summary>
    public class StatusForm : Form
    {

        ArrayList _trackRows;
        ArrayList _trainRows;
        ArrayList _signalRows;
        ArrayList _sensorRows;
        ArrayList _tracks;
        ArrayList _signals;
    
        private Button button1;
        private Button button2;
        private Button button3;
        private TrainGraphics DrawPanel;

        TrainModel _trainModel;
        /// <summary>
        /// konstruktøren
        /// </summary>
        public StatusForm()
        {
            _trainModel = new TrainModel();

            InitializeComponent();

            _signals = new ArrayList();
            InitTracks();
            InitTrains();
            InitSignals();
            InitSensors();

            //_trainModel.Start();
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tracksPanel = new System.Windows.Forms.Panel();
            this.trainsPanel = new System.Windows.Forms.Panel();
            this.signalsPanel = new System.Windows.Forms.Panel();
            this.sensorsPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.DrawPanel = new Noea.TogSim.Gui.GDI.TrainGraphics(_trainModel);
            this.SuspendLayout();
            // 
            // tracksPanel
            // 
            this.tracksPanel.AutoScroll = true;
            this.tracksPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tracksPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tracksPanel.Location = new System.Drawing.Point(383, 15);
            this.tracksPanel.Name = "tracksPanel";
            this.tracksPanel.Size = new System.Drawing.Size(308, 348);
            this.tracksPanel.TabIndex = 0;
            // 
            // trainsPanel
            // 
            this.trainsPanel.AutoScroll = true;
            this.trainsPanel.BackColor = System.Drawing.Color.Lavender;
            this.trainsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trainsPanel.Location = new System.Drawing.Point(27, 15);
            this.trainsPanel.Name = "trainsPanel";
            this.trainsPanel.Size = new System.Drawing.Size(350, 71);
            this.trainsPanel.TabIndex = 1;
            // 
            // signalsPanel
            // 
            this.signalsPanel.AutoScroll = true;
            this.signalsPanel.BackColor = System.Drawing.Color.PapayaWhip;
            this.signalsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.signalsPanel.Location = new System.Drawing.Point(27, 92);
            this.signalsPanel.Name = "signalsPanel";
            this.signalsPanel.Size = new System.Drawing.Size(350, 95);
            this.signalsPanel.TabIndex = 2;
            // 
            // sensorsPanel
            // 
            this.sensorsPanel.AutoScroll = true;
            this.sensorsPanel.BackColor = System.Drawing.Color.Snow;
            this.sensorsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sensorsPanel.Location = new System.Drawing.Point(27, 193);
            this.sensorsPanel.Name = "sensorsPanel";
            this.sensorsPanel.Size = new System.Drawing.Size(350, 85);
            this.sensorsPanel.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(27, 313);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Start";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(189, 313);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Change signal";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(108, 313);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Change track";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DrawPanel
            // 
            this.DrawPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DrawPanel.Location = new System.Drawing.Point(698, 15);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(359, 348);
            this.DrawPanel.TabIndex = 7;
            this.DrawPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseClick);
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 387);
            this.Controls.Add(this.DrawPanel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.sensorsPanel);
            this.Controls.Add(this.signalsPanel);
            this.Controls.Add(this.trainsPanel);
            this.Controls.Add(this.tracksPanel);
            this.Name = "StatusForm";
            this.Text = "Train";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatusForm_FormClosing);
            this.ResumeLayout(false);

        }

        private void InitTracks()
        {
            _trackRows = new ArrayList();
            _tracks = Railroad.TrackList(_trainModel.StartTrack);
            int y = 10;
            //Console.WriteLine("Default font: {0} Højde: {1}", Label.DefaultFont.Name, Label.DefaultFont.Height);
            foreach (ITrack t in _tracks)
            {
                TrackRow tr = new TrackRow(tracksPanel, t, y);
                t.OnChange += tr.ChangeHandler;
                _trackRows.Add(tr);
                tr.UpdateLabels();
                y += Label.DefaultFont.Height + 2;

                if (t.Signals.Count > 0)
                {
                    _signals.AddRange(t.Signals);
                }
            }
            this.tracksPanel.Controls.Add(this.track22);
            this.tracksPanel.Controls.Add(this.track21);
            this.tracksPanel.Controls.Add(this.track12);
            this.tracksPanel.Controls.Add(this.track11);
        }
        private void InitTrains()
        {
            _trainRows = new ArrayList();
            int y = 10;
            foreach (ITrainSet t in _trainModel.TrainSets)
            {
                TrainRow tr = new TrainRow(trainsPanel, t, y);
                t.OnChange += tr.ChangeHandler;
                _trainRows.Add(tr);
                tr.UpdateLabels();
                y += Label.DefaultFont.Height + 2;
            }
        }
        private void InitSignals()
        {
            _signalRows = new ArrayList();
            int y = 10;
            foreach (ITrack t in _tracks)
            {
                foreach (ISignal s in t.Signals)
                {
                    SignalRow sr = new SignalRow(signalsPanel, s, t, y);
                    s.OnChange += sr.ChangeHandler;
                    //Console.WriteLine(s);
                    _signalRows.Add(sr);
                    sr.UpdateLabels();
                    y += Label.DefaultFont.Height + 2;
                }
            }
        }
        private void InitSensors()
        {
            _sensorRows = new ArrayList();
            int y = 10;
            foreach (ISensor s in _trainModel.Sensors)
            {
                SensorRow sr = new SensorRow(sensorsPanel, s, null, y);
                s.OnChange += sr.ChangeHandler;
                _signalRows.Add(sr);
                sr.UpdateLabels();
                y += Label.DefaultFont.Height + 2;
            }
            foreach (ITrack t in _tracks)
            {
                foreach (ISensor s in t.Sensors)
                {
                    SensorRow sr = new SensorRow(sensorsPanel, s,t, y);
                    s.OnChange += sr.ChangeHandler;
                    _signalRows.Add(sr);
                    sr.UpdateLabels();
                    y += Label.DefaultFont.Height + 2;
                }
            }

        }
        #endregion

        private System.Windows.Forms.Panel tracksPanel;
        private System.Windows.Forms.Label track11;
        private System.Windows.Forms.Label track12;
        private System.Windows.Forms.Panel trainsPanel;
        private System.Windows.Forms.Label train14;
        private System.Windows.Forms.Label train13;
        private System.Windows.Forms.Label train12;
        private System.Windows.Forms.Label train11;
        private System.Windows.Forms.Panel signalsPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel sensorsPanel;
        private System.Windows.Forms.Label track22;
        private System.Windows.Forms.Label track21;
        private System.Windows.Forms.Label train24;
        private System.Windows.Forms.Label train23;
        private System.Windows.Forms.Label train22;
        private System.Windows.Forms.Label train21;

        private void button1_Click(object sender, EventArgs e)
        {
            _trainModel.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ((SwitchTrack)_trainModel.StartTrack).Direction = SwitchTrack.Right;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((SwitchSensor)_trainModel.Sensors[0]).Value = true;
        }



        private void StatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trainModel.Dispose();
            StoppingBox stopBox = new StoppingBox();
            stopBox.Show();
            int waitTime = 3000;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(waitTime / 10);
                stopBox.Progress.PerformStep();
            }
            stopBox.Close();
        }

        private void DrawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (ITrainSet train in _trainModel.TrainSets)
            {
                Console.WriteLine("Train: {0}", train.Id);
                foreach (ICar car in train.Cars)
                {
                    Console.WriteLine(" car: {0} ", car.Id);
                }
                foreach (ITrack track in train.Engine.TracksList())
                {
                    Console.WriteLine("Track: {0}", track.Id);
                }
            }
        }
    }

    class TrackRow
    {
        ITrack _track;
        Label _id;
        Label _blocked;
        Label _direction;
        Label _next;

        public TrackRow(System.Windows.Forms.Control cont, ITrack trk, int y)
        {
            _track = trk;
            _id = ControlHelper.LabelSetup(cont, 10, y);
            _blocked = ControlHelper.LabelSetup(cont, 85, y);
            _direction = ControlHelper.LabelSetup(cont, 200, y);
            _direction.Font = new Font(_direction.Font, FontStyle.Underline);
            _direction.Click += new EventHandler(this.DirectionClickHandler);
            _next = ControlHelper.LabelSetup(cont, 230, y);
            
        }
        public Label Id
        {
            get { return _id; }
        }
        public Label Blocked
        {
            get { return _blocked; }
        }
        public Label Direction
        {
            get { return _direction; }
        }
        public Label Next
        {
            get { return _next; }
        }
        public void UpdateLabels()
        {
            Id.Text = String.Format("Track #{0:d3}", _track.Id);

            if (_track.IsBlocked)
            {

                String text = "Blocked";
                if (_track.Train != null)
                {
                    text += String.Format(" by train#{0:d3}", _track.Train.Id);
                }
                else text += " by unknown";
                Blocked.Text = text;
            }
            else
            {

                Blocked.Text = String.Format("Free");
            }

            Direction.Text = "";
            ITrack nextTrack = _track.Next;
            if (_track is SwitchTrack)
            {
                SwitchTrack t=(SwitchTrack) _track;
                String dir;
                if (t.Direction == SwitchTrack.Right)
                {
                    dir = "Right";
                    nextTrack = t.RightTrack;
                }
                else
                {
                    dir = "Left";
                    nextTrack = t.LeftTrack;
                }
                Direction.Text += String.Format("{0}", dir);
                
            }
            if (nextTrack != null)
            {
                Next.Text = String.Format("-->{0:d3}", nextTrack.Id);
            }
            else
            {
                Next.Text = "-->(Dead End)";
            }
        }
        public void ChangeHandler(ITrack t, ITrackEventArgs args)
        {
            if (Blocked.InvokeRequired)
            {
                Blocked.Invoke(new MethodInvoker(this.UpdateLabels));
            }
            else
            {
                UpdateLabels();
            }
        }
        public void DirectionClickHandler(Object obj, EventArgs e)
        {
            ((SwitchTrack)_track).Toggle();
        }
    }
    class TrainRow
    {
        ITrainSet _train;
        Label _id;
        Label _track;
        Label _state;
        Label _reverse;
        Label _speed;
        Label _elapTime;

        public TrainRow(System.Windows.Forms.Control parent, ITrainSet train, int y)
        {
            _train = train;
            _id = ControlHelper.LabelSetup(parent, 12, y);
            _track = ControlHelper.LabelSetup(parent, 72, y);
            _state = ControlHelper.LabelSetup(parent, 146, y);
            _state.Font = new Font(_state.Font, FontStyle.Underline);
            _state.Click += new EventHandler(this.StateClickHandler);
            _reverse = ControlHelper.LabelSetup(parent, 197, y);
            _reverse.Font = new Font(_reverse.Font, FontStyle.Underline);
            _reverse.Click += new EventHandler(this.ReverseClickHandler);
            _speed = ControlHelper.LabelSetup(parent, 250, y);
            _speed.Font = new Font(_speed.Font, FontStyle.Underline);
            _speed.Click += new EventHandler(this.SpeedClickHandler);
            _elapTime = ControlHelper.LabelSetup(parent, 320, y);
            UpdateLabels();
        }

        public Label Id
        {
            get { return _id; }
        }
        public Label Track
        {
            get { return _track; }
        }
        public Label State
        {
            get { return _state; }
        }
        public Label Reverse
        {
            get { return _reverse; }
        }
        public Label Speed
        {
            get { return _speed; }
        }
        public Label ElapsedTime
        {
            get { return _elapTime; }
        }
        public void UpdateLabels()
        {
            Id.Text = String.Format("Train #{0:d3}", _train.Id);
            Track.Text = String.Format("at track #{0:d3}", _train.CurrentTrack.Id);
            State.Text = _train.LocoDriver.StateDescription;
            Speed.Text = String.Format("{0:000.0} / {1:000.0}", _train.RequestedSpeed, _train.ActualSpeed);
            ElapsedTime.Text = String.Format("{0:000.0}", _train.Engine.ElapsedTime);
            if (_train.Reverse)
            {
                Reverse.Text = String.Format("Backward");
            }
            else
            {

                Reverse.Text = String.Format("Forward");
            }
        }
        public void ReverseClickHandler(Object obj, EventArgs e)
        {
            _train.Reverse = !_train.Reverse;
        }
        public void StateClickHandler(Object obj, EventArgs e)
        {
           // _train.
        }
        public void SpeedClickHandler(Object obj, EventArgs e)
        {
            TrainSpeedForm ts = new TrainSpeedForm(_train);
            ts.Show();
        }

        public void ChangeHandler(ITrainSet t, ITrainEventArgs args)
        {
            //Console.WriteLine("ChangeHandler "+DateTime.Now+ " train(" + t.Id + ") er på spor: " + t.CurrentTrack.Id);
            if (Track.InvokeRequired)
            {
                try
                {
                    Track.Invoke(new MethodInvoker(this.UpdateLabels));
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            else
            {
                UpdateLabels();
            }
        }
    }

    class SignalRow
    {
        ISignal _signal;
        ITrack _track;
        Label _id;
        Label _trackId;
        Label _state;

        public SignalRow(System.Windows.Forms.Control parent, ISignal signal, ITrack track, int y)
        {
            _signal = signal;
            _track = track;
            _id = ControlHelper.LabelSetup(parent, 12, y);
            _trackId = ControlHelper.LabelSetup(parent, 75, y);
            _state = ControlHelper.LabelSetup(parent, 146, y);
            //	_signal.OnChange += this.ChangeHandler;
            UpdateLabels();
        }

        public Label Id
        {
            get { return _id; }
        }

        public Label Track
        {
            get { return _trackId; }
        }
        public Label State
        {
            get { return _state; }
        }

        public void UpdateLabels()
        {
            Id.Text = String.Format("Signal #{0:d3}", _signal.Id);
            Track.Text = String.Format("at track #{0:d3}", _track.Id);
            State.Text = _signal.State.Description;
            if (_signal.State.Value == 0)
            {
                State.ForeColor = Color.Green;
            }
            if (_signal.State.Value == 1)
            {
                State.ForeColor = Color.Red;
            }
        }



        public void ChangeHandler(ISignal s, ISignalEventArgs args)
        {
            //Console.WriteLine("ChangeHandler " + DateTime.Now + " siganl(" + _signal.Id + ") er sat til " + _signal.State.Description);
            if (State.InvokeRequired)
            {
                State.Invoke(new MethodInvoker(this.UpdateLabels));
            }
            else
            {

                UpdateLabels();
            }
        }
    }
    class SensorRow
    {
        ISensor _sensor;
        ITrack _track;
        Label _id;
        Label _state;

        public SensorRow(System.Windows.Forms.Control parent, ISensor sensor, ITrack track, int y)
        {
            _sensor = sensor;
            _track = track;
            _id = ControlHelper.LabelSetup(parent, 12, y);
            _state = ControlHelper.LabelSetup(parent, 146, y);
            _state.Font = new Font(_state.Font, FontStyle.Underline);
            _state.Click += new EventHandler(this.StateClickHandler);
            UpdateLabels();
        }

        public Label Id
        {
            get { return _id; }
        }

        public Label State
        {
            get { return _state; }
        }

        public void UpdateLabels()
        {
            Id.Text = String.Format("Sensor #{0:d3}", _sensor.Id);
            if (_track != null)
            {
                Id.Text+= String.Format(" at track #{0:d3}", _track.Id);
            }
            else
            {
                Id.Text += " global";
            }

            if (_sensor.Value is Boolean)
            {
                if ((Boolean)_sensor.Value)
                {
                    State.Text = "Set";
                }
                else
                {
                    State.Text = "Not set";
                }
            }
        }
        public void ChangeHandler(ISensor s, ISensorEventArgs args)
        {
            if (State.InvokeRequired)
            {
                State.Invoke(new MethodInvoker(this.UpdateLabels));
            }
            else
            {

                UpdateLabels();
            }
        }
        public void StateClickHandler(Object obj, EventArgs e)
        {
            if (_sensor is SwitchSensor) ((SwitchSensor)_sensor).Value = !(Boolean)_sensor.Value;
        }
    }
    class ControlHelper
    {
        public static Label LabelSetup(Control parent, int x, int y)
        {
            Label l = new Label();
            l.AutoSize = true;
            l.Location = new Point(x, y);
            parent.Controls.Add(l);
            return l;
        }
        public static Label LabelSetup(Control parent, string text, int x, int y)
        {
            Label l = LabelSetup(parent, x, y);
            l.Text = text;

            return l;
        }
    }
}
