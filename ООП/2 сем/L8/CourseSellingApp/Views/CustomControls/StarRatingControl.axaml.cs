using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace CourseSellingApp.Views.CustomControls
{
    public partial class StarRatingControl : UserControl
    {
        public static readonly StyledProperty<int> MaxStarsProperty =
            AvaloniaProperty.Register<StarRatingControl, int>(
                nameof(MaxStars),
                defaultValue: 5,
                validate: ValidateMaxStars,
                coerce: CoerceMaxStars);

        public int MaxStars
        {
            get => GetValue(MaxStarsProperty);
            set => SetValue(MaxStarsProperty, value);
        }

        private static bool ValidateMaxStars(int value)
        {
            return value > 0;
        }

        private static int CoerceMaxStars(AvaloniaObject sender, int value)
        {
            if (value > 10) return 10;
            if (value < 1) return 1;
            return value;
        }

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<StarRatingControl, double>(
                nameof(Value),
                defaultValue: 0.0,
                validate: ValidateValue,
                coerce: CoerceValue);

        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static bool ValidateValue(double value)
        {
            return value >= 0;
        }

        private static double CoerceValue(AvaloniaObject sender, double value)
        {
            var control = (StarRatingControl)sender;
            if (value > control.MaxStars) return control.MaxStars;
            if (value < 0) return 0;
            return value;
        }

        // 2. Routed Events
        public static readonly RoutedEvent<RoutedEventArgs> RatingChangedEvent =
            RoutedEvent.Register<StarRatingControl, RoutedEventArgs>(
                nameof(RatingChanged),
                RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> RatingChanged
        {
            add => AddHandler(RatingChangedEvent, value);
            remove => RemoveHandler(RatingChangedEvent, value);
        }

        private StackPanel? _starsPanel;

        public StarRatingControl()
        {
            InitializeComponent();
            _starsPanel = this.FindControl<StackPanel>("StarsPanel");
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MaxStarsProperty || change.Property == ValueProperty)
            {
                UpdateStars();
            }
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            UpdateStars();
        }

        private void UpdateStars()
        {
            if (_starsPanel == null) return;

            _starsPanel.Children.Clear();

            for (var i = 1; i <= MaxStars; i++)
            {
                var starPath = new Avalonia.Controls.Shapes.Path
                {
                    Data = StreamGeometry.Parse(
                        "M12,17.27L18.18,21L16.54,13.97L22,9.24L14.81,8.62L12,2L9.19,8.62L2,9.24L7.45,13.97L5.82,21L12,17.27Z"),
                    Fill = i <= Value ? Brushes.Gold : Brushes.Gray,
                    Width = 24,
                    Height = 24,
                    Stretch = Stretch.Uniform,
                    Tag = i,
                    Cursor = new Cursor(StandardCursorType.Hand)
                };

                starPath.PointerPressed += StarPath_PointerPressed;
                _starsPanel.Children.Add(starPath);
            }
        }

        private void StarPath_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (sender is Avalonia.Controls.Shapes.Path path && path.Tag is int rating)
            {
                Value = rating;
                RaiseEvent(new RoutedEventArgs(RatingChangedEvent, this));
            }
        }
    }
}
