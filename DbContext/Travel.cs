using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RedisStudio.DbContext;

public partial class Travel
{
    [DisplayName("ID_TRAVEL")]
    public int TravelId { get; set; }

    public string DossierCode { get; set; }

    public int QAIncidentId { get; set; }

    public string City { get; set; }

    public string Nation { get; set; }

    public double? Latitudine { get; set; }

    public double? Longitudine { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [DisplayName("TIPO_TRASPORTO")]
    public int? TransportType { get; set; }

    public DateTime? InsertDate { get; set; }

    public string InsertBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }

    public bool Enabled { get; set; }
}
