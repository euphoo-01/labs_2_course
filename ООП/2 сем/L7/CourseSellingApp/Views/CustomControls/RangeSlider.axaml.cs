using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace CourseSellingApp.Views.CustomControls
{
    public partial class RangeSlider : UserControl
    {
        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(Minimum), 0.0);

        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<RangeSlider, double>(nameof(Maximum), 3000.0);

        public static readonly StyledProperty<double> LowerValueProperty =
            AvaloniaProperty.Register<RangeSlider, double>(
                nameof(LowerValue),
                0.0,
                coerce: CoerceLowerValue);

        public static readonly StyledProperty<double> UpperValueProperty =
            AvaloniaProperty.Register<RangeSlider, double>(
                nameof(UpperValue),
                3000.0,
                coerce: CoerceUpperValue);

        public double Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double LowerValue
        {
            get => GetValue(LowerValueProperty);
            set => SetValue(LowerValueProperty, value);
        }

        public double UpperValue
        {
            get => GetValue(UpperValueProperty);
            set => SetValue(UpperValueProperty, value);
        }

        private static double CoerceLowerValue(AvaloniaObject sender, double value)
        {
            if (sender is RangeSlider slider)
            {
                if (value < slider.Minimum) return slider.Minimum;
                if (value > slider.UpperValue) return slider.UpperValue;
            }
            return value;
        }

        private static double CoerceUpperValue(AvaloniaObject sender, double value)
        {
            if (sender is RangeSlider slider)
            {
                if (value > slider.Maximum) return slider.Maximum;
                if (value < slider.LowerValue) return slider.LowerValue;
            }
            return value;
        }

        public RangeSlider()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MinimumProperty ||
                change.Property == MaximumProperty ||
                change.Property == LowerValueProperty ||
                change.Property == UpperValueProperty)
            {
                UpdateUI();
            }
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateUI();
        }

        private void LowerThumb_DragDelta(object? sender, VectorEventArgs e)
        {
            UpdateValueFromDrag(e.Vector.X, isLower: true);
        }

        private void UpperThumb_DragDelta(object? sender, VectorEventArgs e)
        {
            UpdateValueFromDrag(e.Vector.X, isLower: false);
        }

        private void UpdateValueFromDrag(double deltaX, bool isLower)
        {
            if (TrackCanvas == null) return;

            double trackWidth = TrackCanvas.Bounds.Width;
            if (trackWidth <= 0) return;

            double range = Maximum - Minimum;
            if (range <= 0) return;

            double valueChange = (deltaX / trackWidth) * range;

            if (isLower)
            {
                LowerValue += valueChange;
            }
            else
            {
                UpperValue += valueChange;
            }
        }

        private void UpdateUI()
        {
            if (TrackCanvas == null || LowerThumb == null || UpperThumb == null || ActiveTrack == null)
                return;

            double trackWidth = TrackCanvas.Bounds.Width;
            if (trackWidth <= 0) return;

            double range = Maximum - Minimum;
            if (range <= 0) return;

            double lowerPercent = (LowerValue - Minimum) / range;
            double upperPercent = (UpperValue - Minimum) / range;

            double lowerX = lowerPercent * trackWidth;
            double upperX = upperPercent * trackWidth;

            double lowerThumbOffset = LowerThumb.Bounds.Width > 0 ? LowerThumb.Bounds.Width / 2 : 8;
            double upperThumbOffset = UpperThumb.Bounds.Width > 0 ? UpperThumb.Bounds.Width / 2 : 8;

            Canvas.SetLeft(LowerThumb, lowerX - lowerThumbOffset);
            Canvas.SetLeft(UpperThumb, upperX - upperThumbOffset);

            Canvas.SetLeft(ActiveTrack, lowerX);
            ActiveTrack.Width = Math.Max(0, upperX - lowerX);
        }
    }
}
