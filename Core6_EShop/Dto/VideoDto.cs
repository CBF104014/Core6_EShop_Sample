namespace Core6_EShop.Dto
{
    public class VideoDto
    {
        public string name { get; set; }
        public string videoSrc { get { return $"/video/{name}"; } }
    }
}
