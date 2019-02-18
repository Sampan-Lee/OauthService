using JiYun.AuthorizationServer.Repositories.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JiYun.AuthorizationServer.Models.Repositories
{
    public class RefreshTokenInfoRepository:IDisposable, IRefreshTokenInfoRepository
    {
        private DBContext dBContext = new DBContext();
        //添加刷新令牌
        public async Task<bool> AddRefreshToken(RefreshTokenInfo refreshToken)
        {
            var existingToken = dBContext.RefreshTokenInfo.FirstOrDefault(ti => ti.client_id == refreshToken.client_id);

            if(existingToken!= null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }
            dBContext.RefreshTokenInfo.Add(refreshToken);
            //var tokenInfo = await dBContext.TokenInfo.FindAsync(ID);
            //tokenInfo.refresh_token = refreshToken;
            return await dBContext.SaveChangesAsync() > 0;
        }
        //删除刷新令牌
        private async Task<bool> RemoveRefreshToken(RefreshTokenInfo refreshToken)
        {
            dBContext.RefreshTokenInfo.Remove(refreshToken);
            return await dBContext.SaveChangesAsync() > 0;
        }
        //根据TokenID删除刷新令牌
        public async Task<bool> RemoveRefreshTokenByID(string refreshTokenID)
        {
            var refreshToken = await dBContext.RefreshTokenInfo.FindAsync(refreshTokenID);
            if (refreshToken != null)
            {
                dBContext.RefreshTokenInfo.Remove(refreshToken);
                return await dBContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        //按令牌ID查找刷新令牌
        public async Task<RefreshTokenInfo> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await dBContext.RefreshTokenInfo.FindAsync(refreshTokenId);
            return refreshToken;
        }
        //获取所有刷新令牌
        public List<RefreshTokenInfo> GetAllRefreshTokens()
        {
            return dBContext.RefreshTokenInfo.ToList();
        }
        //此方法用于检查和验证客户端凭据
        public void Dispose()
        {
            dBContext.Dispose();
        }
    }
}