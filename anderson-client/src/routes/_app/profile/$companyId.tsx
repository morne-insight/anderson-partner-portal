import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useMutation } from "@tanstack/react-query";
import { useForm } from "@tanstack/react-form";
import { CapabilityDto, CompanyProfileDto, UpdateCompanyCommand } from "@/api/types.gen";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Switch } from "@/components/ui/switch";
import { 
  Loader2, 
  Globe, 
  Save, 
  Plus, 
  Trash2,
  Pencil,
  MapPin, 
  User, 
  Briefcase, 
  Building,
  MonitorCloud
} from "lucide-react";
import { useState } from "react";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { callApi } from "@/server/proxy";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import z from "zod";
import {
  Combobox,
  ComboboxChip,
  ComboboxChipRemove,
  ComboboxChips,
  ComboboxContent,
  ComboboxControl,
  ComboboxEmpty,
  ComboboxInput,
  ComboboxItem,
  ComboboxItemIndicator,
  ComboboxList,
  ComboboxValue,
} from '@/components/ui/base-combobox';

export const Route = createFileRoute("/_app/profile/$companyId")({
  component: ProfileEdit,
  loader: async ({ params }) => {
    // Only fetch company profile, reference data will be cached via hooks
    const company = await callApi({ data: { fn: 'getApiCompaniesByIdProfile', args: { path: { id: params.companyId } } } });

    return {
      company: company || {},
    };
  },
});

