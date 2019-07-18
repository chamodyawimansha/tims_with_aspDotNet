using System;
using System.ComponentModel.DataAnnotations;

namespace CECBTIMS.Models
{
    public class BaseCols
    {
        [Display(Name = "Created At")]
        public DateTime CreatedAt
        {
            get =>
                _createdAt ?? DateTime.Now;

            set => this._createdAt = value;
        }

        private DateTime? _createdAt = null;
        [Display(Name = "Updated On")]
        public DateTime? UpdatedAt { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;
    }
}