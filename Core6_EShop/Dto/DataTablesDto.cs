namespace Core6_EShop.Dto
{
    public class DataTablesDto
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string searchValue { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
        public object dataParameter { get; set; }
    }
}
