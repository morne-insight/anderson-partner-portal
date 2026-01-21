using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.AddReportLineQuarterly
{
    public class AddReportLineQuarterlyCommand : IRequest, ICommand
    {
        public AddReportLineQuarterlyCommand(Guid id,
            Guid quarterlyId,
            int partnerCount,
            int headcount,
            int clientCount,
            int officeCount,
            int lawyerCount,
            double estimatedRevenue)
        {
            Id = id;
            QuarterlyId = quarterlyId;
            PartnerCount = partnerCount;
            Headcount = headcount;
            ClientCount = clientCount;
            OfficeCount = officeCount;
            LawyerCount = lawyerCount;
            EstimatedRevenue = estimatedRevenue;
        }

        public Guid Id { get; set; }
        public Guid QuarterlyId { get; set; }
        public int PartnerCount { get; set; }
        public int Headcount { get; set; }
        public int ClientCount { get; set; }
        public int OfficeCount { get; set; }
        public int LawyerCount { get; set; }
        public double EstimatedRevenue { get; set; }
    }
}