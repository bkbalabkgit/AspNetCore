using System.Collections.ObjectModel;

namespace WpfMvvmApp.ViewModels;

public class DocSectionViewModel
{
    public string Header { get; init; } = string.Empty;
    public int Columns { get; init; } = 2;
    public ObservableCollection<DocFieldViewModel> Fields { get; init; } = [];
}
