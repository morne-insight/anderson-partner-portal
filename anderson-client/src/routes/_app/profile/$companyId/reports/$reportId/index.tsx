import { createFileRoute, useRouter } from "@tanstack/react-router";
import { Button } from "@/components/ui/button";
import {
    ArrowLeft,
    Building2,
    Users,
    CheckCircle,
    Calendar,
    FileText,
    Loader2
} from "lucide-react";
import { QuarterlyReportDto, CountryDto, QuarterlyReportLineDto, QuarterlyReportPartnerDto } from "@/api/types.gen";
import { callApi } from "@/server/proxy";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { PartnerStatus } from "@/types/reports";


export const Route = createFileRoute("/_app/profile/$companyId/reports/$reportId/")({
    component: ViewReportPage,
    loader: async ({ params }) => {
        try {
            const response = await callApi({
                data: {
                    fn: 'getApiQuarterliesById',
                    args: {
                        path: {
                            id: params.reportId
                        }
                    }
                }
            });

            return { report: response };
        } catch (error) {
            console.error('Failed to fetch report:', error);
            throw new Error('Failed to load report');
        }
    },
});

function ViewReportPage() {
    const { companyId } = Route.useParams();
    const { report } = Route.useLoaderData();
    const router = useRouter();

    // Fetch country reference data
    const { countries, isLoading, isError } = usePrefetchReferenceData();

    const getPartnerStatusLabel = (status: number) => {
        switch (status) {
            case PartnerStatus.Hired:
                return 'Hired';
            case PartnerStatus.Promoted:
                return 'Promoted';
            case PartnerStatus.Terminated:
                return 'Terminated';
            default:
                return 'Unknown';
        }
    };

    const formatDate = (date: Date) => {
        return new Intl.DateTimeFormat('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
        }).format(new Date(date));
    };

    // Show loading state while reference data is loading
    if (isLoading) {
        return (
            <div className="flex min-h-screen items-center justify-center">
                <Loader2 className="h-8 w-8 animate-spin text-gray-300" />
            </div>
        );
    }

    // Ensure we have the data before proceeding
    if (isError) {
        return (
            <div className="flex min-h-screen items-center justify-center">
                <div className="text-center">
                    <p className="mb-4 text-red-600">Failed to load reference data</p>
                    <Button onClick={() => window.location.reload()}>Retry</Button>
                </div>
            </div>
        );
    }

    return (
        <div className="space-y-8 animate-fade-in pb-20">
            {/* Header */}
            <header className="border-b border-gray-200 pb-6">
                <div className="flex items-center justify-between">
                    <div>
                        <div className="flex items-center gap-3 mb-3">
                            <h2 className="text-4xl font-serif text-black">
                                Q{report.quarter} {report.year} Report
                            </h2>
                            {report.isSubmitted && (
                                <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-green-100 text-green-800">
                                    <CheckCircle className="w-4 h-4 mr-1" />
                                    Submitted
                                </span>
                            )}
                        </div>
                        <p className="text-gray-500 font-light text-lg">
                            {report.isSubmitted && report.submittedDate
                                ? `Submitted on ${formatDate(report.submittedDate)} • `
                                : ''}
                            Created {formatDate(report.createdDate!)}
                        </p>
                    </div>
                    <Button
                        variant="outline"
                        onClick={() => router.navigate({ to: `/profile/${companyId}/reports` })}
                    >
                        <ArrowLeft className="w-4 h-4 mr-2" />
                        Back to Reports
                    </Button>
                </div>
            </header>

            {/* Report Lines Section */}
            {report.reportLines && report.reportLines.length > 0 && (
                <div className="bg-white border border-gray-200 shadow-sm">
                    <div className="p-6 border-b border-gray-200">
                        <h3 className="text-lg font-bold uppercase tracking-widest flex items-center gap-2">
                            <Building2 className="w-5 h-5" />
                            Report Lines ({report.reportLines.length})
                        </h3>
                    </div>

                    <div className="p-6">
                        <div className="border border-gray-200 overflow-hidden">
                            <table className="w-full">
                                <thead className="bg-gray-100">
                                    <tr>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Country</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Partners</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Headcount</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Clients</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Offices</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Lawyers</th>
                                        <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Est. Revenue</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-gray-200">
                                    {report.reportLines.map((reportLine: QuarterlyReportLineDto, index: number) => {
                                        const country = countries?.data?.find((c: CountryDto) => c.id === reportLine.countryId);
                                        return (
                                            <tr key={index} className="bg-white hover:bg-gray-50">
                                                <td className="px-4 py-3 text-sm font-medium">{country?.name || 'Not specified'}</td>
                                                <td className="px-4 py-3 text-sm">{reportLine.partnerCount}</td>
                                                <td className="px-4 py-3 text-sm">{reportLine.headcount}</td>
                                                <td className="px-4 py-3 text-sm">{reportLine.clientCount}</td>
                                                <td className="px-4 py-3 text-sm">{reportLine.officeCount}</td>
                                                <td className="px-4 py-3 text-sm">{reportLine.lawyerCount}</td>
                                                <td className="px-4 py-3 text-sm">${reportLine.estimatedRevenue?.toLocaleString()}</td>
                                            </tr>
                                        );
                                    })}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            )}

            {/* Partners Section */}
            {report.partners && report.partners.length > 0 && (
                <div className="bg-white border border-gray-200 shadow-sm">
                    <div className="p-6 border-b border-gray-200">
                        <h3 className="text-lg font-bold uppercase tracking-widest flex items-center gap-2">
                            <Users className="w-5 h-5" />
                            Partners ({report.partners.length})
                        </h3>
                    </div>

                    <div className="p-6">
                        <table className="w-full border border-gray-200">
                            <thead className="bg-gray-100">
                                <tr>
                                    <th className="text-left p-4 text-xs font-medium text-gray-500 uppercase tracking-wider">Partner Name</th>
                                    <th className="text-left p-4 text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                {report.partners.map((partner: QuarterlyReportPartnerDto, index: number) => (
                                    <tr key={index} className="border-t border-gray-200 bg-gray-50">
                                        <td className="p-4">
                                            <p className="text-sm font-medium">{partner.name}</p>
                                        </td>
                                        <td className="p-4">
                                            <p className="text-sm font-medium">{getPartnerStatusLabel(partner.status || 1)}</p>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}

            {/* Report Metadata */}
            <div className="bg-gray-50 p-6 border border-gray-200">
                <div className="flex items-center justify-between">
                    <div className="flex items-center gap-6 text-sm text-gray-500">
                        <div className="flex items-center gap-2">
                            <Calendar className="w-4 h-4" />
                            <span>Created: {formatDate(report.createdDate!)}</span>
                        </div>
                        {report.isSubmitted && report.submittedDate && (
                            <div className="flex items-center gap-2">
                                <CheckCircle className="w-4 h-4" />
                                <span>Submitted: {formatDate(report.submittedDate)}</span>
                            </div>
                        )}
                        <div className="flex items-center gap-2">
                            <FileText className="w-4 h-4" />
                            <span>Q{report.quarter} {report.year} Report</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
