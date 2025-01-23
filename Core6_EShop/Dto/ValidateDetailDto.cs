namespace Core6_EShop.Dto
{
    public class ValidateDetailDto
    {
        public string fieldFullName { get; set; }
        public string fieldName
        {
            get
            {
                if(String.IsNullOrEmpty(this.fieldFullName))
                    return String.Empty;
                var arr = this.fieldFullName.Split('.');
                return arr[arr.Length - 1];
            }
        }
        public IEnumerable<string> errorDatas { get; set; }
    }
}