function ProfileEdit() {
  const { companyId } = Route.useParams();
  const { company } = Route.useLoaderData();
  
  // Use cached reference data hooks
  const { 
    capabilities: capabilitiesQuery, 
    industries: industriesQuery, 
    countries: countriesQuery, 
    regions: regionsQuery, 
    serviceTypes: serviceTypesQuery,
    isLoading, 
    isError 
  } = usePrefetchReferenceData();

  const initialCompany = company as CompanyProfileDto;
  const router = useRouter();

  
  // Type assertions after null checks - TypeScript now knows these are defined
  const capabilities = capabilitiesQuery.data;
  const industries = industriesQuery.data;
  const countries = countriesQuery.data;
  const regions = regionsQuery.data;
  const serviceTypes = serviceTypesQuery.data;
  
  // Refetch handler to update view after mutations
  const refreshData = () => {
    router.invalidate();
  };
  
  // --- AI Scrape State ---
  const [scrapeUrl, setScrapeUrl] = useState(initialCompany?.websiteUrl || "");
  const scrapeMutation = useMutation({
    mutationFn: async (url: string) => {
      // Note: putApiCompaniesScrapeWebsite needs a body, checking signature...
      // Signature: (options) => ... options.body is ScrapeWebsiteCommand { url: string }
      return callApi({ data: { fn: 'putApiCompaniesScrapeWebsite', args: { body: { url } } } });
    },
    onSuccess: () => {
      refreshData();
      alert("AI Sync started! Your data will be updated shortly.");
    },
    onError: (err) => {
      console.error(err);
      alert("Failed to start AI Sync.");
    }
  });
  
  // --- Main Form (General Info) ---
  const form = useForm({
    defaultValues: {
      name: initialCompany?.name || "",
      shortDescription: initialCompany?.shortDescription || "",
      fullDescription: initialCompany?.fullDescription || "",
      websiteUrl: initialCompany?.websiteUrl || "",
      employeeCount: initialCompany?.employeeCount || 0,
      serviceTypeId: initialCompany?.serviceTypeId || "",
    },
    validators: {
      onSubmit: ({value} : {value: UpdateCompanyCommand}) => {
        const schema = z.object({
          name: z.string().min(1, "Name is required"),
          shortDescription: z.string().optional(),
          fullDescription: z.string().optional(),
          websiteUrl: z.string().url("Invalid URL").optional().or(z.literal("")),
          employeeCount: z.number().min(0).optional(),
          serviceTypeId: z.string().optional(),
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
    onSubmit: async ({ value }: {value: UpdateCompanyCommand}) => {
      try {
        // Save Capabilities and Industries first, to ensure embedding are created with the latest data
        await callApi({ data: { fn: 'putApiCompaniesByIdCapabilities', args: { path: { id: companyId }, body: { id: companyId, capabilityIds: selectedCapabilityIds } } } });
        
        await callApi({ data: { fn: 'putApiCompaniesByIdIndustries', args: { path: { id: companyId }, body: { id: companyId, industryIds: selectedIndustryIds } } } });
        
        // Save Company 
        await callApi({ 
          data: { 
            fn: 'putApiCompaniesById', 
            args: { 
              path: { id: companyId }, 
              body: { 
                id: companyId, 
                name: value.name, 
                shortDescription: value.shortDescription,
                fullDescription: value.fullDescription, 
                websiteUrl: value.websiteUrl,
                employeeCount: value.employeeCount,
                serviceTypeId: value.serviceTypeId,
              }
            }
          }
        });   
          
        alert("Profile saved successfully!");
        refreshData();
      } catch (error) {
        console.error(error);
          alert("Failed to save changes.");
        }
      },
      // validatorAdapter: zodValidator(),
    } as any); // Cast to any to avoid complex form typing issues for now
    
    // --- Capabilities & Industries State ---
  const [selectedCapabilityIds, setSelectedCapabilityIds] = useState<string[]>(
    initialCompany?.capabilities?.map((c) => c.id!).filter(Boolean) || []
  );
  const [selectedIndustryIds, setSelectedIndustryIds] = useState<string[]>(
    initialCompany?.industries?.map((c) => c.id!).filter(Boolean) || []
  );


  // --- Locations State ---
  const [isAddingLocation, setIsAddingLocation] = useState(false);
  const [newLocation, setNewLocation] = useState({ name: "", regionId: "", countryId: "", isHeadOffice: false });
  const [editingLocationId, setEditingLocationId] = useState<string | null>(null);
  const [editLocation, setEditLocation] = useState({ name: "", regionId: "", countryId: "", isHeadOffice: false });
  
  const addLocationMutation = useMutation({
    mutationFn: async () => {
      return callApi({
        data: {
          fn: 'postApiCompaniesByIdLocation',
          args: {
            path: { id: companyId },
            body: {
              id: companyId,
              name: newLocation.name,
              regionId: newLocation.regionId,
              countryId: newLocation.countryId,
              isHeadOffice: newLocation.isHeadOffice
            }
          }
        }
      });
    },
    onSuccess: () => {
      setIsAddingLocation(false);
      setNewLocation({ name: "", regionId: "", countryId: "", isHeadOffice: false });
      refreshData();
    }
  });

  const deleteLocationMutation = useMutation({
    mutationFn: async (locationId: string) => {
      return callApi({ data: { fn: 'deleteApiCompaniesByIdLocation', args: { path: { id: companyId }, query: { locationId } } } });
    },
    onSuccess: () => {
      refreshData();
    }
  });

  const updateLocationMutation = useMutation({
    mutationFn: async (locationId: string) => {
      return callApi({
        data: {
          fn: 'putApiCompaniesByIdLocation',
          args: {
            path: { id: companyId },
            body: {
              id: companyId,
              locationId,
              name: editLocation.name,
              regionId: editLocation.regionId,
              countryId: editLocation.countryId,
              isHeadOffice: editLocation.isHeadOffice
            }
          }
        }
      });
    },
    onSuccess: () => {
      setEditingLocationId(null);
      setEditLocation({ name: "", regionId: "", countryId: "", isHeadOffice: false });
      refreshData();
    }
  });

  const startEditLocation = (loc: any) => {
    setEditingLocationId(loc.id!);
    setEditLocation({
      name: loc.name || "",
      regionId: loc.regionId || "",
      countryId: loc.countryId || "",
      isHeadOffice: loc.isHeadOffice || false
    });
  };

  // --- Contacts State ---
  const [isAddingContact, setIsAddingContact] = useState(false);
  const [newContact, setNewContact] = useState({ firstName: "", lastName: "", email: "", position: "" });

  // --- Invites State ---
  const [isAddingInvite, setIsAddingInvite] = useState(false);
  const [newInvite, setNewInvite] = useState({ name: "", email: "" });

  const addContactMutation = useMutation({
    mutationFn: async () => {
        // PostApiCompaniesByIdContactData
      return callApi({
        data: {
          fn: 'postApiCompaniesByIdContact',
          args: {
            path: { id: companyId },
            body: {
            id: companyId,
            firstName: newContact.firstName,
            lastName: newContact.lastName,
            emailAddress: newContact.email,
            companyPosition: newContact.position
        }
      }
    }
    });
    },
    onSuccess: () => {
      setIsAddingContact(false);
      setNewContact({ firstName: "", lastName: "", email: "", position: "" });
      refreshData();
    }
  });

  const deleteContactMutation = useMutation({
    mutationFn: async (contactId: string) => {
       return callApi({ data: { fn: 'deleteApiCompaniesByIdContact', args: { path: { id: companyId }, query: { contactId } } } });
    },
    onSuccess: () => {
        refreshData();
    }
  });

  // --- ApplicationUser Mutations ---
  const deleteUserMutation = useMutation({
    mutationFn: async (userId: string) => {
      return callApi({ data: { fn: 'deleteApiCompaniesByIdUser', args: { path: { id: companyId }, query: { userId } } } });
    },
    onSuccess: () => {
      refreshData();
    }
  });

  // --- Invite Mutations ---
  const addInviteMutation = useMutation({
    mutationFn: async () => {
      return callApi({
        data: {
          fn: 'postApiInvites',
          args: {
            body: {
              email: newInvite.email,
              companyId: companyId
            }
          }
        }
      });
    },
    onSuccess: () => {
      setIsAddingInvite(false);
      setNewInvite({ name: "", email: "" });
      refreshData();
    }
  });

  const deleteInviteMutation = useMutation({
    mutationFn: async (inviteId: string) => {
      return callApi({
        data: {
          fn: 'deleteApiInvitesById',
          args: {
            path: { id: inviteId }
          }
        }
      });
    },
    onSuccess: () => {
      refreshData();
    }
  });

  const [editingContactId, setEditingContactId] = useState<string | null>(null);
  const [editContact, setEditContact] = useState({ firstName: "", lastName: "", email: "", position: "" });

  const updateContactMutation = useMutation({
    mutationFn: async (contactId: string) => {
      return callApi({
        data: {
          fn: 'putApiCompaniesByIdContact',
          args: {
            path: { id: companyId },
            body: {
          id: companyId,
          contactId,
          firstName: editContact.firstName,
          lastName: editContact.lastName,
          emailAddress: editContact.email,
          companyPosition: editContact.position
        }
      }
    }
    });
    },
    onSuccess: () => {
      setEditingContactId(null);
      setEditContact({ firstName: "", lastName: "", email: "", position: "" });
      refreshData();
    }
  });

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
 

  const startEditContact = (contact: any) => {
    setEditingContactId(contact.id!);
    setEditContact({
      firstName: contact.firstName || "",
      lastName: contact.lastName || "",
      email: contact.emailAddress || "",
      position: contact.companyPosition || ""
    });
  };

  return (
    <div className="space-y-8 animate-fade-in pb-20">
      {/* Header */}
      <header className="border-b border-gray-200 pb-6">
        <h2 className="text-4xl font-serif text-black mb-3">Edit Firm Profile</h2>
        <p className="text-gray-500 font-light text-lg">
          Update your firm's details, capabilities, and global presence.
        </p>
      </header>

      {/* AI Auto-Sync */}
      <div className="bg-black text-white p-8 rounded-lg relative overflow-hidden">
        <div className="absolute top-0 right-0 p-32 bg-red-600/10 rounded-full blur-3xl transform translate-x-1/2 -translate-y-1/2"></div>
        <div className="relative z-10">
          <h3 className="text-xl font-bold uppercase tracking-widest flex items-center gap-3 mb-4">
            <Globe className="w-5 h-5 text-red-500" /> AI Auto-Sync
          </h3>
          <p className="text-gray-400 mb-6 max-w-xl text-sm leading-relaxed">
            Enter your website URL to automatically update your service description and capabilities using AI.
            We'll analyze your public presence to suggest relevant tags and descriptions.
          </p>
          <div className="flex flex-col md:flex-row gap-4 max-w-2xl">
            <div className="flex-1">
              <input
                type="text"
                value={scrapeUrl}
                onChange={(e) => setScrapeUrl(e.target.value)}
                placeholder="https://www.yourcompany.com"
                className="w-full px-4 py-3 bg-white/10 border border-white/20 text-white rounded focus:outline-none focus:border-red-600 transition-colors placeholder-gray-500 text-sm"
              />
            </div>
            <button
              onClick={() => scrapeMutation.mutate(scrapeUrl)}
              disabled={scrapeMutation.isPending || !scrapeUrl}
              className="px-8 py-3 bg-red-600 hover:bg-red-700 text-white text-xs font-bold uppercase tracking-[0.2em] transition-colors disabled:opacity-50 disabled:cursor-not-allowed whitespace-nowrap"
            >
              {scrapeMutation.isPending ? "Syncing..." : "Sync Firm Data"}
            </button>
          </div>
        </div>
      </div>

      {/* Main Form */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-12">
        {/* Left Column: General Info */}
        <div className="lg:col-span-2 space-y-12">
          
          {/* General Information */}
          <section className="space-y-6">
            <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2">
              General Information
            </h3>
            
            <form.Field
              name="name"
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor={field.name}>Firm Name</Label>
                  <Input 
                    id={field.name} 
                    name={field.name} 
                    value={field.state.value as string} 
                    onBlur={field.handleBlur}
                    onChange={(e: any) => field.handleChange(e.target.value)} 
                  />
                </div>
              )}
            />

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
               <form.Field
                name="websiteUrl"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Website Address</Label>
                    <Input 
                        id={field.name} 
                        name={field.name} 
                        value={field.state.value as string} 
                        onBlur={field.handleBlur}
                        onChange={(e: any) => field.handleChange(e.target.value)} 
                    />
                  </div>
                )}
              />
                <form.Field
                name="employeeCount"
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Employee Count</Label>
                    <Input 
                        id={field.name} 
                        name={field.name} 
                        type="number"
                        value={field.state.value as string} 
                        onBlur={field.handleBlur}
                        onChange={(e: any) => field.handleChange(Number(e.target.value))} 
                    />
                  </div>
                )}
              />
            </div>

             <form.Field
              name="shortDescription"
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor={field.name}>Short Description</Label>
                  <Textarea 
                      id={field.name} 
                      name={field.name} 
                      value={field.state.value as string} 
                      onBlur={field.handleBlur}
                      onChange={(e: any) => field.handleChange(e.target.value)} 
                   />
                   <p className="text-[10px] text-gray-500 uppercase tracking-wide">Brief summary for card views</p>
                </div>
              )}
            />

             <form.Field
              name="fullDescription"
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor={field.name}>Firm Description</Label>
                  <Textarea 
                      id={field.name} 
                      name={field.name} 
                      value={field.state.value as string} 
                      onBlur={field.handleBlur}
                      onChange={(e: any) => field.handleChange(e.target.value)} 
                      className="min-h-[150px]"
                   />
                </div>
              )}
            />
          </section>

          {/* Detailed Lists (Locations & Contacts) */}
            
            {/* Locations */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-b border-gray-100 pb-2">
                <h3 className="text-lg font-bold uppercase tracking-widest">
                    Office Locations
                </h3>
            </div>
            
            <div className="space-y-4">
                {initialCompany?.locations?.map(loc => (
                    <div key={loc.id}>
                      {editingLocationId === loc.id ? (
                        <div className="p-6 bg-blue-50 border border-blue-200 space-y-4">
                          <div className="space-y-2">
                            <Label>Office Name</Label>
                            <Input value={editLocation.name} onChange={(e: any) => setEditLocation({...editLocation, name: e.target.value})} placeholder="e.g. London HQ" />
                          </div>
                          <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                              <Label>Region</Label>
                              <Select value={editLocation.regionId} onValueChange={(v: string) => setEditLocation({...editLocation, regionId: v})}>
                                <SelectTrigger><SelectValue placeholder="Select Region" /></SelectTrigger>
                                <SelectContent>
                                  {regions?.map((r: any) => <SelectItem key={r.id} value={r.id!}>{r.name}</SelectItem>)}
                                </SelectContent>
                              </Select>
                            </div>
                            <div className="space-y-2">
                              <Label>Country</Label>
                              <Select value={editLocation.countryId} onValueChange={(v: string) => setEditLocation({...editLocation, countryId: v})}>
                                <SelectTrigger><SelectValue placeholder="Select Country" /></SelectTrigger>
                                <SelectContent>
                                  {countries?.filter((c: any) => !editLocation.regionId || c.regionId === editLocation.regionId)
                                    .map((c: any) => <SelectItem key={c.id} value={c.id!}>{c.name}</SelectItem>)
                                  }
                                </SelectContent>
                              </Select>
                            </div>
                          </div>
                          <div className="flex items-center gap-2">
                            <Switch 
                              checked={editLocation.isHeadOffice} 
                              onCheckedChange={c => setEditLocation({...editLocation, isHeadOffice: c})} 
                              id={`edit-head-office-${loc.id}`}
                            />
                            <Label htmlFor={`edit-head-office-${loc.id}`}>This is the Head Office</Label>
                          </div>
                          <div className="flex gap-2 justify-end pt-2">
                            <Button variant="outline" onClick={() => setEditingLocationId(null)} size="sm">Cancel</Button>
                            <Button 
                              onClick={() => updateLocationMutation.mutate(loc.id!)} 
                              disabled={updateLocationMutation.isPending || !editLocation.name}
                              size="sm"
                              className="bg-blue-600 hover:bg-blue-700"
                            >
                              {updateLocationMutation.isPending ? "Saving..." : "Save Changes"}
                            </Button>
                          </div>
                        </div>
                      ) : (
                        <div className="flex items-center justify-between p-4 bg-gray-50 border border-gray-100 rounded-sm">
                          <div className="flex items-center gap-4">
                            <div className="w-8 h-8 flex items-center justify-center bg-white border border-gray-200">
                              <MapPin className="w-4 h-4 text-gray-400" />
                            </div>
                            <div>
                              <div className="font-bold text-sm text-gray-900">{loc.name} {loc.isHeadOffice && <span className="text-[10px] bg-black text-white px-2 py-0.5 ml-2 uppercase tracking-wide">Head Office</span>}</div>
                              <div className="text-xs text-gray-500 uppercase tracking-wide mt-1">
                                {countries?.find(c => c.id === loc.countryId)?.name}, {regions?.find(r => r.id === loc.regionId)?.name}
                              </div>
                            </div>
                          </div>
                          <div className="flex items-center gap-2">
                            <button 
                              onClick={() => startEditLocation(loc)}
                              className="text-gray-400 hover:text-blue-600 transition-colors"
                            >
                              <Pencil className="w-4 h-4" />
                            </button>
                            <button 
                              onClick={() => {
                                if (confirm("Are you sure you want to delete this location?")) {
                                  deleteLocationMutation.mutate(loc.id!);
                                }
                              }}
                              className="text-gray-400 hover:text-red-600 transition-colors"
                            >
                              <Trash2 className="w-4 h-4" />
                            </button>
                          </div>
                        </div>
                      )}
                    </div>
                ))}

                {isAddingLocation ? (
                    <div className="p-6 bg-gray-50 border border-gray-200 space-y-4">
                        <div className="space-y-2">
                            <Label>Office Name</Label>
                            <Input value={newLocation.name} onChange={(e: any) => setNewLocation({...newLocation, name: e.target.value})} placeholder="e.g. London HQ" />
                        </div>
                        <div className="grid grid-cols-2 gap-4">
                             <div className="space-y-2">
                                <Label>Region</Label>
                                <Select value={newLocation.regionId} onValueChange={(v: string) => setNewLocation({...newLocation, regionId: v})}>
                                    <SelectTrigger><SelectValue placeholder="Select Region" /></SelectTrigger>
                                    <SelectContent>
                                        {regions?.map((r: any) => <SelectItem key={r.id} value={r.id!}>{r.name}</SelectItem>)}
                                    </SelectContent>
                                </Select>
                            </div>
                             <div className="space-y-2">
                                <Label>Country</Label>
                                <Select value={newLocation.countryId} onValueChange={(v: string) => setNewLocation({...newLocation, countryId: v})}>
                                    <SelectTrigger><SelectValue placeholder="Select Country" /></SelectTrigger>
                                    <SelectContent>
                                        {countries
                                            ?.filter((c: any) => !newLocation.regionId || c.regionId === newLocation.regionId)
                                            .map((c: any) => <SelectItem key={c.id} value={c.id!}>{c.name}</SelectItem>)
                                        }
                                    </SelectContent>
                                </Select>
                            </div>
                        </div>
                        <div className="flex items-center gap-2">
                            <Switch 
                                checked={newLocation.isHeadOffice} 
                                onCheckedChange={c => setNewLocation({...newLocation, isHeadOffice: c})} 
                                id="header-office"
                            />
                            <Label htmlFor="head-office">This is the Head Office</Label>
                        </div>
                        <div className="flex gap-2 justify-end pt-2">
                             <Button variant="outline" onClick={() => setIsAddingLocation(false)} size="sm">Cancel</Button>
                             <Button 
                                onClick={() => addLocationMutation.mutate()} 
                                disabled={addLocationMutation.isPending || !newLocation.name}
                                size="sm"
                                className="bg-black hover:bg-gray-800"
                            >
                                {addLocationMutation.isPending ? "Adding..." : "Add Location"}
                            </Button>
                        </div>
                    </div>
                ) : (
                    <Button variant="outline" onClick={() => setIsAddingLocation(true)} className="w-full border-dashed border-gray-300 text-gray-500 hover:border-gray-900 hover:text-gray-900">
                        <Plus className="w-4 h-4 mr-2" /> Add Office Location
                    </Button>
                )}
            </div>
          </section>

          {/* Key Personnel */}
           <section className="space-y-6">
            <div className="flex items-center justify-between border-b border-gray-100 pb-2">
                <h3 className="text-lg font-bold uppercase tracking-widest">
                    Key Personnel
                </h3>
            </div>
            
            <div className="space-y-4">
                {initialCompany?.contacts?.map(contact => (
                    <div key={contact.id}>
                      {editingContactId === contact.id ? (
                        <div className="p-6 bg-blue-50 border border-blue-200 space-y-4">
                          <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                              <Label>First Name</Label>
                              <Input value={editContact.firstName} onChange={(e: any) => setEditContact({...editContact, firstName: e.target.value})} />
                            </div>
                            <div className="space-y-2">
                              <Label>Last Name</Label>
                              <Input value={editContact.lastName} onChange={(e: any) => setEditContact({...editContact, lastName: e.target.value})} />
                            </div>
                          </div>
                          <div className="space-y-2">
                            <Label>Job Title</Label>
                            <Input value={editContact.position} onChange={(e: any) => setEditContact({...editContact, position: e.target.value})} />
                          </div>
                          <div className="space-y-2">
                            <Label>Email Address</Label>
                            <Input value={editContact.email} onChange={(e: any) => setEditContact({...editContact, email: e.target.value})} />
                          </div>
                          <div className="flex gap-2 justify-end pt-2">
                            <Button variant="outline" onClick={() => setEditingContactId(null)} size="sm">Cancel</Button>
                            <Button 
                              onClick={() => updateContactMutation.mutate(contact.id!)} 
                              disabled={updateContactMutation.isPending || !editContact.firstName}
                              size="sm"
                              className="bg-blue-600 hover:bg-blue-700"
                            >
                              {updateContactMutation.isPending ? "Saving..." : "Save Changes"}
                            </Button>
                          </div>
                        </div>
                      ) : (
                        <div className="flex items-center justify-between p-4 bg-gray-50 border border-gray-100 rounded-sm">
                          <div className="flex items-center gap-4">
                            <div className="w-8 h-8 flex items-center justify-center bg-white border border-gray-200 rounded-full">
                              <User className="w-4 h-4 text-gray-400" />
                            </div>
                            <div>
                              <div className="font-bold text-sm text-gray-900">{contact.firstName} {contact.lastName}</div>
                              <div className="text-xs text-gray-500 uppercase tracking-wide mt-1">
                                {contact.companyPosition || "No Title"} • {contact.emailAddress}
                              </div>
                            </div>
                          </div>
                          <div className="flex items-center gap-2">
                            <button 
                              onClick={() => startEditContact(contact)}
                              className="text-gray-400 hover:text-blue-600 transition-colors"
                            >
                              <Pencil className="w-4 h-4" />
                            </button>
                            <button 
                              onClick={() => {
                                if (confirm("Are you sure you want to delete this contact?")) {
                                  deleteContactMutation.mutate(contact.id!);
                                }
                              }}
                              className="text-gray-400 hover:text-red-600 transition-colors"
                            >
                              <Trash2 className="w-4 h-4" />
                            </button>
                          </div>
                        </div>
                      )}
                    </div>
                ))}
            
             {isAddingContact ? (
                 <div className="p-6 bg-gray-50 border border-gray-200 space-y-4">
                     <div className="grid grid-cols-2 gap-4">
                         <div className="space-y-2">
                             <Label>First Name</Label>
                             <Input value={newContact.firstName} onChange={(e: any) => setNewContact({...newContact, firstName: e.target.value})} />
                         </div>
                         <div className="space-y-2">
                             <Label>Last Name</Label>
                             <Input value={newContact.lastName} onChange={(e: any) => setNewContact({...newContact, lastName: e.target.value})} />
                         </div>
                     </div>
                      <div className="space-y-2">
                         <Label>Job Title</Label>
                         <Input value={newContact.position} onChange={(e: any) => setNewContact({...newContact, position: e.target.value})} />
                     </div>
                      <div className="space-y-2">
                         <Label>Email Address</Label>
                         <Input value={newContact.email} onChange={(e: any) => setNewContact({...newContact, email: e.target.value})} />
                     </div>
                     <div className="flex gap-2 justify-end pt-2">
                          <Button variant="outline" onClick={() => setIsAddingContact(false)} size="sm">Cancel</Button>
                          <Button 
                             onClick={() => addContactMutation.mutate()} 
                             disabled={addContactMutation.isPending || !newContact.firstName}
                             size="sm"
                             className="bg-black hover:bg-gray-800"
                         >
                             {addContactMutation.isPending ? "Adding..." : "Add Person"}
                         </Button>
                     </div>
                 </div>
             ) : (
                <Button variant="outline" onClick={() => setIsAddingContact(true)} className="w-full border-dashed border-gray-300 text-gray-500 hover:border-gray-900 hover:text-gray-900">
                    <Plus className="w-4 h-4 mr-2" /> Add Key Person
                </Button>
             )}
            </div>
           </section>

          {/* Application Users */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-b border-gray-100 pb-2">
                <h3 className="text-lg font-bold uppercase tracking-widest">
                    Application Users
                </h3>
            </div>
            
            <div className="space-y-4">
                {initialCompany?.applicationIdentityUsers?.map((user, index: number) => (
                    <div key={user.id || index} className="flex items-center justify-between p-4 bg-gray-50 border border-gray-100 rounded-sm">
                        <div className="flex items-center gap-4">
                            <div className="w-8 h-8 flex items-center justify-center bg-white border border-gray-200 rounded-full">
                                <User className="w-4 h-4 text-gray-400" />
                            </div>
                            <div>
                                <div className="font-bold text-sm text-gray-900">
                                    {user.email || 'Unknown User'}
                                </div>
                                <div className="text-xs text-gray-500 uppercase tracking-wide mt-1">
                                    {user.userName ||'Application User'}
                                </div>
                            </div>
                        </div>
                        <div className="flex items-center gap-2">
                            <button 
                                onClick={() => {
                                    if (confirm("Are you sure you want to remove this user from the company?")) {
                                        deleteUserMutation.mutate(user.id!);
                                    }
                                }}
                                className="text-gray-400 hover:text-red-600 transition-colors"
                                disabled={deleteUserMutation.isPending}
                            >
                                <Trash2 className="w-4 h-4" />
                            </button>
                        </div>
                    </div>
                ))}

                {(!initialCompany?.applicationIdentityUsers || initialCompany.applicationIdentityUsers.length === 0) && (
                    <div className="p-6 bg-gray-50 border border-gray-200 text-center">
                        <p className="text-gray-500 text-sm">No application users found.</p>
                    </div>
                )}
            </div>
          </section>

          {/* Company Invites */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-b border-gray-100 pb-2">
                <h3 className="text-lg font-bold uppercase tracking-widest">
                    Pending Invites
                </h3>
            </div>
            
            <div className="space-y-4">
                {initialCompany?.invites?.map(invite => (
                    <div key={invite.id} className="flex items-center justify-between p-4 bg-gray-50 border border-gray-100 rounded-sm">
                        <div className="flex items-center gap-4">
                            <div className="w-8 h-8 flex items-center justify-center bg-white border border-gray-200 rounded-full">
                                <User className="w-4 h-4 text-gray-400" />
                            </div>
                            <div>
                                <div className="font-bold text-sm text-gray-900">{invite.name}</div>
                                <div className="text-xs text-gray-500 uppercase tracking-wide mt-1">
                                    {invite.email} • Pending Invite
                                </div>
                            </div>
                        </div>
                        <div className="flex items-center gap-2">
                            <button 
                                onClick={() => {
                                    if (confirm("Are you sure you want to cancel this invite?")) {
                                        deleteInviteMutation.mutate(invite.id!);
                                    }
                                }}
                                className="text-gray-400 hover:text-red-600 transition-colors"
                                disabled={deleteInviteMutation.isPending}
                            >
                                <Trash2 className="w-4 h-4" />
                            </button>
                        </div>
                    </div>
                ))}

                {isAddingInvite ? (
                    <div className="p-6 bg-gray-50 border border-gray-200 space-y-4">
                        <div className="space-y-2">
                            <Label>Full Name</Label>
                            <Input 
                                value={newInvite.name} 
                                onChange={(e: any) => setNewInvite({...newInvite, name: e.target.value})} 
                                placeholder="Enter full name"
                            />
                        </div>
                        <div className="space-y-2">
                            <Label>Email Address</Label>
                            <Input 
                                type="email"
                                value={newInvite.email} 
                                onChange={(e: any) => setNewInvite({...newInvite, email: e.target.value})} 
                                placeholder="Enter email address"
                            />
                        </div>
                        <div className="flex gap-2 justify-end pt-2">
                            <Button variant="outline" onClick={() => setIsAddingInvite(false)} size="sm">Cancel</Button>
                            <Button 
                                onClick={() => addInviteMutation.mutate()} 
                                disabled={addInviteMutation.isPending || !newInvite.name || !newInvite.email}
                                size="sm"
                                className="bg-black hover:bg-gray-800"
                            >
                                {addInviteMutation.isPending ? "Sending..." : "Send Invite"}
                            </Button>
                        </div>
                    </div>
                ) : (
                    <Button variant="outline" onClick={() => setIsAddingInvite(true)} className="w-full border-dashed border-gray-300 text-gray-500 hover:border-gray-900 hover:text-gray-900">
                        <Plus className="w-4 h-4 mr-2" /> Send Invite
                    </Button>
                )}
            </div>
          </section>

        </div>

        {/* Right Column: Tags & Metadata */}
        <div className="space-y-12">
             {/* Service Type */}
            <section className="space-y-6">
                <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2 mb-0">
                    <MonitorCloud className="w-4 h-4" /> Service Type
                </h3>
                <div className="p-6 pt-4 bg-white border border-gray-200 shadow-sm w-full">
                    <p className="text-xs text-gray-400 italic mb-2">
                        Select the primary service your firm provides.
                    </p>
                    <form.Field
                        name="serviceTypeId"
                        children={(field) => (
                            <div>
                                <Select 
                                    value={field.state.value as string} 
                                    onValueChange={field.handleChange}
                                >
                                    <SelectTrigger className="w-full">
                                        <SelectValue placeholder="Select a service type" />
                                    </SelectTrigger>
                                    <SelectContent className="w-full">
                                        {serviceTypes?.map((serviceType) => (
                                            <SelectItem key={serviceType.id} value={serviceType.id!}>
                                                {serviceType.name}
                                            </SelectItem>
                                        ))}
                                    </SelectContent>
                                </Select>
                                {field.state.meta.errors && (
                                    <p className="text-red-500 text-xs mt-1">{field.state.meta.errors}</p>
                                )}
                            </div>
                        )}
                    />
                </div>
            </section>

             {/* Core Skills */}
            <section className="space-y-6">
                <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2 mb-0">
                    <Briefcase className="w-4 h-4" /> Core Skills
                </h3>
                <div className="p-6 pt-4 bg-white border border-gray-200 shadow-sm">
                    <Combobox 
                        items={capabilities || []} 
                        multiple 
                        value={capabilities?.filter(cap => selectedCapabilityIds.includes(cap.id!)) || []}
                        onValueChange={(value: unknown) => {
                            const selectedCapabilities = value as CapabilityDto[];
                            setSelectedCapabilityIds(selectedCapabilities.map(cap => cap.id!));
                        }}
                    >
                        <ComboboxChips className="p-0 border-0 shadow-none mb-4">
                            <ComboboxValue>
                                {(value: CapabilityDto[]) => (
                                    <>
                                        {value.length === 0 && (
                                            <p className="text-xs text-gray-400 italic my-2">No capabilities selected.</p>
                                        )}
                                        {value.map((capability) => (
                                            <ComboboxChip 
                                                key={capability.id} 
                                                aria-label={capability.name} 
                                                className="px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all bg-black text-white border-black rounded-none"
                                            >
                                                {capability.name}
                                                <ComboboxChipRemove />
                                            </ComboboxChip>
                                        ))}
                                    </>
                                )}
                            </ComboboxValue>
                        </ComboboxChips>

                    <p className="text-xs text-gray-400 italic my-2">
                        Select all that apply to your firm.
                    </p>
                        <ComboboxControl>
                            <ComboboxValue>
                                <ComboboxInput placeholder="Search and select capabilities..." />
                            </ComboboxValue>
                        </ComboboxControl>

                        <ComboboxContent>
                            <ComboboxEmpty>No capabilities found.</ComboboxEmpty>  
                            <ComboboxList>
                                {(capability: CapabilityDto) => (
                                    <ComboboxItem key={capability.id} value={capability}>   
                                        <ComboboxItemIndicator />
                                        <div className="col-start-2">{capability.name}</div>  
                                    </ComboboxItem>
                                )}
                            </ComboboxList> 
                        </ComboboxContent>
                    </Combobox>
                </div>
            </section>

             {/* Industries */}
            <section className="space-y-6">
                <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2 mb-0">
                     <Building className="w-4 h-4" /> Primary Industries
                </h3>
                <div className="p-6 bg-white border border-gray-200 shadow-sm">
                    <Combobox 
                        items={industries || []} 
                        multiple 
                        value={industries?.filter(ind => selectedIndustryIds.includes(ind.id!)) || []}
                        onValueChange={(value: unknown) => {
                            const selectedIndustries = value as any[];
                            setSelectedIndustryIds(selectedIndustries.map(ind => ind.id!));
                        }}
                    >
                        <ComboboxChips className="p-0 border-0 shadow-none mb-4">
                            <ComboboxValue>
                                {(value: any[]) => (
                                    <>
                                        {value.length === 0 && (
                                            <p className="text-xs text-gray-400 italic my-2">No industries selected.</p>
                                        )}
                                        {value.map((industry) => (
                                            <ComboboxChip 
                                                key={industry.id} 
                                                aria-label={industry.name} 
                                                className="px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all bg-red-600 text-white border-red-600 rounded-none"
                                            >
                                                {industry.name}
                                                <ComboboxChipRemove />
                                            </ComboboxChip>
                                        ))}
                                    </>
                                )}
                            </ComboboxValue>
                        </ComboboxChips>

                    <p className="text-xs text-gray-400 italic my-2">
                        Select the primary industries your firm serves.
                    </p>
                        <ComboboxControl>
                            <ComboboxValue>
                                <ComboboxInput placeholder="Search and select industries..." />
                            </ComboboxValue>
                        </ComboboxControl>

                        <ComboboxContent>
                            <ComboboxEmpty>No industries found.</ComboboxEmpty>  
                            <ComboboxList>
                                {(industry: any) => (
                                    <ComboboxItem key={industry.id} value={industry}>   
                                        <ComboboxItemIndicator />
                                        <div className="col-start-2">{industry.name}</div>  
                                    </ComboboxItem>
                                )}
                            </ComboboxList> 
                        </ComboboxContent>
                    </Combobox>
                </div>
            </section>
        </div>
      </div>

      {/* Footer Actions */}
      <div className="fixed bottom-0 left-0 md:left-72 right-0 p-6 bg-white border-t border-gray-200 flex justify-between items-center z-40 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)]">
        <span className="text-xs text-gray-400 font-medium uppercase tracking-widest hidden md:inline-block">
            Last saved: {new Date().toLocaleDateString()}
        </span>
        <div className="flex items-center gap-4 w-full md:w-auto justify-end">
             <Button variant="outline" onClick={() => router.history.back()}>CANCEL</Button>
            <form.Subscribe
                selector={(state) => [state.canSubmit, state.isSubmitting]}
                children={([canSubmit, isSubmitting]) => (
                    <Button 
                        onClick={form.handleSubmit} 
                        disabled={!canSubmit}
                        className="bg-red-600 hover:bg-red-700 min-w-[160px] uppercase font-bold tracking-widest text-xs"
                    >
                        {isSubmitting ? (
                            <>Saving <Loader2 className="w-3 h-3 ml-2 animate-spin" /></>
                        ) : (
                            <>Save Changes <Save className="w-3 h-3 ml-2" /></>
                        )}
                    </Button>
                )}
            />
        </div>
      </div>
    </div>
  );
}
