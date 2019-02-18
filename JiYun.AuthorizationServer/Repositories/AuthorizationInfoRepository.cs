using JiYun.AuthorizationServer.Repositories.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiYun.AuthorizationServer.Models.Repositories
{
    public class AuthorizationInfoRepository:IDisposable, IAuthorizationInfoRepository
    {
        DBContext dBContext = new DBContext();
        public AuthorizationInfo ValidateClient(string client_id, string client_secret)
        {
            return dBContext.AuthorizationInfo.FirstOrDefault(
                ti =>
                ti.client_id.Equals(client_id) &&
                ti.client_secret.Equals(client_secret) &&
                ti.IsDel.Value == false
            );
        }
        public void Dispose()
        {
            dBContext.Dispose();
        }
    }
}