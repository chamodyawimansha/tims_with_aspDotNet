using System;
using System.ComponentModel.DataAnnotations;

namespace CECBTIMS.Models
{
    public class BaseCols
    {
        public DateTime CreatedAt
        {
            get =>
                _createdAt ?? DateTime.Now;

            set => this._createdAt = value;
        }

        private DateTime? _createdAt = null;

        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;
    }
}