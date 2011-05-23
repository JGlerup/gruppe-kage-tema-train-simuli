using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Noea.TogSim.Model;

namespace Noea.TogSim.Gui.GDI
{
    public class TrainGraphics : Panel
    {
        private Hashtable _trackGraphicsTable;
        private Hashtable _unvisitedTracks;
        private ITrack _track;
        private TrainModel _trainModel;
        private Bitmap _trackTexture;
        private TrackImage _trackImage;
        private Bitmap _trackMap;
        private Bitmap _trainTexture;
        private Timer _updateTimer;
        private float _trackMinX, _trackMaxX, _trackMinY, _trackMaxY;
        private float _scale;
        private PointF _offset;

        public TrainGraphics()
        {
            _trainModel = new TrainModel();
            Init();
        }
        public TrainGraphics(TrainModel tm)
        {
            _trainModel = tm;
            Init();
        }

        private void Init()
        {
            DrawModel();

            this.Paint += this.PaintModel;
            this.Resize += this.ResizePanel;


            _updateTimer = new Timer();
            _updateTimer.Interval = 250; //Millisecs
            _updateTimer.Tick += new EventHandler(_updateTimer_Tick);

            _updateTimer.Start();
        }

        private void DrawModel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            int w = 10;
            int h = 5;
            _trainTexture = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(_trainTexture);
            g.FillRectangle(new SolidBrush(Color.Aqua), 0, 0, w, h);
            g.DrawRectangle(new Pen(Color.Green), 0, 0, w, h);

            w = 10;
            h = 10;
            _trackTexture = new Bitmap(w, h);
            g = Graphics.FromImage(_trackTexture);
            // g.FillRectangle(new SolidBrush(Color.IndianRed), 0, 0, w, h);
            g.DrawRectangle(new Pen(Color.Black), 0, 0, w - 2, h);
            //g.DrawLine(new Pen(Color.Black), 0, 0, w, 0);


            _trackMinX = 0;
            _trackMaxX = 10;
            _trackMinY = 0;
            _trackMaxY = 10;

            _trackGraphicsTable = new Hashtable();
            _unvisitedTracks = new Hashtable();

            ITrack track = _trainModel.StartTrack;
            foreach (ITrack t in Railroad.TrackList(track))
            {
                //Console.WriteLine("Track: {0}", t);
                _unvisitedTracks.Add(t.Id, t);
            }

            _trackImage = new TrackImage(_trackTexture);

            while (_unvisitedTracks.Count > 0)
            {
                TraverseTrack(track);
                if (_unvisitedTracks.Count > 0)
                {
                    IDictionaryEnumerator enumerator = (IDictionaryEnumerator)_unvisitedTracks.Keys.GetEnumerator();
                    if (enumerator.MoveNext() != null)
                    {
                        track = (ITrack)enumerator.Value;
                    }
                }
            }


            _scale = CalcScaleFactor(_trackMinX, _trackMaxX, _trackMinY, _trackMaxY, 25f);
            _offset = CalcOffsetPoint(_trackMinX, _trackMinY, _scale, 25f);

            GenerateTracks();
        }

        private void TraverseTrack(ITrack track)
        {
            bool isNew = HandleTrack(track);
            if (isNew)
            {
                foreach (ITrack t in track.NextList)
                {
                    if (t != null) TraverseTrack(t);
                }
            }
        }
        private bool HandleTrack(ITrack t)
        {
            TrackGeometry tg = null;

            if (_trackGraphicsTable.Count == 0)
            {
                tg = TrackGeometry.Create(t, new PointF(0f, 0f), 0, 0, _trackImage);
            }
            if (!_trackGraphicsTable.Contains(t))
            {
                if (t is SwitchTrack && false)
                {
                    tg = HandleSwitchTrack((SwitchTrack)t, tg);
                }
                else
                {
                    tg = HandleSimpleTrack(t, tg);
                }
            }
            else
            {
                return false;
            }
            if (tg != null)
            {
                AddTrackGraphicss(t, tg);
                return true;
            }
            else
            {

                throw new Exception("TrainGrapics: Track not resolved. Track id: " + t.Id);
            }
        }

