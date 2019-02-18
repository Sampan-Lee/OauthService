using JiYun.AuthorizationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiYun.AuthorizationServer.Repositories.IOC
{
    public interface IAuthorizationInfoRepository
    {
        AuthorizationInfo ValidateClient(string client_id, string client_secret);
    }
}
