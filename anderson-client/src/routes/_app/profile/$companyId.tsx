import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "@tanstack/react-form";
import { 
  getApiCompaniesByIdProfile, 
  putApiCompaniesScrapeWebsite, 
  putApiCompaniesById, 
  putApiCompaniesByIdCapabilities,
  putApiCompaniesByIdIndustries,
  postApiCompaniesByIdLocation,
  putApiCompaniesByIdLocation,
  deleteApiCompaniesByIdLocation,
  postApiCompaniesByIdContact,
  putApiCompaniesByIdContact,
  deleteApiCompaniesByIdContact,
  getApiCapabilities,
  getApiIndustries,
  getApiCountries,
  getApiRegions
} from "@/api/sdk.gen";
import { CompanyProfileDto } from "@/api/types.gen";
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
  Building 
} from "lucide-react";
import React, { useState } from "react";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";

export const Route = createFileRoute("/_app/profile/$companyId")({
  component: ProfileEdit,
  loader: async ({ params }) => {
    // Fetch all required reference data and company profile
    const [company, capabilities, industries, countries, regions] = await Promise.all([
      getApiCompaniesByIdProfile({ path: { id: params.companyId } }),
      getApiCapabilities(),
      getApiIndustries(),
      getApiCountries(),
      getApiRegions(),
    ]);

    return {
      company: company.data as any,
      capabilities: capabilities.data || [],
      industries: industries.data || [],
      countries: countries.data || [],
      regions: regions.data || [],
    };
  },
});

