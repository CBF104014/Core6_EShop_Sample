using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;

namespace Core6_EShop.Service.Implement
{
    public class CountryService : BaseService<Country>
    {
        private CountryRepository countryRepository { get; set; }
        public CountryService(CountryRepository countryRepository) : base(countryRepository)
        {
            this.countryRepository = countryRepository;
        }
    }
}
