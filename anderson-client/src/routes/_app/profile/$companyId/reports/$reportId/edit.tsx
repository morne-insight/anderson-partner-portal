import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useForm } from "@tanstack/react-form";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import {
  Save,
  ArrowLeft,
  DollarSign,
  Users,
  Target,
  TrendingUp,
  FileText,
  Send
} from "lucide-react";
import { QuarterlyReport, UpdateQuarterlyReportCommand } from "@/types/reports";
import z from "zod";

export const Route = createFileRoute("/_app/profile/$companyId/reports/$reportId/edit")({
  component: EditReportPage,
  loader: async ({ params }) => {
    // TODO: Replace with actual API call when backend is implemented
    // const report = await callApi({ data: { fn: 'getApiCompaniesByIdReportsById', args: { path: { id: params.companyId, reportId: params.reportId } } } });

    // Mock data for now
    const mockReport: QuarterlyReport = {
      id: params.reportId,
      companyId: params.companyId,
      year: 2024,
      quarter: 'Q4',
      isSubmitted: false,
      createdDate: new Date('2024-12-01'),
      lastModifiedDate: new Date('2024-12-15'),
      revenue: 2500000,
      expenses: 2050000,
      netIncome: 450000,
      employeeCount: 45,
      newClients: 8,
      projectsCompleted: 12,
      keyAchievements: 'Successfully launched new AI consulting practice, secured three major enterprise clients, and expanded team by 15%.',
      challenges: 'Market volatility affected client decision-making timelines, and we faced increased competition in the data analytics space.',
      nextQuarterGoals: 'Focus on expanding AI practice, target 10 new clients, and launch new training programs for existing team members.',
      marketConditions: 'The consulting market showed signs of recovery with increased demand for digital transformation services.',
      competitivePosition: 'We maintain a strong position in the data analytics space but need to strengthen our AI capabilities to compete effectively.',
      additionalNotes: 'Considering strategic partnerships to enhance our AI offerings.'
    };

    return {
      report: mockReport,
    };
  },
});

