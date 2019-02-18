using JiYun.AuthorizationService.Models;
using JiYun.AuthorizationService.Models.Repositories;
using JiYun.AuthorizationService.Repositories.IOC;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JiYun.AuthorizationService.Providers
{
    public class JyAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        public IAuthorizationInfoRepository authorizationInfoRepository = DependencyResolver.Current.GetService<IAuthorizationInfoRepository>();
        public ITokenInfoRepository tokenInfoRepository = DependencyResolver.Current.GetService<ITokenInfoRepository>();
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string client_id;
            string client_secret;
            if (context.TryGetBasicCredentials(out client_id, out client_secret))
            {                
                var authorizationInfo = authorizationInfoRepository.ValidateClient(client_id, client_secret);
                if (authorizationInfo != null)
                {

                    if ((bool)authorizationInfo.Active)
                    {
                        context.OwinContext.Set<AuthorizationInfo>("client", authorizationInfo);
                        context.OwinContext.Set<string>("refreshToken", Guid.NewGuid().ToString("n"));
                        context.Validated(client_id);
                    }
                    else
                    {
                        context.SetError("invalid_client", "客户端凭据失效");
                    }
                }
                else
                {
                    context.SetError("invalid_client", "客户端凭据不存在");
                }
            }
            else
            {
                context.SetError("invalid_client", "无法通过Authorization标头检索客户端凭据。");
            }
            return base.ValidateClientAuthentication(context);
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "client_id",context.ClientId
                },
                {
                    "client_ip",HttpContext.Current.Request.UserHostAddress
                },
                {
                    "browser_type",HttpContext.Current.Request.Browser.Type
                },
                {
                    "browser_version",HttpContext.Current.Request.Browser.Version
                }
            });
            //设置返回值中的refreshtoken过期时间，展示给客户端
            var client = context.OwinContext.Get<AuthorizationInfo>("client");
            var issued = DateTime.Now;
            var expired = issued.AddMinutes(Convert.ToDouble(client.refresh_token_life_time));
            props.Dictionary.Add("issued", issued.ToString("yyyy-MM-dd HH:mm:ss"));
            props.Dictionary.Add("expires", expired.ToString("yyyy-MM-dd HH:mm:ss"));
            //授权ticket
            var ticket = new AuthenticationTicket(oAuthIdentity, props);     
            context.Validated(ticket);
            return base.GrantClientCredentials(context);
        }


        public override async Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            await tokenInfoRepository.Add(new TokenInfo()
            {
                client_id = context.Properties.Dictionary["client_id"],
                access_token = context.AccessToken,
                token_type = context.Options.AuthenticationType,
                expires_in = Convert.ToInt32(Startup.OAuthOptions.AccessTokenExpireTimeSpan.TotalSeconds),
                refresh_token = context.OwinContext.Get<string>("refreshToken"),
                client_ip = context.Properties.Dictionary["client_ip"],
                browser_type = context.Properties.Dictionary["browser_type"],
                browser_version = context.Properties.Dictionary["browser_version"]
            });
        }


        /// <summary>
        /// TokenEndpoint
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                if (property.Key.Equals(".refresh") ||
                    property.Key.Equals("client_id") ||
                    property.Key.Equals(".issued") ||
                    property.Key.Equals(".expires")||
                    property.Key.Equals("client_ip") ||
                    property.Key.Equals("browser_type") ||
                    property.Key.Equals("browser_version"))
                {
                    continue;
                }
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }        
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["client_id"];
            var currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId”，“刷新令牌发给另一个clientId。");
                return Task.FromResult<object>(null);
            }
            //更改刷新令牌请求的身份验证票证
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim","newValue"));
            var client = context.OwinContext.Get<AuthorizationInfo>("client");
            var issued = DateTime.Now;
            var expired = issued.AddMinutes(Convert.ToDouble(client.refresh_token_life_time));
            context.Ticket.Properties.Dictionary["issued"] = issued.ToString("yyyy-MM-dd HH:mm:ss");
            context.Ticket.Properties.Dictionary["expires"] = expired.ToString("yyyy-MM-dd HH:mm:ss");
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
            return Task.FromResult<object>(null);            
        }
    }
}