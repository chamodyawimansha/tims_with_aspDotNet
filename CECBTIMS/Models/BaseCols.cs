using System;
using System.ComponentModel.DataAnnotations;

namespace CECBTIMS.Models
{
    public class BaseCols
    {
        public DateTime CreatedAt
        {
            get
            {
                return this.createdAt.HasValue
                    ? this.createdAt.Value
                    : DateTime.Now;
            }

            set { this.createdAt = value; }
        }

        private DateTime? createdAt = null;

        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } // For optimistic concurrency;
    }
}