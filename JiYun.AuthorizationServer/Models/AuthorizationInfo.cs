using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiYun.AuthorizationServer.Models
{
    public class AuthorizationInfo
    {
        /// <summary>
		/// 
		/// </summary>		
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string client_id { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string client_secret { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string grant_type { get; set; }
        /// <summary>
        /// 刷新令牌生存时间
        /// </summary>
        public int? refresh_token_life_time { get; set; }
        /// <summary>
        /// 授权客户
        /// </summary>
        public string allowed_origin { get; set; }
        /// <summary>
        /// 0：失效；1：有效
        /// </summary>		
        public bool? Active { get; set; }
        /// <summary>
        /// 0：未删除；1：已删除
        /// </summary>		
        public bool? IsDel { get; set; }
    }
}