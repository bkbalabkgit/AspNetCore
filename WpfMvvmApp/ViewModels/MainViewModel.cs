using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfMvvmApp.Models;

namespace WpfMvvmApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Title { get; private set; } = "OCR Verification";

        public Uri? FormImageUri { get; private set; }

        public ObservableCollection<DocSectionViewModel> Sections { get; } = new ObservableCollection<DocSectionViewModel>();

        public MainViewModel()
        {
            LoadDocDefinition();
        }

        private void LoadDocDefinition()
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "dental-docdef.json");
            if (!File.Exists(jsonPath))
            {
                return;
            }

            var docDef = JsonSerializer.Deserialize<DocDefinition>(File.ReadAllText(jsonPath));
            if (docDef is null)
            {
                return;
            }

            Title = docDef.Title;
            OnPropertyChanged(nameof(Title));

            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, docDef.ImagePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(imagePath))
            {
                FormImageUri = new Uri(imagePath, UriKind.Absolute);
                OnPropertyChanged(nameof(FormImageUri));
            }

            Sections.Clear();
            foreach (var section in docDef.Sections)
            {
                Sections.Add(new DocSectionViewModel
                {
                    Header = section.Header,
                    Columns = section.Columns < 1 ? 1 : section.Columns,
                    Fields = new ObservableCollection<DocFieldViewModel>(section.Fields.Select(MapField))
                });
            }
        }

        private static DocFieldViewModel MapField(DocFieldDefinition field)
        {
            var type = field.Type.Trim().ToLowerInvariant();

            return new DocFieldViewModel
            {
                Key = field.Key,
                Label = field.Label,
                Placeholder = field.Placeholder,
                IsDate = type == "date",
                IsMultiline = type == "multiline",
                IsCheckbox = type == "checkbox",
                IsText = type == "text" || type == string.Empty || (type != "date" && type != "multiline" && type != "checkbox")
            };
        }
    }
}
