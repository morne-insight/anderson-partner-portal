import { createFileRoute, useRouter } from "@tanstack/react-router";
import { Button } from "@/components/ui/button";
import {
    ArrowLeft,
    DollarSign,
    Users,
    Target,
    TrendingUp,
    CheckCircle,
    Calendar,
    FileText
} from "lucide-react";
import { QuarterlyReport } from "@/types/reports";

export const Route = createFileRoute("/_app/profile/$companyId/reports/$reportId/")({
    component: ViewReportPage,
    loader: async ({ params }) => {
        // TODO: Replace with actual API call when backend is implemented
        // const report = await callApi({ data: { fn: 'getApiCompaniesByIdReportsById', args: { path: { id: params.companyId, reportId: params.reportId } } } });

        // Mock data for now
        const mockReport: QuarterlyReport = {
            id: params.reportId,
            companyId: params.companyId,
            year: 2024,
            quarter: 3,
            isSubmitted: true,
            createdDate: new Date('2024-09-01'),
            submittedDate: new Date('2024-10-15'),
            lastModifiedDate: new Date('2024-10-15'),
            revenue: 2200000,
            expenses: 1820000,
            netIncome: 380000,
            employeeCount: 42,
            newClients: 6,
            projectsCompleted: 15,
            keyAchievements: 'Successfully completed major digital transformation project for Fortune 500 client, expanded our AI consulting capabilities, and achieved 15% revenue growth compared to Q2.',
            challenges: 'Faced increased competition in the market and some delays in project deliveries due to client resource constraints. Also experienced challenges in recruiting senior talent.',
            nextQuarterGoals: 'Focus on expanding our market presence in the financial services sector, launch new AI-powered analytics products, and strengthen our partnership network.',
            marketConditions: 'The consulting market showed strong demand for digital transformation services, particularly in AI and data analytics. However, economic uncertainty led to longer sales cycles.',
            competitivePosition: 'We maintain a strong competitive position in data analytics but need to accelerate our AI capabilities development to stay ahead of emerging competitors.',
            additionalNotes: 'Considering strategic acquisition opportunities to enhance our AI expertise and expand geographical reach.',
            reports: [
                {
                    id: 'report-1',
                    partnerCount: 8,
                    headcount: 42,
                    clientCount: 25,
                    officeCount: 2,
                    lawyerCount: 15,
                    estimatedRevenue: 2200000
                }
            ],
            partners: [
                {
                    id: 'partner-1',
                    name: 'John Smith',
                    status: 1
                },
                {
                    id: 'partner-2',
                    name: 'Sarah Johnson',
                    status: 1
                }
            ]
        };

        return {
            report: mockReport,
        };
    },
});

