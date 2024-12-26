using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System.Threading.Tasks;

namespace belonging.Views;

public partial class MainWindow : Window
{
    private string? _selectedShape;
    private bool _isCheckMode;
    private TextBlock? _resultMessage;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void DrawSquare_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _selectedShape = "Square";
        _isCheckMode = false;
    }

    private void DrawPentagon_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _selectedShape = "Pentagon";
        _isCheckMode = false;
    }

    private void DrawHexagon_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _selectedShape = "Hexagon";
        _isCheckMode = false;
    }
    
    private void CheckAim_OnClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _isCheckMode = true;
    }
    
    private async void OnCanvasClick(object? sender, PointerPressedEventArgs e)
    {
        var pointerPosition = e.GetPosition(DrawingCanvas);

        if (_isCheckMode)
        {
            var hitControl = DrawingCanvas.InputHitTest(pointerPosition) as Shape;
            string message = hitControl != null ? "Ты попал!" : "Ты не попал!";
            await ShowResultMessage(message);
        }
        else if (_selectedShape != null)
        {
            DrawingCanvas.Children.Clear();

            Shape? shape = null;
            double shapeWidth = 0, shapeHeight = 0;

            switch (_selectedShape)
            {
                case "Square":
                    shape = new Rectangle
                    {
                        Width = 50,
                        Height = 50,
                        Fill = Brushes.Blue
                    };
                    shapeWidth = 50;
                    shapeHeight = 50;
                    break;

                case "Pentagon":
                    shape = new Polygon
                    {
                        Points = new Avalonia.Collections.AvaloniaList<Point>
                        {
                            new Point(25, 0),
                            new Point(50, 15),
                            new Point(40, 50),
                            new Point(10, 50),
                            new Point(0, 15)
                        },
                        Fill = Brushes.Green
                    };
                    shapeWidth = 50;
                    shapeHeight = 50;
                    break;

                case "Hexagon":
                    shape = new Polygon
                    {
                        Points = new Avalonia.Collections.AvaloniaList<Point>
                        {
                            new Point(25, 0),
                            new Point(50, 13),
                            new Point(50, 38),
                            new Point(25, 50),
                            new Point(0, 38),
                            new Point(0, 13)
                        },
                        Fill = Brushes.Red
                    };
                    shapeWidth = 50;
                    shapeHeight = 50;
                    break;
            }

            if (shape != null)
            {
                
                double x = Math.Max(0, Math.Min(pointerPosition.X - shapeWidth / 2, DrawingCanvas.Bounds.Width - shapeWidth));
                double y = Math.Max(0, Math.Min(pointerPosition.Y - shapeHeight / 2, DrawingCanvas.Bounds.Height - shapeHeight));

                Canvas.SetLeft(shape, x);
                Canvas.SetTop(shape, y);
                
                DrawingCanvas.Children.Add(shape);
            }
        }
    }

    private async Task ShowResultMessage(string message)
    {
        if (_resultMessage != null)
        {
            DrawingCanvas.Children.Remove(_resultMessage);
            _resultMessage = null;
        }
        
        _resultMessage = new TextBlock
        {
            Text = message,
            FontSize = 20,
            Foreground = Brushes.Black,
            Background = Brushes.LightYellow,
            TextAlignment = TextAlignment.Center
        };
        
        double x = (DrawingCanvas.Bounds.Width - 200) / 2;
        double y = 20;

        Canvas.SetLeft(_resultMessage, x);
        Canvas.SetTop(_resultMessage, y);

        DrawingCanvas.Children.Add(_resultMessage);
        
        await Task.Delay(2000);

        if (_resultMessage != null)
        {
            DrawingCanvas.Children.Remove(_resultMessage);
            _resultMessage = null;
        }
    }
}