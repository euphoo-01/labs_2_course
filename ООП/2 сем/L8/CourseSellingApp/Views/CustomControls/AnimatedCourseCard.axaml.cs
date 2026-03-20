using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CourseSellingApp.Views.CustomControls
{
    public partial class AnimatedCourseCard : UserControl
    {
        public static readonly StyledProperty<double> HoverScaleProperty =
            AvaloniaProperty.Register<AnimatedCourseCard, double>(
                nameof(HoverScale),
                defaultValue: 1.05,
                validate: ValidateHoverScale,
                coerce: CoerceHoverScale);

        public double HoverScale
        {
            get => GetValue(HoverScaleProperty);
            set => SetValue(HoverScaleProperty, value);
        }

        private static bool ValidateHoverScale(double value)
        {
            return value > 0;
        }

        private static double CoerceHoverScale(AvaloniaObject sender, double value)
        {
            if (value < 1.0) return 1.0;
            if (value > 2.0) return 2.0;
            return value;
        }

        public static readonly RoutedEvent<RoutedEventArgs> CardClickedEvent =
            RoutedEvent.Register<AnimatedCourseCard, RoutedEventArgs>(
                nameof(CardClicked),
                RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> CardClicked
        {
            add => AddHandler(CardClickedEvent, value);
            remove => RemoveHandler(CardClickedEvent, value);
        }

        public static readonly RoutedEvent<RoutedEventArgs> CardPreviewClickedEvent =
            RoutedEvent.Register<AnimatedCourseCard, RoutedEventArgs>(
                nameof(CardPreviewClicked),
                RoutingStrategies.Tunnel);

        public event EventHandler<RoutedEventArgs> CardPreviewClicked
        {
            add => AddHandler(CardPreviewClickedEvent, value);
            remove => RemoveHandler(CardPreviewClickedEvent, value);
        }

        public static readonly RoutedEvent<RoutedEventArgs> CardHoveredEvent =
            RoutedEvent.Register<AnimatedCourseCard, RoutedEventArgs>(
                nameof(CardHovered),
                RoutingStrategies.Direct);

        public event EventHandler<RoutedEventArgs> CardHovered
        {
            add => AddHandler(CardHoveredEvent, value);
            remove => RemoveHandler(CardHoveredEvent, value);
        }

        public AnimatedCourseCard()
        {
            InitializeComponent();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            var previewEventArgs = new RoutedEventArgs(CardPreviewClickedEvent);
            RaiseEvent(previewEventArgs);

            if (!previewEventArgs.Handled)
            {
                var clickEventArgs = new RoutedEventArgs(CardClickedEvent);
                RaiseEvent(clickEventArgs);
            }
        }

        protected override void OnPointerEntered(PointerEventArgs e)
        {
            base.OnPointerEntered(e);

            var hoverEventArgs = new RoutedEventArgs(CardHoveredEvent);
            RaiseEvent(hoverEventArgs);
        }
    }
}
