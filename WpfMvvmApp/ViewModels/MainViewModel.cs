using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfMvvmApp.Models;

namespace WpfMvvmApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly string _docDefsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DocDefs");

        public string Title { get; private set; } = "OCR Verification";

        public Uri? FormImageUri { get; private set; }

        public ObservableCollection<DocumentOptionViewModel> DocumentTypes { get; } = new ObservableCollection<DocumentOptionViewModel>();

        private DocumentOptionViewModel? _selectedDocumentType;
        public DocumentOptionViewModel? SelectedDocumentType
        {
            get => _selectedDocumentType;
            set
            {
                _selectedDocumentType = value;
                OnPropertyChanged();
                if (value != null)
                {
                    LoadDocDefinition(value.FileName);
                }
            }
        }

        public ObservableCollection<DocSectionViewModel> Sections { get; } = new ObservableCollection<DocSectionViewModel>();

        public ObservableCollection<OcrTableSectionViewModel> TableSections { get; } = new ObservableCollection<OcrTableSectionViewModel>();

        public MainViewModel()
        {
            LoadDocumentCatalog();
        }

        private void LoadDocumentCatalog()
        {
            DocumentTypes.Clear();

            if (!Directory.Exists(_docDefsDirectory))
            {
                return;
            }

            var files = Directory.GetFiles(_docDefsDirectory, "*.json")
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var displayName = Path.GetFileNameWithoutExtension(fileName)
                    .Replace('-', ' ')
                    .Replace('_', ' ');

                DocumentTypes.Add(new DocumentOptionViewModel
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(displayName),
                    FileName = fileName
                });
            }

            SelectedDocumentType = DocumentTypes.FirstOrDefault();
        }

        private void LoadDocDefinition(string fileName)
        {
            var jsonPath = Path.Combine(_docDefsDirectory, fileName);
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
            if (docDef == null)
            if (docDef is null)
            {
                return;
            }

            Title = docDef.Title;
            OnPropertyChanged(nameof(Title));

            FormImageUri = null;
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, docDef.ImagePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(imagePath))
            {
                FormImageUri = new Uri(imagePath, UriKind.Absolute);
            }
            OnPropertyChanged(nameof(FormImageUri));
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

            TableSections.Clear();
            foreach (var tableSection in docDef.TableSections)
            {
                TableSections.Add(MapTableSection(tableSection));
            }
        }

        private static OcrTableSectionViewModel MapTableSection(DocTableSectionDefinition tableSection)
        {
            var sectionVm = new OcrTableSectionViewModel
            {
                Header = tableSection.Header,
                ColumnCount = Math.Max(1, tableSection.Columns.Count),
                Headers = new ObservableCollection<DocFieldViewModel>(tableSection.Columns.Select(column => new DocFieldViewModel
                {
                    Key = column.Key,
                    Label = column.Label,
                    IsText = true,
                    TextValue = column.Label
                }))
            };

            foreach (var row in tableSection.Rows)
            {
                var cellsByKey = row.Cells.ToDictionary(x => x.Key, x => x, StringComparer.OrdinalIgnoreCase);
                var rowVm = new OcrTableRowViewModel { EntryNo = row.EntryNo, ColumnCount = sectionVm.ColumnCount };

                foreach (var column in tableSection.Columns)
                {
                    DocTableCellDefinition? cell;
                    cellsByKey.TryGetValue(column.Key, out cell);

                    rowVm.Cells.Add(MapField(new DocFieldDefinition
                    {
                        Key = column.Key,
                        Label = column.Label,
                        Type = column.Type,
                        Value = cell != null ? cell.Value : string.Empty,
                        Confidence = cell != null ? cell.Confidence : 0.0
                    }));
                }

                sectionVm.Rows.Add(rowVm);
            }

            return sectionVm;
        }

        private static DocFieldViewModel MapField(DocFieldDefinition field)
        {
            var type = field.Type.Trim().ToLowerInvariant();
            var vm = new DocFieldViewModel
            {
                Key = field.Key,
                Label = field.Label,
                Placeholder = field.Placeholder,
                Confidence = field.Confidence <= 0 ? 0.5 : field.Confidence,
                IsDate = type == "date",
                IsMultiline = type == "multiline",
                IsCheckbox = type == "checkbox",
                IsText = type == "text" || type == string.Empty || (type != "date" && type != "multiline" && type != "checkbox")
            };

            if (vm.IsDate)
            {
                DateTime parsedDate;
                if (DateTime.TryParse(field.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate)
                    || DateTime.TryParse(field.Value, out parsedDate))
                {
                    vm.DateValue = parsedDate;
                }
                else
                {
                    vm.TextValue = field.Value;
                }
            }
            else if (vm.IsCheckbox)
            {
                bool parsedBool;
                if (bool.TryParse(field.Value, out parsedBool))
                {
                    vm.BoolValue = parsedBool;
                }
            }
            else
            {
                vm.TextValue = field.Value;
            }

            return vm;
        }
    }
}
