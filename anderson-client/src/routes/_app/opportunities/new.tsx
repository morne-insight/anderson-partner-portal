import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { toast } from "sonner";
import { useForm } from "@tanstack/react-form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import {
  Loader2,
  ArrowLeft,
  Building,
  ChevronDown
} from "lucide-react";
import { callApi } from "@/server/proxy";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";

export const Route = createFileRoute("/_app/opportunities/new")({
  component: CreateOpportunity,
  loader: async () => {
    const [companies,] = await Promise.all([
      callApi({ data: { fn: 'getApiCompaniesMe' } }),
    ]);

    return {
      companies: companies || []
    };
  },
});

function CreateOpportunity() {
  const router = useRouter();
  const { companies } = Route.useLoaderData();

  const {
    opportunityTypes,
    countries,
    serviceTypes,
    capabilities,
    industries,
    isLoading: isLoadingReferenceData,
    isError: isErrorReferenceData
  } = usePrefetchReferenceData();

  const [datePickerOpen, setDatePickerOpen] = useState(false);

  const createMutation = useMutation({
    mutationFn: async (values: any) => {
      const response = await callApi({ data: { fn: 'postApiOpportunities', args: { body: values } } });
      if (response.error) {
        throw response.error;
      }
      return response;
    },
    onSuccess: () => {
      router.navigate({ to: "/opportunities" });
    },
    onError: (err) => {
      console.error("Failed to create opportunity", err);
      toast.error("Failed to create opportunity. Please try again.");
    }
  });

  // Show loading state while reference data is loading
  if (isLoadingReferenceData) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin text-gray-300" />
      </div>
    );
  }

  // Show error state if reference data failed to load
  if (isErrorReferenceData) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <p className="text-red-600 mb-4">Failed to load reference data</p>
          <Button onClick={() => window.location.reload()}>Retry</Button>
        </div>
      </div>
    );
  }

  const opportunityTypesData = opportunityTypes.data || [];
  const countriesData = countries.data || [];
  const serviceTypesData = serviceTypes.data || [];
  const capabilitiesData = capabilities.data || [];
  const industriesData = industries.data || [];

  const form = useForm({
    defaultValues: {
      title: "",
      shortDescription: "",
      fullDescription: "",
      deadline: null as Date | null,
      opportunityTypeId: "",
      countryId: "",
      companyId: companies.length === 1 ? (companies[0]?.id || "") : "",
      serviceTypes: [] as string[],
      capabilities: [] as string[],
      industries: [] as string[],
    },
    validators: {
      onSubmit: ({ value }) => {
        console.log('Form validation values:', {
          title: value.title,
          shortDescription: value.shortDescription,
          opportunityTypeId: value.opportunityTypeId,
          countryId: value.countryId,
          companyId: value.companyId,
          deadline: value.deadline,
        });
        const errors: any = {};
        if (!value.title?.trim()) errors.title = "Title is required";
        if (!value.shortDescription?.trim()) errors.shortDescription = "Short description is required";
        if (!value.opportunityTypeId) errors.opportunityTypeId = "Type is required";
        if (!value.countryId) errors.countryId = "Country is required";
        if (!value.companyId) errors.companyId = "Company is required";

        console.log('Validation errors:', errors);
        return Object.keys(errors).length > 0 ? errors : undefined;
      }
    },
    onSubmit: async ({ value }) => {
      const payload = {
        ...value,
        deadline: value.deadline ? new Date(value.deadline.getTime() - (value.deadline.getTimezoneOffset() * 60000)).toISOString().split('T')[0] : null
      };
      await createMutation.mutateAsync(payload);
    },
  });

  return (
    <div className="space-y-8 animate-fade-in pb-20">
      {/* Header */}
      <header className="border-b border-gray-200 pb-6">
        <button
          onClick={() => router.history.back()}
          className="flex items-center gap-2 text-gray-500 hover:text-black text-xs uppercase tracking-widest font-bold mb-4 transition-colors"
        >
          <ArrowLeft className="w-4 h-4" /> Back to Opportunities
        </button>
        <h2 className="text-4xl font-serif text-black mb-3">Post New Opportunity</h2>
        <p className="text-gray-500 font-light text-lg">
          Share a tender, joint venture, or resource request with the Anderson network.
        </p>
      </header>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }}
        className="grid grid-cols-1 lg:grid-cols-3 gap-12"
      >
        {/* Left Column: Main Form */}
        <div className="lg:col-span-2 space-y-8">

          {/* Company Selection */}
          <section className="space-y-4">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
              <Building className="w-4 h-4" /> Posting Company
            </h3>

            {companies.length === 0 ? (
              <div className="p-6 bg-red-50 border border-red-200 text-red-700 text-sm">
                You are not linked to any companies. Please create a company profile first.
              </div>
            ) : companies.length === 1 ? (
              <div className="p-4 bg-gray-50 border border-gray-200">
                <p className="font-bold text-gray-900">{companies[0]?.name}</p>
                <p className="text-xs text-gray-500 uppercase tracking-wide mt-1">
                  Posting as this company
                </p>
              </div>
            ) : (
              <form.Field
                name="companyId"
                children={(field) => (
                  <div className="space-y-2">
                    <Label>Select Company</Label>
                    <Select value={field.state.value} onValueChange={field.handleChange}>
                      <SelectTrigger>
                        <SelectValue placeholder="Select a company" />
                      </SelectTrigger>
                      <SelectContent>
                        {companies.map((company: any) => (
                          <SelectItem key={company.id} value={company.id!}>
                            {company.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
              />
            )}
          </section>

          {/* Basic Details */}
          <section className="space-y-4">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2">
              Opportunity Details
            </h3>

            <form.Field
              name="title"
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor="title">Title *</Label>
                  <Input
                    id="title"
                    value={field.state.value}
                    onBlur={field.handleBlur}
                    onChange={(e) => field.handleChange(e.target.value)}
                    placeholder="e.g. Strategic Partnership for EMEA Market Entry"
                  />
                </div>
              )}
            />

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <form.Field
                name="opportunityTypeId"
                children={(field) => (
                  <div className="space-y-2">
                    <Label>Opportunity Type *</Label>
                    <Select value={field.state.value} onValueChange={field.handleChange}>
                      <SelectTrigger>
                        <SelectValue placeholder="Select type" />
                      </SelectTrigger>
                      <SelectContent>
                        {opportunityTypesData.map((type: any) => (
                          <SelectItem key={type.id} value={type.id!}>
                            {type.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
              />

              <form.Field
                name="countryId"
                children={(field) => (
                  <div className="space-y-2">
                    <Label>Country *</Label>
                    <Select value={field.state.value} onValueChange={field.handleChange}>
                      <SelectTrigger>
                        <SelectValue placeholder="Select country" />
                      </SelectTrigger>
                      <SelectContent>
                        {countriesData.map((country: any) => (
                          <SelectItem key={country.id} value={country.id!}>
                            {country.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
              />
            </div>

            <form.Field
              name="deadline"
              mode="value"
              children={(field) => {
                console.log('Deadline field state:', field.state.value);
                return (
                  <div className="space-y-2">
                    <Label>Deadline</Label>
                    <Popover open={datePickerOpen} onOpenChange={setDatePickerOpen}>
                      <PopoverTrigger asChild>
                        <Button
                          variant="outline"
                          className="w-full md:w-64 justify-between font-normal"
                        >
                          {field.state.value ? field.state.value.toLocaleDateString() : "Select deadline"}
                          <ChevronDown className="w-4 h-4" />
                        </Button>
                      </PopoverTrigger>
                      <PopoverContent className="w-auto overflow-hidden p-0" align="start">
                        <Calendar
                          mode="single"
                          selected={field.state.value ?? undefined}
                          captionLayout="dropdown"
                          onSelect={(date) => {
                            console.log('Deadline selected:', date, 'Field before change:', field.state.value);
                            field.setValue(date || null);
                            console.log('Using field.setValue method');
                            setDatePickerOpen(false);
                          }}
                        />
                      </PopoverContent>
                    </Popover>
                  </div>
                );
              }}
            />

            <form.Field
              name="shortDescription"
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor="shortDescription">Short Description *</Label>
                  <Textarea
                    id="shortDescription"
                    value={field.state.value}
                    onBlur={field.handleBlur}
                    onChange={(e) => field.handleChange(e.target.value)}
                    placeholder="Brief summary of the opportunity (displayed in listings)"
                  />
                  <p className="text-[10px] text-gray-500 uppercase tracking-wide">Max 500 characters</p>
                </div>
              )}
            />

            <form.Field
              name="fullDescription"
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor="fullDescription">Full Description</Label>
                  <Textarea
                    id="fullDescription"
                    value={field.state.value}
                    onBlur={field.handleBlur}
                    onChange={(e) => field.handleChange(e.target.value)}
                    placeholder="Detailed description of the opportunity, requirements, and expectations"
                    className="min-h-[150px]"
                  />
                </div>
              )}
            />
          </section>
        </div>

        {/* Right Column: Tags & Metadata */}
        <div className="space-y-8">
          {/* Service Types */}
          <section className="space-y-4">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2">
              Service Types
            </h3>
            <div className="p-4 bg-white border border-gray-200 shadow-sm">
              <form.Field
                name="serviceTypes"
                children={(field) => (
                  <div className="flex flex-wrap gap-2">
                    {serviceTypesData.map((st: any) => {
                      const isSelected = field.state.value.includes(st.id!);
                      return (
                        <button
                          type="button"
                          key={st.id}
                          onClick={() => {
                            if (isSelected) field.handleChange(field.state.value.filter((id: string) => id !== st.id));
                            else field.handleChange([...field.state.value, st.id!]);
                          }}
                          className={`px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all ${isSelected
                            ? "bg-black text-white border-black"
                            : "bg-gray-50 text-gray-400 border-gray-200 hover:border-gray-400 hover:text-gray-600"
                            }`}
                        >
                          {st.name}
                        </button>
                      );
                    })}
                  </div>
                )}
              />
            </div>
          </section>

          {/* Capabilities */}
          <section className="space-y-4">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2">
              Required Capabilities
            </h3>
            <div className="p-4 bg-white border border-gray-200 shadow-sm">
              <form.Field
                name="capabilities"
                children={(field) => (
                  <div className="flex flex-wrap gap-2">
                    {capabilitiesData.map((cap: any) => {
                      const isSelected = field.state.value.includes(cap.id!);
                      return (
                        <button
                          type="button"
                          key={cap.id}
                          onClick={() => {
                            if (isSelected) field.handleChange(field.state.value.filter((id: string) => id !== cap.id));
                            else field.handleChange([...field.state.value, cap.id!]);
                          }}
                          className={`px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all ${isSelected
                            ? "bg-red-600 text-white border-red-600"
                            : "bg-gray-50 text-gray-400 border-gray-200 hover:border-gray-400 hover:text-gray-600"
                            }`}
                        >
                          {cap.name}
                        </button>
                      );
                    })}
                  </div>
                )}
              />
            </div>
          </section>

          {/* Industries */}
          <section className="space-y-4">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2">
              Target Industries
            </h3>
            <div className="p-4 bg-white border border-gray-200 shadow-sm">
              <form.Field
                name="industries"
                children={(field) => (
                  <div className="flex flex-wrap gap-2">
                    {industriesData.map((ind: any) => {
                      const isSelected = field.state.value.includes(ind.id!);
                      return (
                        <button
                          type="button"
                          key={ind.id}
                          onClick={() => {
                            if (isSelected) field.handleChange(field.state.value.filter((id: string) => id !== ind.id));
                            else field.handleChange([...field.state.value, ind.id!]);
                          }}
                          className={`px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all ${isSelected
                            ? "bg-blue-600 text-white border-blue-600"
                            : "bg-gray-50 text-gray-400 border-gray-200 hover:border-gray-400 hover:text-gray-600"
                            }`}
                        >
                          {ind.name}
                        </button>
                      );
                    })}
                  </div>
                )}
              />
            </div>
          </section>
        </div>
      </form>

      {/* Footer Actions */}
      <div className="fixed bottom-0 left-0 md:left-72 right-0 p-6 bg-white border-t border-gray-200 flex justify-between items-center z-40 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]">
        <Button variant="outline" onClick={() => router.history.back()}>
          CANCEL
        </Button>
        <form.Subscribe
          selector={(state) => [state.canSubmit, state.isSubmitting]}
          children={([canSubmit, isSubmitting]) => (
            <Button
              onClick={form.handleSubmit}
              disabled={!canSubmit || isSubmitting || createMutation.isPending}
              className="bg-red-600 hover:bg-red-700 min-w-[160px] uppercase font-bold tracking-widest text-xs"
            >
              {createMutation.isPending || isSubmitting ? (
                <><Loader2 className="w-3 h-3 mr-2 animate-spin" /> Posting...</>
              ) : (
                "Post Opportunity"
              )}
            </Button>
          )}
        />
      </div>
    </div>
  );
}
