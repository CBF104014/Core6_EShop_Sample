namespace Core6_EShop.Dto
{
    public class TableCodeDto
    {
        public TableCodeDto(string TableName, string TableFullName)
        {
            this.TableName = TableName;
            this.TableFullName = TableFullName;
        }
        public string TableName { get; }
        public string TableFullName { get; }
    }
}
