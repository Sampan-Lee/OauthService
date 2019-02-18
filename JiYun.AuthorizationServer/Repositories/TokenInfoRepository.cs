using JiYun.AuthorizationServer.Repositories.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JiYun.AuthorizationServer.Models.Repositories
{
    public class TokenInfoRepository : IDisposable, ITokenInfoRepository
    {
        private DBContext dBContext = new DBContext();
        public async Task<bool> Add(TokenInfo tokenInfo)
        {
            tokenInfo.ID = dBContext.ID;
            dBContext.TokenInfo.Add(tokenInfo);
            var result= await dBContext.SaveChangesAsync();
            return result == 1;
        }
        public async Task<bool> Modify(string ID, string refreshToken)
        {
            var tokenInfo = await dBContext.TokenInfo.FindAsync(ID);
            tokenInfo.refresh_token = refreshToken;
            var result = await dBContext.SaveChangesAsync();
            return result == 1;
        }
        public void Dispose()
        {
            dBContext.Dispose();
        }
    }
}