using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ThePiperAtTheGatesOfDawnTools.ViewModels;

namespace ThePiperAtTheGatesOfDawnTools.Views;

public partial class TrackPositionView : UserControl
{
    public TrackPositionView()
    {
        InitializeComponent();
        DataContext = new TrackPositionViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}