function ViewReportPage() {
    const { companyId } = Route.useParams();
    const { report } = Route.useLoaderData();
    const router = useRouter();

    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
            minimumFractionDigits: 0,
            maximumFractionDigits: 0,
        }).format(amount);
    };

    const formatDate = (date: Date) => {
        return new Intl.DateTimeFormat('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
        }).format(new Date(date));
    };

    return (
        <div className="space-y-8 animate-fade-in pb-20">
            {/* Header */}
            <header className="border-b border-gray-200 pb-6">
                <div className="flex items-center justify-between">
                    <div>
                        <div className="flex items-center gap-3 mb-3">
                            <h2 className="text-4xl font-serif text-black">
                                {report.quarter} {report.year} Report
                            </h2>
                            <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-green-100 text-green-800">
                                <CheckCircle className="w-4 h-4 mr-1" />
                                Submitted
                            </span>
                        </div>
                        <p className="text-gray-500 font-light text-lg">
                            Submitted on {formatDate(report.submittedDate!)} • Created {formatDate(report.createdDate)}
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

            {/* Report Content */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
                {/* Left Column: Financial Data */}
                <div className="space-y-8">
                    <section className="space-y-6">
                        <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
                            <DollarSign className="w-4 h-4" /> Financial Performance
                        </h3>

                        <div className="bg-white p-6 border border-gray-200 shadow-sm space-y-6">
                            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                                <div className="text-center">
                                    <div className="text-2xl font-bold text-green-600 mb-1">
                                        {formatCurrency(report.revenue)}
                                    </div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wide">Total Revenue</div>
                                </div>
                                <div className="text-center">
                                    <div className="text-2xl font-bold text-red-600 mb-1">
                                        {formatCurrency(report.expenses)}
                                    </div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wide">Total Expenses</div>
                                </div>
                                <div className="text-center">
                                    <div className={`text-2xl font-bold mb-1 ${report.netIncome >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                                        {formatCurrency(report.netIncome)}
                                    </div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wide">Net Income</div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section className="space-y-6">
                        <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
                            <Users className="w-4 h-4" /> Operational Metrics
                        </h3>

                        <div className="bg-white p-6 border border-gray-200 shadow-sm space-y-6">
                            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                                <div className="text-center">
                                    <div className="text-2xl font-bold text-blue-600 mb-1">
                                        {report.employeeCount}
                                    </div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wide">Total Employees</div>
                                </div>
                                <div className="text-center">
                                    <div className="text-2xl font-bold text-purple-600 mb-1">
                                        {report.newClients}
                                    </div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wide">New Clients</div>
                                </div>
                                <div className="text-center">
                                    <div className="text-2xl font-bold text-orange-600 mb-1">
                                        {report.projectsCompleted}
                                    </div>
                                    <div className="text-xs text-gray-500 uppercase tracking-wide">Projects Completed</div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>

                {/* Right Column: Strategic Information */}
                <div className="space-y-8">
                    <section className="space-y-6">
                        <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
                            <Target className="w-4 h-4" /> Strategic Information
                        </h3>

                        <div className="space-y-6">
                            <div className="bg-white p-6 border border-gray-200 shadow-sm">
                                <h4 className="font-bold text-sm text-gray-900 mb-3 uppercase tracking-wide">Key Achievements</h4>
                                <p className="text-gray-700 text-sm leading-relaxed">{report.keyAchievements}</p>
                            </div>

                            <div className="bg-white p-6 border border-gray-200 shadow-sm">
                                <h4 className="font-bold text-sm text-gray-900 mb-3 uppercase tracking-wide">Challenges Faced</h4>
                                <p className="text-gray-700 text-sm leading-relaxed">{report.challenges}</p>
                            </div>

                            <div className="bg-white p-6 border border-gray-200 shadow-sm">
                                <h4 className="font-bold text-sm text-gray-900 mb-3 uppercase tracking-wide">Next Quarter Goals</h4>
                                <p className="text-gray-700 text-sm leading-relaxed">{report.nextQuarterGoals}</p>
                            </div>
                        </div>
                    </section>

                    <section className="space-y-6">
                        <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
                            <TrendingUp className="w-4 h-4" /> Market Analysis
                        </h3>

                        <div className="space-y-6">
                            <div className="bg-white p-6 border border-gray-200 shadow-sm">
                                <h4 className="font-bold text-sm text-gray-900 mb-3 uppercase tracking-wide">Market Conditions</h4>
                                <p className="text-gray-700 text-sm leading-relaxed">{report.marketConditions}</p>
                            </div>

                            <div className="bg-white p-6 border border-gray-200 shadow-sm">
                                <h4 className="font-bold text-sm text-gray-900 mb-3 uppercase tracking-wide">Competitive Position</h4>
                                <p className="text-gray-700 text-sm leading-relaxed">{report.competitivePosition}</p>
                            </div>

                            {report.additionalNotes && (
                                <div className="bg-white p-6 border border-gray-200 shadow-sm">
                                    <h4 className="font-bold text-sm text-gray-900 mb-3 uppercase tracking-wide">Additional Notes</h4>
                                    <p className="text-gray-700 text-sm leading-relaxed">{report.additionalNotes}</p>
                                </div>
                            )}
                        </div>
                    </section>
                </div>
            </div>

            {/* Report Metadata */}
            <div className="bg-gray-50 p-6 border border-gray-200">
                <div className="flex items-center justify-between">
                    <div className="flex items-center gap-6 text-sm text-gray-500">
                        <div className="flex items-center gap-2">
                            <Calendar className="w-4 h-4" />
                            <span>Created: {formatDate(report.createdDate)}</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <CheckCircle className="w-4 h-4" />
                            <span>Submitted: {formatDate(report.submittedDate!)}</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <FileText className="w-4 h-4" />
                            <span>Report ID: {report.id}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
