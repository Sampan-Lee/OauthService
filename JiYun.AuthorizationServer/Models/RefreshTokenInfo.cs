
using System;

namespace JiYun.AuthorizationServer.Models
{
    public  class RefreshTokenInfo
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
		/// 发行时间
		/// </summary>		
        public DateTime? issued { get; set; }
	        /// <summary>
		/// 过期时间
		/// </summary>		
        public DateTime? expired { get; set; }
	        /// <summary>
		/// 受保护票据
		/// </summary>		
        public string protected_ticket { get; set; }
	 }
}
