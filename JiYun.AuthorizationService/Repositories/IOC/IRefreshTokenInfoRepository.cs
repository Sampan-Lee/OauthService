using JiYun.AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiYun.AuthorizationService.Repositories.IOC
{
    public interface IRefreshTokenInfoRepository
    {
        Task<bool> AddRefreshToken(RefreshTokenInfo refreshToken);        
        Task<bool> RemoveRefreshTokenByID(string refreshTokenID);
        Task<RefreshTokenInfo> FindRefreshToken(string refreshTokenId);
        List<RefreshTokenInfo> GetAllRefreshTokens();
    }
}
