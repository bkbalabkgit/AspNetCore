using System.Collections.ObjectModel;

namespace WpfMvvmApp.ViewModels
{
    public class OcrTableSectionViewModel
    {
        public string Header { get; set; } = string.Empty;
        public int ColumnCount { get; set; }
        public ObservableCollection<DocFieldViewModel> Headers { get; set; } = new ObservableCollection<DocFieldViewModel>();
        public ObservableCollection<OcrTableRowViewModel> Rows { get; set; } = new ObservableCollection<OcrTableRowViewModel>();
    }
}
