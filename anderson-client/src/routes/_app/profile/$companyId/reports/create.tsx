import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import {
  Save,
  ArrowLeft,
  Plus,
  X,
  Users,
  Building2,
  Loader2
} from "lucide-react";
import {
  type CreateQuarterlyCommand,
  type CreateQuarterlyReportsDto,
  type CreateQuarterlyPartnersDto,
  type ReportQuarter,
  type CountryDto
} from "@/api";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { PartnerStatus } from "@/types/reports";
import { callApi } from "@/server/proxy";

interface CreateReportSearch {
  year: string;
  quarter: string;
}

export const Route = createFileRoute("/_app/profile/$companyId/reports/create")({
  component: CreateReportPage,
  validateSearch: (search: Record<string, unknown>): CreateReportSearch => ({
    year: (search.year as string) || '',
    quarter: (search.quarter as string) || '',
  }),
});

function CreateReportPage() {
  const { companyId } = Route.useParams();
  const { year, quarter } = Route.useSearch();
  const router = useRouter();

  // State for report lines management
  const [reportLines, setReportLines] = useState<CreateQuarterlyReportsDto[]>([]);
  const [isAddingReportLine, setIsAddingReportLine] = useState(false);
  const [newReportLine, setNewReportLine] = useState<CreateQuarterlyReportsDto>({
    partnerCount: 0,
    headcount: 0,
    clientCount: 0,
    officeCount: 0,
    lawyerCount: 0,
    estimatedRevenue: 0,
    countryId: '',
  });

  // State for partners management
  const [partners, setPartners] = useState<CreateQuarterlyPartnersDto[]>([]);
  const [isAddingPartner, setIsAddingPartner] = useState(false);
  const [newPartner, setNewPartner] = useState<CreateQuarterlyPartnersDto>({
    name: '',
    status: 1, // Default status
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const { countries, isLoading, isError } = usePrefetchReferenceData();

  // Helper function to get status label
  const getPartnerStatusLabel = (status: number): string => {
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

  // Show loading state while reference data is loading  
  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin text-gray-300" />
      </div>
    );
  }

  // Ensure we have the data before proceeding
  if (isError) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <p className="text-red-600 mb-4">Failed to load reference data</p>
          <Button onClick={() => window.location.reload()}>Retry</Button>
        </div>
      </div>
    );
  }

  const handleAddReportLine = () => {
    if (newReportLine.partnerCount && newReportLine.headcount && newReportLine.estimatedRevenue) {
      setReportLines([...reportLines, { ...newReportLine }]);
      setNewReportLine({
        partnerCount: 0,
        headcount: 0,
        clientCount: 0,
        officeCount: 0,
        lawyerCount: 0,
        estimatedRevenue: 0,
        countryId: '',
      });
      setIsAddingReportLine(false);
    }
  };

  const handleRemoveReportLine = (index: number) => {
    setReportLines(reportLines.filter((_, i) => i !== index));
  };

  const handleAddPartner = () => {
    if (newPartner.name?.trim()) {
      setPartners([...partners, { ...newPartner }]);
      setNewPartner({
        name: '',
        status: 1,
      });
      setIsAddingPartner(false);
    }
  };

  const handleRemovePartner = (index: number) => {
    setPartners(partners.filter((_, i) => i !== index));
  };

  const handleSubmit = async () => {
    if (reportLines.length === 0) {
      alert('Please add at least one report line.');
      return;
    }

    if (partners.length === 0) {
      alert('Please add at least one partner.');
      return;
    }

    setIsSubmitting(true);
    try {
      const createCommand: CreateQuarterlyCommand = {
        year: parseInt(year),
        quarter: parseInt(quarter) as ReportQuarter,
        companyId,
        partners,
        reports: reportLines,
      };

      await callApi({
        data: {
          fn: "postApiQuarterlies",
          args: {
            body: createCommand,
          },
        },
      });

      alert("Report created successfully!");
      router.navigate({ to: `/profile/${companyId}/reports` });
    } catch (error) {
      console.error(error);
      alert("Failed to create report.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="space-y-8 animate-fade-in pb-20">
      {/* Header */}
      <header className="border-b border-gray-200 pb-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-4xl font-serif text-black mb-3">
              Create Q{quarter} {year} Report
            </h2>
            <p className="text-gray-500 font-light text-lg">
              Add report lines and partners to create your quarterly business report.
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
      <div className="bg-white border border-gray-200 shadow-sm">
        <div className="p-6 border-b border-gray-200">
          <h3 className="text-lg font-bold uppercase tracking-widest flex items-center gap-2">
            <Building2 className="w-5 h-5" />
            Report Lines ({reportLines.length})
          </h3>
        </div>

        <div className="p-6 space-y-4">
          {reportLines.length > 0 && (
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
                    <th className="px-4 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {reportLines.map((reportLine, index) => {
                    const country = countries?.data?.find(c => c.id === reportLine.countryId);
                    return (
                      <tr key={index} className="bg-white hover:bg-gray-50">
                        <td className="px-4 py-3 text-sm font-medium">{country?.name || 'Not specified'}</td>
                        <td className="px-4 py-3 text-sm">{reportLine.partnerCount}</td>
                        <td className="px-4 py-3 text-sm">{reportLine.headcount}</td>
                        <td className="px-4 py-3 text-sm">{reportLine.clientCount}</td>
                        <td className="px-4 py-3 text-sm">{reportLine.officeCount}</td>
                        <td className="px-4 py-3 text-sm">{reportLine.lawyerCount}</td>
                        <td className="px-4 py-3 text-sm">${reportLine.estimatedRevenue?.toLocaleString()}</td>
                        <td className="px-4 py-3 text-right">
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => handleRemoveReportLine(index)}
                            className="text-red-600 hover:text-red-700 hover:bg-red-50"
                          >
                            <X className="w-4 h-4" />
                          </Button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}

          {isAddingReportLine ? (
            <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Country</Label>
                  <Select
                    value={newReportLine.countryId || ''}
                    onValueChange={(value) => setNewReportLine({ ...newReportLine, countryId: value })}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select Country" />
                    </SelectTrigger>
                    <SelectContent>
                      {countries?.data?.map((country: CountryDto) => (
                        <SelectItem key={country.id} value={country.id || ''}>
                          {country.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label>Partner Count</Label>
                  <Input
                    type="number"
                    value={newReportLine.partnerCount || ''}
                    onChange={(e) => setNewReportLine({ ...newReportLine, partnerCount: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Client Count</Label>
                  <Input
                    type="number"
                    value={newReportLine.clientCount || ''}
                    onChange={(e) => setNewReportLine({ ...newReportLine, clientCount: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Office Count</Label>
                  <Input
                    type="number"
                    value={newReportLine.officeCount || ''}
                    onChange={(e) => setNewReportLine({ ...newReportLine, officeCount: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Headcount</Label>
                  <Input
                    type="number"
                    value={newReportLine.headcount || ''}
                    onChange={(e) => setNewReportLine({ ...newReportLine, headcount: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Lawyer Count</Label>
                  <Input
                    type="number"
                    value={newReportLine.lawyerCount || ''}
                    onChange={(e) => setNewReportLine({ ...newReportLine, lawyerCount: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Estimated Revenue ($)</Label>
                  <Input
                    type="number"
                    value={newReportLine.estimatedRevenue || ''}
                    onChange={(e) => setNewReportLine({ ...newReportLine, estimatedRevenue: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                  />
                </div>
              </div>
              <div className="flex justify-end gap-2 pt-2">
                <Button
                  onClick={() => setIsAddingReportLine(false)}
                  size="sm"
                  variant="outline"
                >
                  Cancel
                </Button>
                <Button
                  className="bg-black hover:bg-gray-800"
                  disabled={!newReportLine.partnerCount || !newReportLine.headcount || !newReportLine.estimatedRevenue}
                  onClick={handleAddReportLine}
                  size="sm"
                >
                  Add Report Line
                </Button>
              </div>
            </div>
          ) : (
            <Button
              className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
              onClick={() => setIsAddingReportLine(true)}
              variant="outline"
            >
              <Plus className="w-4 h-4 mr-2" />
              Add Report Line
            </Button>
          )}
        </div>
      </div>

      {/* Partners Section */}
      <div className="bg-white border border-gray-200 shadow-sm">
        <div className="p-6 border-b border-gray-200">
          <h3 className="text-lg font-bold uppercase tracking-widest flex items-center gap-2">
            <Users className="w-5 h-5" />
            Partners ({partners.length})
          </h3>
        </div>

        <div className="p-6 space-y-4">
          {partners.length > 0 && (
            <table className="w-full border border-gray-200">
              <thead className="bg-gray-100">
                <tr>
                  <th className="text-left p-4 text-xs font-medium text-gray-500 uppercase tracking-wider">Partner Name</th>
                  <th className="text-left p-4 text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                  <th className="w-16"></th>
                </tr>
              </thead>
              <tbody>
                {partners.map((partner, index) => (
                  <tr key={index} className="border-t border-gray-200 bg-gray-50">
                    <td className="p-4">
                      <p className="text-sm font-medium">{partner.name}</p>
                    </td>
                    <td className="p-4">
                      <p className="text-sm font-medium">{getPartnerStatusLabel(partner.status || 1)}</p>
                    </td>
                    <td className="p-4">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleRemovePartner(index)}
                        className="text-red-600 hover:text-red-700 hover:bg-red-50"
                      >
                        <X className="w-4 h-4" />
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}

          {isAddingPartner ? (
            <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Partner Name</Label>
                  <Input
                    value={newPartner.name || ''}
                    onChange={(e) => setNewPartner({ ...newPartner, name: e.target.value })}
                    placeholder="e.g. John Smith"
                  />
                </div>
                <div className="space-y-2">
                  <Label>Status</Label>
                  <Select
                    value={newPartner.status?.toString() || ''}
                    onValueChange={(value) => setNewPartner({ ...newPartner, status: parseInt(value) })}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select Status" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value={PartnerStatus.Hired.toString()}>Hired</SelectItem>
                      <SelectItem value={PartnerStatus.Promoted.toString()}>Promoted</SelectItem>
                      <SelectItem value={PartnerStatus.Terminated.toString()}>Terminated</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
              <div className="flex justify-end gap-2 pt-2">
                <Button
                  onClick={() => setIsAddingPartner(false)}
                  size="sm"
                  variant="outline"
                >
                  Cancel
                </Button>
                <Button
                  className="bg-black hover:bg-gray-800"
                  disabled={!newPartner.name?.trim()}
                  onClick={handleAddPartner}
                  size="sm"
                >
                  Add Partner
                </Button>
              </div>
            </div>
          ) : (
            <Button
              className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
              onClick={() => setIsAddingPartner(true)}
              variant="outline"
            >
              <Plus className="w-4 h-4 mr-2" />
              Add Partner
            </Button>
          )}
        </div>
      </div>

      {/* Footer Actions */}
      <div className="fixed bottom-0 left-0 md:left-72 right-0 p-6 bg-white border-t border-gray-200 flex justify-between items-center z-40 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]">
        <span className="text-xs text-gray-400 font-medium uppercase tracking-widest hidden md:inline-block">
          Q{quarter} {year} Report
        </span>
        <div className="flex items-center gap-4 w-full md:w-auto justify-end">
          <Button
            variant="outline"
            onClick={() => router.navigate({ to: `/profile/${companyId}/reports` })}
          >
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={isSubmitting || reportLines.length === 0 || partners.length === 0}
            className="bg-red-600 hover:bg-red-700 min-w-[160px] uppercase font-bold tracking-widest text-xs"
          >
            {isSubmitting ? (
              <>Creating...</>
            ) : (
              <>
                <Save className="w-3 h-3 mr-2" />
                Create Report
              </>
            )}
          </Button>
        </div>
      </div>
    </div>
  );
}
