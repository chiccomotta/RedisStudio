using System;

#nullable disable

namespace RedisStudio.Models
{
    public partial class FooClass
    {
        public int BPDossierConfigId { get; set; }
        public int BPId { get; set; }
        public string CAT { get; set; }
        public string CATDescription { get; set; }
        public string CATDetail { get; set; }
        public string MP { get; set; }
        public string MPDescription { get; set; }
        public string OA { get; set; }
        public string DefaultEvent { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool Enabled { get; set; }
        public int? ServiceTypeId { get; set; }
    }
}
