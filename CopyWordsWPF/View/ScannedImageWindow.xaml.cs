using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CopyWordsWPF.View
{
    // see example here: http://www.codeproject.com/Articles/97871/WPF-simple-zoom-and-drag-support-in-a-ScrollViewer
    public partial class ScannedImageWindow : Window
    {
        private Point? _lastCenterPositionOnTarget;
        private Point? _lastMousePositionOnTarget;
        private Point? _lastDragPoint;

        private DispatcherTimer _clickTimer;
        private int _clickCounter;

        public ScannedImageWindow()
        {
            InitializeComponent();

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseMove += OnMouseMove;
            scrollViewer.PreviewMouseWheel += scrollViewer_PreviewMouseWheel;

            _clickTimer = new DispatcherTimer();
            _clickTimer.Interval = TimeSpan.FromMilliseconds(300);
            _clickTimer.Tick += EvaluateClicks;

            SetZoom(1.6, 1.6);
        }

        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta < 0 && (scaleTransform.ScaleX < .4 || scaleTransform.ScaleY < .4))
                {
                    return;
                }

                if (e.Delta > 0)
                {
                    ChangeZoom(true);
                }
                else
                {
                    ChangeZoom(false);
                }
            }
        }

        #region Event handlers

        private void btnZooomOut_Click(object sender, RoutedEventArgs e)
        {
            ChangeZoom(false);
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ChangeZoom(true);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - _lastDragPoint.Value.X;
                double dY = posNow.Y - _lastDragPoint.Value.Y;

                _lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);

            //make sure we still can use the scrollbars
            if (mousePos.X <= scrollViewer.ViewportWidth &&
                mousePos.Y < scrollViewer.ViewportHeight)
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                _lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }

            _clickCounter++;
            _clickTimer.Start();
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            _lastDragPoint = null;
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!_lastMousePositionOnTarget.HasValue)
                {
                    if (_lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(
                            scrollViewer.ViewportWidth / 2,
                            scrollViewer.ViewportHeight / 2);

                        Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, grid);

                        targetBefore = _lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = _lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(grid);

                    _lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double inTargetPixelsX = targetNow.Value.X - targetBefore.Value.X;
                    double inTargetPixelsY = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / grid.Width;
                    double multiplicatorY = e.ExtentHeight / grid.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset - (inTargetPixelsX * multiplicatorX);
                    double newOffsetY = scrollViewer.VerticalOffset - (inTargetPixelsY * multiplicatorY);

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }

        #endregion

        #region Private Methods

        private void ChangeZoom(bool zoomin)
        {
            double zoom = zoomin ? .2 : -.2;

            scaleTransform.ScaleX += zoom;
            scaleTransform.ScaleY += zoom;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            _lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);
        }

        private void SetZoom(double scaleX, double scaleY)
        {
            scaleTransform.ScaleX = scaleX;
            scaleTransform.ScaleY = scaleY;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            _lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);
        }

        private void EvaluateClicks(object sender, EventArgs e)
        {
            _clickTimer.Stop();

            // if it was double click
            if (_clickCounter == 2)
            {
                SetZoom(1.0, 1.0);
            }

            _clickCounter = 0;
        }

        #endregion
    }
}
