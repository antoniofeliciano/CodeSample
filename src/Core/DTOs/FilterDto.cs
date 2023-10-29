namespace Core.DTOs
{
    public class FilterDto
    {
        public string? ColumnName { get; set; }
        public string? ColumnValue { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderByDirection { get; set; }
    }
}