function ProfileEdit() {
  const { companyId } = Route.useParams();
  const { 
    company, 
    capabilities: allCapabilities, 
    industries: allIndustries,
    countries: allCountries,
    regions: allRegions 
  } = Route.useLoaderData();

  const initialCompany = company as CompanyProfileDto;
  const router = useRouter();

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
      return putApiCompaniesScrapeWebsite({ body: { url } });
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
    },
    onSubmit: async ({ value }) => {
      try {
        await putApiCompaniesById({
          path: { id: companyId },
          body: {
            id: companyId,
            name: value.name,
            shortDescription: value.shortDescription,
            description: value.fullDescription, // Mapping fullDescription to description as per types
            websiteUrl: value.websiteUrl,
            employeeCount: value.employeeCount,
          }
        });
        
        // Also save Capabilities and Industries
        await putApiCompaniesByIdCapabilities({
          path: { id: companyId },
          body: { id: companyId, capabilityIds: selectedCapabilityIds }
        });
        
        await putApiCompaniesByIdIndustries({
            path: { id: companyId },
            body: { id: companyId, industryIds: selectedIndustryIds }
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

  const toggleCapability = (id: string) => {
    if (selectedCapabilityIds.includes(id)) {
      setSelectedCapabilityIds(prev => prev.filter(c => c !== id));
    } else {
      setSelectedCapabilityIds(prev => [...prev, id]);
    }
  };

  const toggleIndustry = (id: string) => {
    if (selectedIndustryIds.includes(id)) {
      setSelectedIndustryIds(prev => prev.filter(c => c !== id));
    } else {
      setSelectedIndustryIds(prev => [...prev, id]);
    }
  };

  // --- Locations State ---
  const [isAddingLocation, setIsAddingLocation] = useState(false);
  const [newLocation, setNewLocation] = useState({ name: "", regionId: "", countryId: "", isHeadOffice: false });
  const [editingLocationId, setEditingLocationId] = useState<string | null>(null);
  const [editLocation, setEditLocation] = useState({ name: "", regionId: "", countryId: "", isHeadOffice: false });
  
  const addLocationMutation = useMutation({
    mutationFn: async () => {
      return postApiCompaniesByIdLocation({
        path: { id: companyId },
        body: {
          id: companyId,
          name: newLocation.name,
          regionId: newLocation.regionId,
          countryId: newLocation.countryId,
          isHeadOffice: newLocation.isHeadOffice
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
      return deleteApiCompaniesByIdLocation({ path: { id: companyId }, query: { locationId } });
    },
    onSuccess: () => {
      refreshData();
    }
  });

  const updateLocationMutation = useMutation({
    mutationFn: async (locationId: string) => {
      return putApiCompaniesByIdLocation({
        path: { id: companyId },
        body: {
          id: companyId,
          locationId,
          name: editLocation.name,
          regionId: editLocation.regionId,
          countryId: editLocation.countryId,
          isHeadOffice: editLocation.isHeadOffice
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

  const addContactMutation = useMutation({
    mutationFn: async () => {
        // PostApiCompaniesByIdContactData
      return postApiCompaniesByIdContact({
        path: { id: companyId },
        body: {
            id: companyId,
            firstName: newContact.firstName,
            lastName: newContact.lastName,
            emailAddress: newContact.email,
            companyPosition: newContact.position
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
       return deleteApiCompaniesByIdContact({ path: { id: companyId }, query: { contactId } });
    },
    onSuccess: () => {
        refreshData();
    }
  });

  const [editingContactId, setEditingContactId] = useState<string | null>(null);
  const [editContact, setEditContact] = useState({ firstName: "", lastName: "", email: "", position: "" });

  const updateContactMutation = useMutation({
    mutationFn: async (contactId: string) => {
      return putApiCompaniesByIdContact({
        path: { id: companyId },
        body: {
          id: companyId,
          contactId,
          firstName: editContact.firstName,
          lastName: editContact.lastName,
          emailAddress: editContact.email,
          companyPosition: editContact.position
        }
      });
    },
    onSuccess: () => {
      setEditingContactId(null);
      setEditContact({ firstName: "", lastName: "", email: "", position: "" });
      refreshData();
    }
  });

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
                    value={field.state.value} 
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
                        value={field.state.value} 
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
                        value={field.state.value} 
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
                      value={field.state.value} 
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
                      value={field.state.value} 
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
                                  {allRegions.map((r: any) => <SelectItem key={r.id} value={r.id!}>{r.name}</SelectItem>)}
                                </SelectContent>
                              </Select>
                            </div>
                            <div className="space-y-2">
                              <Label>Country</Label>
                              <Select value={editLocation.countryId} onValueChange={(v: string) => setEditLocation({...editLocation, countryId: v})}>
                                <SelectTrigger><SelectValue placeholder="Select Country" /></SelectTrigger>
                                <SelectContent>
                                  {allCountries
                                    .filter((c: any) => !editLocation.regionId || c.regionId === editLocation.regionId)
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
                                {allCountries.find(c => c.id === loc.countryId)?.name}, {allRegions.find(r => r.id === loc.regionId)?.name}
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
                                        {allRegions.map((r: any) => <SelectItem key={r.id} value={r.id!}>{r.name}</SelectItem>)}
                                    </SelectContent>
                                </Select>
                            </div>
                             <div className="space-y-2">
                                <Label>Country</Label>
                                <Select value={newLocation.countryId} onValueChange={(v: string) => setNewLocation({...newLocation, countryId: v})}>
                                    <SelectTrigger><SelectValue placeholder="Select Country" /></SelectTrigger>
                                    <SelectContent>
                                        {allCountries
                                            .filter((c: any) => !newLocation.regionId || c.regionId === newLocation.regionId)
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

        </div>

        {/* Right Column: Tags & Metadata */}
        <div className="space-y-12">
             {/* Core Skills */}
            <section className="space-y-6">
                <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
                    <Briefcase className="w-4 h-4" /> Core Skills
                </h3>
                <div className="p-6 bg-white border border-gray-200 shadow-sm">
                    <div className="flex flex-wrap gap-2 mb-4">
                        {allCapabilities.map((cap: any) => {
                            const isSelected = selectedCapabilityIds.includes(cap.id!);
                            return (
                                <button
                                    key={cap.id}
                                    onClick={() => toggleCapability(cap.id!)}
                                    className={`px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all ${
                                        isSelected 
                                        ? "bg-black text-white border-black" 
                                        : "bg-gray-50 text-gray-400 border-gray-200 hover:border-gray-400 hover:text-gray-600"
                                    }`}
                                >
                                    {cap.name}
                                </button>
                            );
                        })}
                    </div>
                    <p className="text-xs text-gray-400 italic">
                        Select all that apply to your firm.
                    </p>
                </div>
            </section>

             {/* Industries */}
            <section className="space-y-6">
                <h3 className="text-lg font-bold uppercase tracking-widest border-b border-gray-100 pb-2 flex items-center gap-2">
                     <Building className="w-4 h-4" /> Primary Industries
                </h3>
                  <div className="p-6 bg-white border border-gray-200 shadow-sm">
                    <div className="flex flex-wrap gap-2 mb-4">
                        {allIndustries.map((ind: any) => {
                            const isSelected = selectedIndustryIds.includes(ind.id!);
                            return (
                                <button
                                    key={ind.id}
                                    onClick={() => toggleIndustry(ind.id!)}
                                    className={`px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border transition-all ${
                                        isSelected 
                                        ? "bg-red-600 text-white border-red-600" 
                                        : "bg-gray-50 text-gray-400 border-gray-200 hover:border-gray-400 hover:text-gray-600"
                                    }`}
                                >
                                    {ind.name}
                                </button>
                            );
                        })}
                    </div>
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
