using System.Collections.ObjectModel;

namespace WpfMvvmApp.ViewModels
{
    public class OcrTableRowViewModel
    {
        public string EntryNo { get; set; } = string.Empty;
        public int ColumnCount { get; set; }
        public ObservableCollection<DocFieldViewModel> Cells { get; set; } = new ObservableCollection<DocFieldViewModel>();
    }
}
