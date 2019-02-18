
using System;

namespace JiYun.AuthorizationService.Models
{
    public  class TokenInfo
    {
        /// <summary>
		/// 
		/// </summary>		
        public string ID { get; set; }
	    /// <summary>
		/// 客户端ID
		/// </summary>		
        public string client_id { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>		
        public string access_token { get; set; }
	    /// <summary>
		/// 令牌类型
		/// </summary>		
        public string token_type { get; set; }
	    /// <summary>
		/// 令牌过期秒数
		/// </summary>		
        public int? expires_in { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string refresh_token { get; set; }
	    /// <summary>
		/// 客户端IP地址
		/// </summary>		
        public string client_ip { get; set; }
	    /// <summary>
		/// 浏览器类型
		/// </summary>		
        public string browser_type { get; set; }
	    /// <summary>
		/// 浏览器版本
		/// </summary>		
        public string browser_version { get; set; }
	 }
}
