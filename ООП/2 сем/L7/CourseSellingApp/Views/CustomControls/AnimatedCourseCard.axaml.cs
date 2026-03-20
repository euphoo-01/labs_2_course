using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CourseSellingApp.Views.CustomControls
{
    public partial class AnimatedCourseCard : UserControl
    {
        // 1. Avalonia's equivalent of WPF's DependencyProperty with ValidateValueCallback and CoerceValueCallback
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

        // ValidateValueCallback equivalent: checks if the value is fundamentally valid
        private static bool ValidateHoverScale(double value)
        {
            // The scale must be a positive number
            return value > 0;
        }

        // CoerceValueCallback equivalent: adjusts the value to fit within specific constraints
        private static double CoerceHoverScale(AvaloniaObject sender, double value)
        {
            // Coerce the scale to be strictly between 1.0 and 2.0
            if (value < 1.0) return 1.0;
            if (value > 2.0) return 2.0;
            return value;
        }

        // 2. RoutedEvents (Bubble, Tunnel, Direct)

        // Bubbling RoutedEvent: travels up the visual tree
        public static readonly RoutedEvent<RoutedEventArgs> CardClickedEvent =
            RoutedEvent.Register<AnimatedCourseCard, RoutedEventArgs>(
                nameof(CardClicked),
                RoutingStrategies.Bubble);

        public event EventHandler<RoutedEventArgs> CardClicked
        {
            add => AddHandler(CardClickedEvent, value);
            remove => RemoveHandler(CardClickedEvent, value);
        }

        // Tunneling RoutedEvent: travels down the visual tree
        public static readonly RoutedEvent<RoutedEventArgs> CardPreviewClickedEvent =
            RoutedEvent.Register<AnimatedCourseCard, RoutedEventArgs>(
                nameof(CardPreviewClicked),
                RoutingStrategies.Tunnel);

        public event EventHandler<RoutedEventArgs> CardPreviewClicked
        {
            add => AddHandler(CardPreviewClickedEvent, value);
            remove => RemoveHandler(CardPreviewClickedEvent, value);
        }

        // Direct RoutedEvent: only affects the element itself, no routing
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

            // Triggering the Tunneling event first (Preview)
            var previewEventArgs = new RoutedEventArgs(CardPreviewClickedEvent);
            RaiseEvent(previewEventArgs);

            // If the tunneling event wasn't handled, trigger the Bubbling event
            if (!previewEventArgs.Handled)
            {
                var clickEventArgs = new RoutedEventArgs(CardClickedEvent);
                RaiseEvent(clickEventArgs);
            }
        }

        protected override void OnPointerEntered(PointerEventArgs e)
        {
            base.OnPointerEntered(e);

            // Triggering the Direct event
            var hoverEventArgs = new RoutedEventArgs(CardHoveredEvent);
            RaiseEvent(hoverEventArgs);
        }
    }
}
