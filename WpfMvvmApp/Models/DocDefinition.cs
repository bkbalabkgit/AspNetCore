using System.Text.Json.Serialization;

namespace WpfMvvmApp.Models;

public class DocDefinition
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("imagePath")]
    public string ImagePath { get; set; } = string.Empty;

    [JsonPropertyName("sections")]
    public List<DocSectionDefinition> Sections { get; set; } = [];
}

public class DocSectionDefinition
{
    [JsonPropertyName("header")]
    public string Header { get; set; } = string.Empty;

    [JsonPropertyName("columns")]
    public int Columns { get; set; } = 2;

    [JsonPropertyName("fields")]
    public List<DocFieldDefinition> Fields { get; set; } = [];
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
}