function EditReportPage() {
  const { companyId, reportId } = Route.useParams();
  const { report } = Route.useLoaderData();
  const router = useRouter();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isSubmittingFinal, setIsSubmittingFinal] = useState(false);

  const form = useForm({
    defaultValues: {
      revenue: report.revenue,
      expenses: report.expenses,
      netIncome: report.netIncome,
      employeeCount: report.employeeCount,
      newClients: report.newClients,
      projectsCompleted: report.projectsCompleted,
      keyAchievements: report.keyAchievements,
      challenges: report.challenges,
      nextQuarterGoals: report.nextQuarterGoals,
      marketConditions: report.marketConditions,
      competitivePosition: report.competitivePosition,
      additionalNotes: report.additionalNotes || '',
    },
    validators: {
      onSubmit: ({ value }: { value: any }) => {
        const schema = z.object({
          revenue: z.number().min(0, "Revenue must be non-negative"),
          expenses: z.number().min(0, "Expenses must be non-negative"),
          netIncome: z.number(),
          employeeCount: z.number().min(0, "Employee count must be non-negative"),
          newClients: z.number().min(0, "New clients must be non-negative"),
          projectsCompleted: z.number().min(0, "Projects completed must be non-negative"),
          keyAchievements: z.string().min(1, "Key achievements are required"),
          challenges: z.string().min(1, "Challenges are required"),
          nextQuarterGoals: z.string().min(1, "Next quarter goals are required"),
          marketConditions: z.string().min(1, "Market conditions are required"),
          competitivePosition: z.string().min(1, "Competitive position is required"),
          additionalNotes: z.string().optional(),
        });

        const result = schema.safeParse(value);
        if (!result.success) {
          const errors: Record<string, string> = {};
          result.error.issues.forEach((issue) => {
            if (issue.path[0]) {
              errors[issue.path[0] as string] = issue.message;
            }
          });
          return errors;
        }
        return undefined;
      }
    },
    onSubmit: async ({ value }) => {
      setIsSubmitting(true);
      try {
        // TODO: Replace with actual API call when backend is implemented
        // const reportData: UpdateQuarterlyReportCommand = {
        //   id: reportId,
        //   companyId,
        //   year: report.year,
        //   quarter: report.quarter,
        //   ...value
        // };
        // await callApi({ data: { fn: 'putApiCompaniesByIdReportsById', args: { path: { id: companyId, reportId }, body: reportData } } });

        // Mock success for now
        await new Promise(resolve => setTimeout(resolve, 1000));

        alert("Report saved successfully!");
        router.navigate({ to: `/profile/${companyId}/reports` });
      } catch (error) {
        console.error(error);
        alert("Failed to save report.");
      } finally {
        setIsSubmitting(false);
      }
    },
  });

  const handleSubmitFinal = async () => {
    if (!form.state.canSubmit) {
      alert("Please fix all validation errors before submitting.");
      return;
    }

    if (!confirm("Are you sure you want to submit this report? Once submitted, it cannot be edited.")) {
      return;
    }

    setIsSubmittingFinal(true);
    try {
      // First save the current changes
      const formValues = form.state.values;

      // TODO: Replace with actual API calls when backend is implemented
      // const reportData: UpdateQuarterlyReportCommand = {
      //   id: reportId,
      //   companyId,
      //   year: report.year,
      //   quarter: report.quarter,
      //   ...formValues
      // };
      // await callApi({ data: { fn: 'putApiCompaniesByIdReportsById', args: { path: { id: companyId, reportId }, body: reportData } } });
      // await callApi({ data: { fn: 'postApiCompaniesByIdReportsByIdSubmit', args: { path: { id: companyId, reportId }, body: { id: reportId } } } });

      // Mock success for now
      await new Promise(resolve => setTimeout(resolve, 1500));

      alert("Report submitted successfully!");
      router.navigate({ to: `/profile/${companyId}/reports` });
    } catch (error) {
      console.error(error);
      alert("Failed to submit report.");
    } finally {
      setIsSubmittingFinal(false);
    }
  };

  // Auto-calculate net income when revenue or expenses change
  const handleRevenueChange = (value: number) => {
    form.setFieldValue('revenue', value);
    const expenses = form.getFieldValue('expenses') || 0;
    form.setFieldValue('netIncome', value - expenses);
  };

  const handleExpensesChange = (value: number) => {
    form.setFieldValue('expenses', value);
    const revenue = form.getFieldValue('revenue') || 0;
    form.setFieldValue('netIncome', revenue - value);
  };

  return (
    <div className="space-y-8 animate-fade-in pb-20">
      {/* Header */}
      <header className="border-b border-gray-200 pb-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-4xl font-serif text-black mb-3">
              Edit {report.quarter} {report.year} Report
            </h2>
            <p className="text-gray-500 font-light text-lg">
              Update your quarterly business report. Save as draft or submit for final review.
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

      {/* Form */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
        {/* Left Column: Financial Data */}
        <div className="space-y-8">
          <section className="space-y-6">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
              <DollarSign className="w-4 h-4" /> Financial Performance
            </h3>

            <div className="space-y-6">
              <form.Field
                name="revenue"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Total Revenue ($)</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      step="0.01"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => handleRevenueChange(parseFloat(e.target.value) || 0)}
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="expenses"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Total Expenses ($)</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      step="0.01"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => handleExpensesChange(parseFloat(e.target.value) || 0)}
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="netIncome"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Net Income ($)</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      step="0.01"
                      value={field.state.value}
                      readOnly
                      className="bg-gray-50"
                    />
                    <p className="text-xs text-gray-500">Automatically calculated as Revenue - Expenses</p>
                  </div>
                )}
              />
            </div>
          </section>

          <section className="space-y-6">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
              <Users className="w-4 h-4" /> Operational Metrics
            </h3>

            <div className="space-y-6">
              <form.Field
                name="employeeCount"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Total Employee Count</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(parseInt(e.target.value) || 0)}
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="newClients"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>New Clients Acquired</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(parseInt(e.target.value) || 0)}
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="projectsCompleted"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Projects Completed</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      type="number"
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(parseInt(e.target.value) || 0)}
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />
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
              <form.Field
                name="keyAchievements"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Key Achievements</Label>
                    <Textarea
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      className="min-h-[100px]"
                      placeholder="Describe the major accomplishments and milestones achieved this quarter..."
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="challenges"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Challenges Faced</Label>
                    <Textarea
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      className="min-h-[100px]"
                      placeholder="Outline the main challenges and obstacles encountered this quarter..."
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="nextQuarterGoals"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Next Quarter Goals</Label>
                    <Textarea
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      className="min-h-[100px]"
                      placeholder="Define the key objectives and targets for the upcoming quarter..."
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />
            </div>
          </section>

          <section className="space-y-6">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
              <TrendingUp className="w-4 h-4" /> Market Analysis
            </h3>

            <div className="space-y-6">
              <form.Field
                name="marketConditions"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Market Conditions</Label>
                    <Textarea
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      className="min-h-[100px]"
                      placeholder="Analyze the current market conditions and trends affecting your business..."
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="competitivePosition"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Competitive Position</Label>
                    <Textarea
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      className="min-h-[100px]"
                      placeholder="Assess your firm's position relative to competitors and market dynamics..."
                    />
                    {field.state.meta.errors && (
                      <p className="text-red-500 text-xs">{field.state.meta.errors}</p>
                    )}
                  </div>
                )}
              />

              <form.Field
                name="additionalNotes"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Additional Notes (Optional)</Label>
                    <Textarea
                      id={field.name}
                      name={field.name}
                      value={field.state.value}
                      onBlur={field.handleBlur}
                      onChange={(e) => field.handleChange(e.target.value)}
                      className="min-h-[80px]"
                      placeholder="Any additional information or context for this quarter..."
                    />
                  </div>
                )}
              />
            </div>
          </section>
        </div>
      </div>

      {/* Footer Actions */}
      <div className="fixed bottom-0 left-0 md:left-72 right-0 p-6 bg-white border-t border-gray-200 flex justify-between items-center z-40 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]">
        <span className="text-xs text-gray-400 font-medium uppercase tracking-widest hidden md:inline-block">
          {report.quarter} {report.year} Report • Draft
        </span>
        <div className="flex items-center gap-4 w-full md:w-auto justify-end">
          <Button
            variant="outline"
            onClick={() => router.navigate({ to: `/profile/${companyId}/reports` })}
          >
            Cancel
          </Button>
          <form.Subscribe
            selector={(state) => [state.canSubmit, state.isSubmitting]}
            children={([canSubmit, isSubmitting]) => (
              <>
                <Button
                  onClick={form.handleSubmit}
                  disabled={!canSubmit || isSubmitting}
                  variant="outline"
                  className="min-w-[120px] uppercase font-bold tracking-widest text-xs"
                >
                  {isSubmitting ? (
                    <>Saving...</>
                  ) : (
                    <>
                      <Save className="w-3 h-3 mr-2" />
                      Save Draft
                    </>
                  )}
                </Button>
                <Button
                  onClick={handleSubmitFinal}
                  disabled={!canSubmit || isSubmittingFinal}
                  className="bg-green-600 hover:bg-green-700 min-w-[140px] uppercase font-bold tracking-widest text-xs"
                >
                  {isSubmittingFinal ? (
                    <>Submitting...</>
                  ) : (
                    <>
                      <Send className="w-3 h-3 mr-2" />
                      Submit Final
                    </>
                  )}
                </Button>
              </>
            )}
          />
        </div>
      </div>
    </div>
  );
}
