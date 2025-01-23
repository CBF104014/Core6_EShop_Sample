namespace Core6_EShop.Dto
{
    public class ValidateDto
    {
        public IEnumerable<ValidateDetailDto> validateDatas { get; set; }
        public bool isValid { get; set; }
    }
}
