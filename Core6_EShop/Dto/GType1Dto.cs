using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.ViewModel;

namespace Core6_EShop.Dto
{
    public class GType1Dto : Setting
    {
        public GType1Dto()
        {
            timestamp = DateTime.Now.Ticks.ToString();
        }
        public string timestamp { get; set; }
        public int defaultValue { get; set; } = Code.gType1Code.noodle.varValue;
        public string navId { get => $"nav_{timestamp}_{varKey}_{varValue}"; }
        public string panelId { get => $"panel_{timestamp}_{varKey}_{varValue}"; }
        public string navClass { get => varValue == defaultValue ? $"nav-link active" : $"nav-link"; }
        public string panelClass { get => varValue == defaultValue ? $"tab-pane fade show active" : $"tab-pane fade"; }
        public string ariaSelected { get => varValue == defaultValue ? $"true" : $"false"; }
        public string dataBsTarget { get => $"#{panelId}"; }
    }
}
