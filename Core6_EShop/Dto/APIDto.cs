using static Core6_EShop.Cls.Code;

namespace Core6_EShop.Dto
{
    public class APIDto
    {
        public APIDto(int _state, string _title, string _message = "", object _payload = null)
        {
            this.state = _state;
            this.title = _title ?? "";
            this.message = _message ?? "";
            this.payload = _payload;
        }
        public int state { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public object payload { get; set; }
        public string icon
        {
            get
            {
                if (this.state == (int)stateCode.error)
                    return stateCode.error.ToString();
                else if (this.state == (int)stateCode.success)
                    return stateCode.success.ToString();
                else if (this.state == (int)stateCode.warning)
                    return stateCode.warning.ToString();
                else if (this.state == (int)stateCode.question)
                    return stateCode.question.ToString();
                else return stateCode.info.ToString();
            }
        }
    }
}
