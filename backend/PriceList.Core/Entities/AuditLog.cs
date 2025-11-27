using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class AuditLog : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public string ActionType { get; set; } = default!; // e.g. "Login", "Create", "Update", "Delete"

        public string? EntityName { get; set; }
        public string? EntityId { get; set; }

        public string? Route { get; set; }
        public string? HttpMethod { get; set; }

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public string? OldValuesJson { get; set; }
        public string? NewValuesJson { get; set; }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public enum ActionType
    {
        Login,
        LogOut,
        Create,
        Update,
        Delete,
    }

    public enum EntityName
    {
        User,
        Feature,
        Form,
        FormRow,
        FormCell,
        FormCellMedia,
        FormCellImage,
        FormCol,
        FormCellHeader,
        Category,
        CategoryImage,
        ProductGroup,
        ProductGroupImage,
        Brand,
        Brandimage
    }
}
