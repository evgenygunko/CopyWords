using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace CopyWordsWPF.View
{
    // see http://stackoverflow.com/questions/741956/pan-zoom-image
    public class ZoomBorder : Border
    {
        #region Dependency properties

        public static readonly DependencyProperty XPosProperty =
            DependencyProperty.Register("XPos", typeof(double), typeof(ZoomBorder), new UIPropertyMetadata(0.0));

        public double XPos
        {
            get { return (double)GetValue(XPosProperty); }
            set { SetValue(XPosProperty, value); }
        }

        public static readonly DependencyProperty YPosProperty =
            DependencyProperty.Register("YPos", typeof(double), typeof(ZoomBorder), new UIPropertyMetadata(0.0));

        public double YPos
        {
            get { return (double)GetValue(YPosProperty); }
            set { SetValue(YPosProperty, value); }
        }

        public static readonly DependencyProperty XScaleProperty =
            DependencyProperty.Register("XScale", typeof(double), typeof(ZoomBorder), new UIPropertyMetadata(1.0));

        public double XScale
        {
            get { return (double)GetValue(XScaleProperty); }
            set { SetValue(XScaleProperty, value); }
        }

        public static readonly DependencyProperty YScaleProperty =
            DependencyProperty.Register("YScale", typeof(double), typeof(ZoomBorder), new UIPropertyMetadata(1.0));

        public double YScale
        {
            get { return (double)GetValue(YScaleProperty); }
            set { SetValue(YScaleProperty, value); }
        }

        #endregion  
        
        private UIElement _child = null;
        private Point _origin;
        private Point _start;

        private DispatcherTimer _clickTimer;
        private int _clickCounter;

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (value != null && value != this.Child)
                {
                    this.Initialize(value);
                }

                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this._child = element;
            if (_child != null)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);
                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);
                _child.RenderTransform = group;
                _child.RenderTransformOrigin = new Point(0.0, 0.0);

                this.MouseWheel += child_MouseWheel;
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseMove += child_MouseMove;
                this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(
                  child_PreviewMouseRightButtonDown);
            }

            SetInitialZoom();

            _clickTimer = new DispatcherTimer();
            _clickTimer.Interval = TimeSpan.FromMilliseconds(300);
            _clickTimer.Tick += EvaluateClicks;        
        }

        private void EvaluateClicks(object sender, EventArgs e)
        {
            _clickTimer.Stop();
            
            if (_child != null)
            {
                _child.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
            }

            // if it was double click
            if (_clickCounter == 2)
            {
                _child.ReleaseMouseCapture();
                Reset();
            }

            _clickCounter = 0;
        }
        
        #region Public methods

        public void Reset()
        {
            if (_child != null)
            {
                // reset zoom
                var st = GetScaleTransform(_child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = GetTranslateTransform(_child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        public void ZoomIn()
        {
            ChangeZoom(true);
        }

        public void ZoomOut()
        {
            ChangeZoom(false);
        }

        #endregion

        private void ChangeZoom(bool zoomin)
        {
            if (_child != null)
            {
                var st = GetScaleTransform(_child);

                double zoom = zoomin ? .2 : -.2;
                if (!zoomin && (st.ScaleX < .4 || st.ScaleY < .4))
                {
                    return;
                }

                st.ScaleX += zoom;
                st.ScaleY += zoom;

                XScale = st.ScaleX;
                YScale = st.ScaleY;
            }
        }

        private void SetInitialZoom()
        {
            if (_child != null)
            {
                // set zoom
                var st = GetScaleTransform(_child);
                st.ScaleX = XScale;
                st.ScaleY = YScale;

                // set pan
                var tt = GetTranslateTransform(_child);
                tt.X = XPos;
                tt.Y = YPos;
            }
        }

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
              .Children.First(tr => tr is ScaleTransform);
        }
        
        #region Child Events

        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IgnoreEvent())
            {
                return;
            }

            if (_child != null)
            {
                var st = GetScaleTransform(_child);
                var tt = GetTranslateTransform(_child);

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                {
                    return;
                }

                Point relative = e.GetPosition(_child);
                double abosuluteX;
                double abosuluteY;

                abosuluteX = (relative.X * st.ScaleX) + tt.X;
                abosuluteY = (relative.Y * st.ScaleY) + tt.Y;

                st.ScaleX += zoom;
                st.ScaleY += zoom;

                XScale = st.ScaleX;
                YScale = st.ScaleY;

                tt.X = abosuluteX - (relative.X * st.ScaleX);
                tt.Y = abosuluteY - (relative.Y * st.ScaleY);
            }
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_child != null)
            {
                var tt = GetTranslateTransform(_child);
                _start = e.GetPosition(this);
                _origin = new Point(tt.X, tt.Y);
                this.Cursor = Cursors.Hand;
                _child.CaptureMouse();

                _clickCounter++;
                _clickTimer.Start();
            }
        }

        private void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_child != null)
            {
                if (_child.IsMouseCaptured)
                {
                    var tt = GetTranslateTransform(_child);
                    Vector v = _start - e.GetPosition(this);
                    tt.X = _origin.X - v.X;
                    tt.Y = _origin.Y - v.Y;
                }
            }
        }

        #endregion

        private bool IgnoreEvent()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                return false;
            }

            return true;
        }
    }
}
