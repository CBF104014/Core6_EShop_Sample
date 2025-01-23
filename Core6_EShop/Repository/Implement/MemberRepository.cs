using Core6_EShop.Cls;
using Core6_EShop.Models;
using Core6_EShop.Repository.Base;

namespace Core6_EShop.Repository.Implement
{
    public class MemberRepository : BaseRepository<Member>
    {
        public MemberRepository() { }
        public Member GetSampleData()
        {
            return new Member()
            {
                memberState = Code.memberStateCode.enable.varValue,
            }.Init();
        }
        public async Task<int> GetNewMemberId()
        {
            var sqlCmd = $"select ifnull(max(memberId),0) memberId from {Code.tableCode.Member.TableFullName}";
            var newMemberId = await QueryFirstAsync<int>(sqlCmd);
            return newMemberId + 1;
        }
        public async Task<Member> SelByEmail(string email)
        {
            return await SelFirst<Member>($"email=@email", new { email });
        }
        public async Task<Member> SelById(int memberId)
        {
            return await SelFirst<Member>($"memberId=@memberId", new { memberId });
        }
    }
}