        private void AddTrackGraphicss(ITrack t, TrackGeometry tg)
        {
            _trackGraphicsTable.Add(t, tg);
            _unvisitedTracks.Remove(t.Id);
            RegTrackBoundaries(tg);
        }

        private TrackGeometry HandleSimpleTrack(ITrack t, TrackGeometry tg)
        {
            TrackGeometry tp = (TrackGeometry)_trackGraphicsTable[t.Previous];
            if (tp != null)
            {
                tg = TrackGeometry.Create(t, tp.EndPoint, tp.EndAngle, 0, _trackImage);
            }
            else
            {
                tp = (TrackGeometry)_trackGraphicsTable[t.Next];
                if (tp != null) tg = TrackGeometry.Create(t, tp.StartPoint, tp.StartAngle, 0, new TrackImage(_trackTexture));
            }
            return tg;
        }

        private TrackGeometry HandleSwitchTrack(SwitchTrack t, TrackGeometry tg)
        {

            TrackGeometry tp = (TrackGeometry)_trackGraphicsTable[t.Previous.Previous];
            if (tp != null)
            {
                tg = TrackGeometry.Create(t, tp.EndPoint, tp.EndAngle, 0, _trackImage);
            }
            else
            {
                tp = (TrackGeometry)_trackGraphicsTable[t.Next.Next];
                if (tp != null) tg = TrackGeometry.Create(t, tp.StartPoint, tp.StartAngle, 0, new TrackImage(_trackTexture));
            }
            return tg;
        }


        void _updateTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void RegTrackBoundaries(TrackGeometry tg)
        {
            if (tg.EndPoint.X < _trackMinX) _trackMinX = tg.EndPoint.X;
            if (tg.EndPoint.X > _trackMaxX) _trackMaxX = tg.EndPoint.X;
            if (tg.EndPoint.Y < _trackMinY) _trackMinY = tg.EndPoint.Y;
            if (tg.EndPoint.Y > _trackMaxY) _trackMaxY = tg.EndPoint.Y;
        }
        private float CalcScaleFactor(float minX, float maxX, float minY, float maxY, float margin)
        {
            float scaleX = (this.Size.Width - 2 * margin) / Math.Abs(maxX - minX);
            float scaleY = (this.Size.Height - 2 * margin) / Math.Abs(maxY - minY);

            if (scaleX < scaleY)
            {
                return scaleX;
            }
            else
            {
                return scaleY;
            }
        }
        private PointF CalcOffsetPoint(float minX, float minY, float scale, float margin)
        {
            PointF p = new PointF();
            p.X = margin - minX * scale;
            p.Y = margin - minY * scale;
            return p;
        }
        private void GenerateTracks()
        {
            Bitmap trackMap = new Bitmap(this.Width, this.Height);
            Graphics dc = Graphics.FromImage(trackMap);

            ITrack track;
            IDictionaryEnumerator enumerator = _trackGraphicsTable.GetEnumerator();
            Pen trackEndPen = new Pen(Color.Red, 2);
            while (enumerator.MoveNext())
            {
                TrackGeometry tg = (TrackGeometry)enumerator.Value;
                track = tg.Track;
                Bitmap texture = tg.Image.Texture;
                int numTextures = (int)(track.Length / texture.Height) * 4;
                for (int i = 0; i < numTextures; i++)
                {
                    double pos = track.Length * i / (numTextures - 1);
                    // Console.WriteLine("Pos: " , pos);
                    PointF p = tg.GetPosition(pos);
                    //Console.WriteLine("p(x,y)=({0},{1})", p.X, p.Y);
                    float x = p.X * _scale + _offset.X;
                    float y = p.Y * _scale + _offset.Y;
                    //Console.WriteLine("X: {0}, Y:{1}", x, y);
                    Matrix transform = new Matrix();
                    transform.RotateAt((float)(tg.ToDeg(tg.GetAngle(pos)/*+Math.PI/2*/)), new PointF(x, y));

                    Matrix oldTransform = dc.Transform;
                    dc.Transform = transform;

                    float textureWidth = 20f * _scale;
                    float textureHeight = 10f * _scale;
                    // dc.FillEllipse(new SolidBrush(Color.Green), x, y, 10, 10);
                    dc.DrawImage(texture, (float)(x - textureWidth / 2f), y, textureWidth, textureHeight);
                    if (i == 0) dc.DrawLine(trackEndPen, x - textureWidth, y, x + textureWidth, y);

                    if (i == numTextures / 2) dc.DrawString("" + tg.Track.Id, new Font("Arial", 8), new SolidBrush(Color.Red), x + textureWidth * 0.5f, y);
                    dc.Transform = oldTransform;
                }
            }
            _trackMap = trackMap;
        }

