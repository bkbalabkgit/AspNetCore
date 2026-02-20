using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WpfMvvmApp.Models
{
    public class DocDefinition
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("imagePath")]
        public string ImagePath { get; set; } = string.Empty;

        [JsonPropertyName("sections")]
        public List<DocSectionDefinition> Sections { get; set; } = new List<DocSectionDefinition>();

        [JsonPropertyName("tableSections")]
        public List<DocTableSectionDefinition> TableSections { get; set; } = new List<DocTableSectionDefinition>();
    }

    public class DocSectionDefinition
    {
        [JsonPropertyName("header")]
        public string Header { get; set; } = string.Empty;

        [JsonPropertyName("columns")]
        public int Columns { get; set; } = 2;

        [JsonPropertyName("fields")]
        public List<DocFieldDefinition> Fields { get; set; } = new List<DocFieldDefinition>();
    }

    public class DocFieldDefinition
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";

        [JsonPropertyName("placeholder")]
        public string Placeholder { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; } = 1.0;
    }

    public class DocTableSectionDefinition
    {
        [JsonPropertyName("header")]
        public string Header { get; set; } = string.Empty;

        [JsonPropertyName("columns")]
        public List<DocTableColumnDefinition> Columns { get; set; } = new List<DocTableColumnDefinition>();

        [JsonPropertyName("rows")]
        public List<DocTableRowDefinition> Rows { get; set; } = new List<DocTableRowDefinition>();
    }

    public class DocTableColumnDefinition
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "text";
    }

    public class DocTableRowDefinition
    {
        [JsonPropertyName("entryNo")]
        public string EntryNo { get; set; } = string.Empty;

        [JsonPropertyName("cells")]
        public List<DocTableCellDefinition> Cells { get; set; } = new List<DocTableCellDefinition>();
    }

    public class DocTableCellDefinition
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; } = 1.0;
    }
}
