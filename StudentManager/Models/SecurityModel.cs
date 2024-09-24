using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManager.Models
{
    public class Security
    {
    }

    public class Role
    {
        [Key]
        public long id_role { get; set; }

        public string role_name { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Navigation property to RolePermissions
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class Permission
    {
        [Key]
        public long id_permission { get; set; }

        public string permission_name { get; set; }
        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        // Navigation property to RolePermissions
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class RolePermission
    {
        [Key]
        public long id_role_permission { get; set; }

        public string created_by_user { get; set; }
        public string last_updated_by_user { get; set; }
        public string deleted_by_user { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

        [ForeignKey("Role")]
        public long id_role { get; set; }

        [ForeignKey("Permission")]
        public long id_permission { get; set; }

        // Navigation property
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
