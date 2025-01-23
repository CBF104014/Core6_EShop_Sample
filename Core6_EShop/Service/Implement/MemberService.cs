using Core6_EShop.Cls;
using Core6_EShop.Dto;
using Core6_EShop.Models;
using Core6_EShop.Repository.Implement;
using Core6_EShop.Service.Base;

namespace Core6_EShop.Service.Implement
{
    public class MemberService : BaseService<Member>
    {
        private MemberRepository memberRepository { get; set; }
        public MemberService(MemberRepository memberRepository) : base(memberRepository)
        {
            this.memberRepository = memberRepository;
        }
        public Member GetSampleData()
        {
            return memberRepository.GetSampleData();
        }
        public async Task<int> GetNewMemberId()
        {
            return await memberRepository.GetNewMemberId();
        }
        public async Task<Member> SelByEmail(string email)
        {
            return await memberRepository.SelByEmail(email);
        }
        public async Task<Member> SelById(int memberId)
        {
            return await memberRepository.SelById(memberId);
        }
        public async Task<APIDto> SaveData(Member memberData)
        {
            if (memberData.rankey > 0)
            {
                var dbData = await SelById(memberData.memberId);
                //若有更改email，檢查email是否重複
                if (dbData.email != memberData.email)
                {
                    dbData = await SelByEmail(memberData.email);
                    if (dbData != null)
                        return new APIDto((int)Code.stateCode.error, "該電子郵件已存在", "", new { });
                }
                await memberRepository.UpdByRankey(memberData);
                return new APIDto((int)Code.stateCode.success, "存檔成功", "", new { });
            }
            else
            {
                //檢查email是否重複
                var dbData = await SelByEmail(memberData.email);
                if (dbData != null)
                    return new APIDto((int)Code.stateCode.error, "該電子郵件已存在", "", new { });
                memberData.memberId = await GetNewMemberId();
                 await memberRepository.Create(memberData);
                return new APIDto((int)Code.stateCode.success, "新增成功", "", new { });
            }
        }
    }
}
