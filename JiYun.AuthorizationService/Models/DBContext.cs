using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JiYun.AuthorizationService.Models
{
    public partial class DBContext: DbContext
    {
        public DBContext(): base("name=OAuthService")
        {
        }
        public string ID => Guid.NewGuid().ToString("n");
        public virtual DbSet<AuthorizationInfo> AuthorizationInfo { get; set; }
        public virtual DbSet<TokenInfo> TokenInfo { get; set; }
        public virtual DbSet<RefreshTokenInfo> RefreshTokenInfo { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);            
        }

    }
}