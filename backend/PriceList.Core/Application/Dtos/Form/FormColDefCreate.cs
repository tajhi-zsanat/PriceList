namespace PriceList.Core.Application.Dtos.Form
{
    public sealed class FormColDefCreate
    {
        public int FormId { get; set; }
        public required List<string> CustomColDef { get; set; }
    }
}
