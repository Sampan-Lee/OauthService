using JiYun.AuthorizationService.Models;
using JiYun.AuthorizationService.Models.Repositories;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JiYun.AuthorizationService.Providers
{
    public class JyRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }        
        /// <summary>
        /// 创建刷新令牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            
            var clientid = context.Ticket.Properties.Dictionary["client_id"];
            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }
            //真实设置refreshtoken的过期时间
            var client = context.OwinContext.Get<AuthorizationInfo>("client");
            var issued = DateTime.UtcNow;
            var expired = issued.AddMinutes(Convert.ToDouble(client.refresh_token_life_time));
            context.Ticket.Properties.IssuedUtc = issued;
            context.Ticket.Properties.ExpiresUtc = expired;
            //生成 refresh_token 并存储到数据库中                 
            string ProtectedTicket = context.SerializeTicket();
            var refreshTokenId = context.OwinContext.Get<string>("refreshToken");
            //创建Refresh Token对象
            var refreshToken = new RefreshTokenInfo()
            {
                //以散列格式存储RefreshTokenId
                ID = refreshTokenId,
                client_id = clientid,
                protected_ticket = ProtectedTicket,
                issued = issued,
                expired = expired
            };
            using (RefreshTokenInfoRepository refreshTokenInfoRepository = new RefreshTokenInfoRepository())
            {
                var result = await refreshTokenInfoRepository.AddRefreshToken(refreshToken);
                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }            
            await Task.FromResult<bool>(true);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 接收刷新令牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            // context.Token  -- refresh_token 
            // 判断 refresh_token 是否可用
            var client= context.OwinContext.Get<AuthorizationInfo>("client");
            if (client != null && (bool)client.Active)
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new string[] { client.allowed_origin });
                using (RefreshTokenInfoRepository refreshTokenInfoRepository = new RefreshTokenInfoRepository())
                {
                    var refreshToken = await refreshTokenInfoRepository.FindRefreshToken(context.Token);
                    if (refreshToken != null)
                    {
                        context.DeserializeTicket(refreshToken.protected_ticket);
                        var result = await refreshTokenInfoRepository.RemoveRefreshTokenByID(context.Token);
                    }
                }
            }
            await Task.FromResult<bool>(true);
        }        
    }
}