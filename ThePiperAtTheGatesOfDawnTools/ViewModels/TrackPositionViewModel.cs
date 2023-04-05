using System.Reactive;
using System.Text;
using Avalonia;
using ReactiveUI;

namespace ThePiperAtTheGatesOfDawnTools.ViewModels;

public class TrackPositionViewModel : ViewModelBase
{
    private string _rawTracks;

    public string RawTracks
    {
        get => _rawTracks;
        set
        {
            IsParsed = false;
            this.RaiseAndSetIfChanged(ref _rawTracks, value);
        }
    }

    private string _parsedTracks;

    public string ParsedTracks
    {
        get => _parsedTracks;
        set => this.RaiseAndSetIfChanged(ref _parsedTracks, value);
    }

    private bool _isParsed;

    public bool IsParsed
    {
        get => _isParsed;
        set => this.RaiseAndSetIfChanged(ref _isParsed, value);
    }

    public ReactiveCommand<Unit, Unit> ParseCommand { get; set; }

    public void Parse()
    {
        var lines = RawTracks.Split('\n');
        var totalSeconds = 0;
        var output = new StringBuilder();
        foreach (var line in lines)
        {
            if (!line.Contains("(") || !line.Contains(")"))
                continue;
            var lastOpenParenthesisIndex = line.LastIndexOf('(');
            var lastCloseParenthesisIndex = line.LastIndexOf(')');
            var duration = line.Substring(lastOpenParenthesisIndex + 1, lastCloseParenthesisIndex - lastOpenParenthesisIndex - 1);
            if (!duration.Contains(":"))
                continue;
            var durationParts = duration.Split(':');
            if (!int.TryParse(durationParts[0], out var minutes) || !int.TryParse(durationParts[1], out var seconds))
                continue;
            output.AppendLine($"{line.Substring(0, lastOpenParenthesisIndex).Trim()} starts at {totalSeconds / 60}:{totalSeconds % 60:D2}");
            totalSeconds += minutes * 60 + seconds;
        }
        
        ParsedTracks = output.ToString();
        IsParsed = true;
    }

    public ReactiveCommand<Unit, Unit> CopyCommand { get; set; }

    public void Copy()
    {
        Application.Current?.Clipboard?.SetTextAsync(ParsedTracks);
    }

    public TrackPositionViewModel()
    {
        ParseCommand = ReactiveCommand.Create(Parse);
        CopyCommand = ReactiveCommand.Create(Copy);
    }
}