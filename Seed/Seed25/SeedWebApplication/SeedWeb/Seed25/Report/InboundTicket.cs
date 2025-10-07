using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraReports.UI;
using Seed25.DTO;
using System.Collections.Generic;

namespace Seed25.Report
{
    public partial class InboundTicket : XtraReport
    {
        public InboundTicket()
        {
            InitializeComponent();
            this.DataSource = new List<InboundTicketDTO>();
            this.DataMember = ""; // optional
        }

        // Bind a single row
        public InboundTicket(InboundTicketDTO data) : this()
        {
            // Overwrite with real runtime data
            this.DataSource = new List<InboundTicketDTO> { data };
            this.DataMember = ""; // optional
        }

      
    }
}
