using DevExpress.XtraReports.UI;
using Seed25.DTO;
using System;
using System.Collections.Generic;

namespace Seed25.Report
{
    public partial class TestReport : XtraReport
    {
        public TestReport()
        {
            InitializeComponent();  // controls + expression bindings exist now

            // Design-time sample so VS designer/preview shows values
            var sample = new InboundTicketDTO
            {
                UID = Guid.NewGuid(),
                Ticket = 123456,
                Weight = 2800,
                TimeIn = DateTime.Now,
                Plant = "123 Grain Rd.",
                Phone = "541-442-5555 ",
                Prompt1 = "Corn",
                Prompt2 = "Inbound"
            };

            this.DataSource = new List<InboundTicketDTO> { sample };
            // this.DataMember = ""; // optional
        }

        public TestReport(InboundTicketDTO data) : this()
        {
            // Overwrite with real runtime data
            this.DataSource = new List<InboundTicketDTO> { data };
            // this.DataMember = ""; // optional
        }
    }
}
