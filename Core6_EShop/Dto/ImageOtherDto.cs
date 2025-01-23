namespace Core6_EShop.Dto
{
    public class ImageOtherDto
    {
        public string name { get; set; }
        public string path { get { return $"/image/other/{name}"; } }
    }
}