        private void DrawTrain(Graphics dc)
        {
            foreach (ITrainSet t in _trainModel.TrainSets)
            {
                double pos = 0;
                try
                {
                    pos = t.Engine.TrackPos;
                    ITrack track = t.CurrentTrack;
                    ITrack preTrack = track.GetNext(t.PreviousTrack);

                    double rearPos;
                    TrackGeometry tg = (TrackGeometry)_trackGraphicsTable[track];

                    foreach (ICar car in t.Cars)
                    {
                        //float carWidth = 10f;
                        PointF frontPoint = tg.GetPosition(pos);
                        float x = frontPoint.X * _scale + _offset.X;
                        float y = frontPoint.Y * _scale + _offset.Y;

                        Matrix oldTransform = dc.Transform;
                        Matrix transform = dc.Transform.Clone(); //No changes should be made to original transform

                        transform.RotateAt((float)tg.ToDeg(tg.GetAngle(pos)), new PointF(x, y));
                        dc.Transform = transform;
                        PointF frontCorner = new PointF(x - _trainTexture.Width * _scale / 2, y - _trainTexture.Height * _scale / 2);
                        dc.DrawImage(_trainTexture, frontCorner.X, frontCorner.Y, _trainTexture.Width * _scale, _trainTexture.Height * _scale);
                        dc.Transform = oldTransform;

                        int direction;
                        direction = -TrackDirection(track, preTrack);
                        rearPos = pos - direction * car.Length;

                        bool found = true;
                        if (rearPos < 0) found = false;
                        if (rearPos > track.Length) found = false;

                        while (!found)//while aht. korte skinner
                        {
                            //trackIndex++;
                            //if (trackIndex < tracks.Length)
                            //{
                            ITrack temp = track;
                            track = track.GetNext(preTrack);
                            preTrack = temp;
                            if (track == null)
                            {
                                found = true;
                                track = preTrack;//UHA, UHA
                            }
                            if (rearPos < 0)
                            {
                                rearPos = track.Length + rearPos;
                                if (rearPos >= 0) found = true;
                            }
                            if (rearPos > preTrack.Length)
                            {
                                rearPos = rearPos - preTrack.Length;
                                if (rearPos <= track.Length) found = true;
                            }
                            if (found)
                            {
                                tg = (TrackGeometry)_trackGraphicsTable[track];
                            }
                        }
                        if (rearPos == 0) Console.WriteLine("RearPos er 0");
                        if (rearPos == track.Length) Console.WriteLine("RearPos er track.length");
                        if (!found)
                        {
                            Console.WriteLine("Track ikke fundet. Rearpos sat til 0. Rearpos: {0} direction: {1} car: {2}", rearPos, direction, car.Id);

                            rearPos = 0;
                        }
                        pos = rearPos;

                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine("DrawTrain: Exception catched {0}", e.Message);
                    Console.WriteLine("Train {0} - Sim time {1} secs: Frame not updated.\nPosition: {3}\nMessage: {2}\n Execution continues", t.Id, t.Engine.ElapsedTime, e.Message,pos);
                }

            }
        }
        private int TrackDirection(ITrack curTrack, ITrack prevTrack)
        {
            int result;
            if (curTrack.GetNext(prevTrack) == curTrack.Next)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }
            return result;
        }
        private void PaintModel(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            dc.DrawImage(_trackMap, 0, 0);
            DrawTrain(dc);
        }
        private void ResizePanel(object sender, EventArgs e)
        {
            DrawModel();
        }
    }
}
