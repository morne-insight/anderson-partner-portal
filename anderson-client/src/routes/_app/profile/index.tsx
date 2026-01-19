import { createFileRoute, Link, useRouter } from "@tanstack/react-router";
import { useMutation } from "@tanstack/react-query";
import { Building2, ChevronRight, Globe, Users, Plus, Loader2 } from "lucide-react";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { callApi } from "@/server/proxy";

type Company = {
  id?: string;
  name?: string;
  shortDescription?: string;
  websiteUrl?: string;
  employeeCount?: number;
};

export const Route = createFileRoute("/_app/profile/")({
  component: ProfileIndex,
  loader: async () => {
    try {
      const response = await callApi({ data: { fn: 'getApiCompaniesMe' } });
      return { companies: (response || []) };
    } catch (error) {
      console.error("Failed to fetch companies", error);
      return { companies: [] };
    }
  },
});

function ProfileIndex() {
  const { companies } = Route.useLoaderData();
  const router = useRouter();
  const [scrapeUrl, setScrapeUrl] = useState("");
  const [newFirmName, setNewFirmName] = useState("");

  const isValidUrl = (url: string): boolean => {
    const urlRegex = /^https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z]{2,}(?:[-a-zA-Z0-9()@:%_+.~#?&/=]*)$/;
    return urlRegex.test(url);
  };

  const refreshData = () => {
    router.invalidate();
  };

  const scrapeMutation = useMutation({
    mutationFn: async (url: string) => {
      return callApi({ data: { fn: 'putApiCompaniesScrapeWebsite', args: { body: { url } } } });
    },
    onSuccess: () => {
      refreshData();
    },
    onError: (err) => {
        console.error("Scrape failed", err);
        alert("Failed to sync website. Please try creating manually.");
    }
  });

  const createMutation = useMutation({
    mutationFn: async () => {
      return callApi({ data: { fn: 'postApiCompanies', args: { body: {
            name: newFirmName || "New Firm",
            shortDescription: "",
            fullDescription: "",
            websiteUrl: "",
            employeeCount: 0,
            serviceTypes: [],
            capabilities: [],
            industries: []
        }  }}});
    },
    onSuccess: (data) => {
      refreshData();
      if (data?.value) {
        router.navigate({ to: "/profile/$companyId", params: { companyId: data.value } });
      }
    },
    onError: (err) => {
        console.error("Create failed", err);
        alert("Failed to create profile.");
    }
  });

  const getHostname = (url: string) => {
    try {
      const normalizedUrl = url.startsWith("http") ? url : `https://${url}`;
      return new URL(normalizedUrl).hostname;
    } catch {
      return url;
    }
  };

  return (
    <div className="space-y-8 animate-fade-in">
      <header className="border-b border-gray-200 pb-6">
        <h2 className="text-4xl font-serif text-black mb-3">My Profile</h2>
        <p className="text-gray-500 font-light text-lg">
          Manage your firm's profile, capabilities, and presence in the Anderson network.
        </p>
      </header>

      <section>
        <div className="flex items-center justify-between mb-6">
          <h3 className="text-xl font-bold uppercase tracking-widest text-black flex items-center gap-2">
            <Building2 className="w-5 h-5" /> Linked Companies
          </h3>
        </div>

        {companies.length === 0 ? (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
             {/* AI Auto-Create */}
            <div className="bg-black text-white p-8 rounded-lg relative overflow-hidden flex flex-col justify-center">
                 <div className="absolute top-0 right-0 p-32 bg-red-600/10 rounded-full blur-3xl transform translate-x-1/2 -translate-y-1/2"></div>
                 <div className="relative z-10 space-y-4">
                    <div className="w-12 h-12 bg-red-600 rounded-full flex items-center justify-center mb-2">
                        <Globe className="w-6 h-6 text-white" />
                    </div>
                    <h3 className="text-2xl font-serif">Auto-Sync from Website</h3>
                    <p className="text-gray-400 text-sm leading-relaxed">
                        Enter your website URL. Our AI will analyze your public presence to create your profile and suggest relevant tags.
                    </p>
                    
                    <div className="space-y-3 pt-4">
                        <Input 
                            value={scrapeUrl}
                            onChange={(e) => setScrapeUrl(e.target.value)}
                            placeholder="https://www.yourcompany.com"
                            className="bg-white/10 border-white/20 text-white placeholder-gray-500 focus:border-red-600"
                        />
                         <Button 
                            onClick={() => scrapeMutation.mutate(scrapeUrl)}
                            disabled={scrapeMutation.isPending || !isValidUrl(scrapeUrl)}
                            className="w-full bg-red-600 hover:bg-red-700 font-bold uppercase tracking-wider py-6"
                        >
                            {scrapeMutation.isPending ? (<><Loader2 className="w-4 h-4 animate-spin mr-2"/> Syncing...</>) : "Sync & Create Profile"}
                        </Button>
                    </div>
                 </div>
            </div>

            {/* Manual Create */}
             <div className="bg-gray-50 border border-gray-200 p-8 rounded-lg flex flex-col justify-center items-center text-center space-y-6">
                 <div className="w-16 h-16 bg-white border border-gray-200 rounded-full flex items-center justify-center shadow-sm">
                    <Plus className="w-8 h-8 text-gray-400" />
                 </div>
                 <div>
                     <h3 className="text-xl font-bold text-gray-900 mb-2">Create Manually</h3>
                     <p className="text-gray-500 text-sm max-w-sm mx-auto mb-4">
                        Start from scratch. Manually enter your firm's details, locations, and personnel to get listed in the directory.
                     </p>
                     <Input 
                        placeholder="Enter Firm Name" 
                        value={newFirmName}
                        onChange={(e: any) => setNewFirmName(e.target.value)}
                        className="max-w-xs mx-auto mb-4 mt-4"
                     />
                 </div>
                  <Button 
                    onClick={() => createMutation.mutate()}
                    disabled={createMutation.isPending || !newFirmName}
                    variant="outline" 
                    className="border-black text-black hover:bg-black hover:text-white uppercase tracking-wider px-8 py-6"
                  >
                    {createMutation.isPending ? "Creating..." : "Create New Profile"}
                  </Button>
            </div>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-6">
            {companies.map((company: Company) => (
              <div
                key={company.id}
                className="bg-white border border-gray-200 p-8 flex flex-col md:flex-row items-center gap-8 hover:shadow-lg transition-all duration-300 group"
              >
                <div className="flex-1">
                  <h3 className="text-2xl font-serif text-black mb-2 flex items-center gap-3">
                     {company.name}
                  </h3>
                  <p className="text-gray-500 text-sm mb-4 line-clamp-2">
                    {company.shortDescription || "No description provided."}
                  </p>
                  
                  <div className="flex flex-wrap gap-6 text-xs text-gray-400 font-medium uppercase tracking-wider">
                    {company.websiteUrl && (
                      <div className="flex items-center gap-2">
                        <Globe className="w-4 h-4" />
                        <a href={company.websiteUrl.startsWith("http") ? company.websiteUrl : `https://${company.websiteUrl}`} target="_blank" rel="noopener noreferrer" className="hover:text-red-600 transition-colors">
                          {getHostname(company.websiteUrl)}
                        </a>
                      </div>
                    )}
                    {company.employeeCount !== undefined && (
                      <div className="flex items-center gap-2">
                        <Users className="w-4 h-4" />
                        <span>{company.employeeCount} Employees</span>
                      </div>
                    )}
                  </div>
                </div>

                <div className="flex-shrink-0">
                  <Link
                    to="/profile/$companyId"
                    params={{ companyId: company.id! }}
                    className="flex items-center gap-2 px-6 py-3 bg-white border border-black text-black text-[10px] font-bold uppercase tracking-[0.2em] hover:bg-black hover:text-white transition-all group-hover:border-red-600 group-hover:bg-red-600 group-hover:text-white"
                  >
                    Manage Firm <ChevronRight className="w-3 h-3" />
                  </Link>
                </div>
              </div>
            ))}
            
            {/* Add another company button - small version for when list exists */}
            <div className="flex justify-center mt-8">
                 <Button 
                    onClick={() => createMutation.mutate()} 
                    variant="ghost" 
                    className="text-gray-400 hover:text-black uppercase text-xs tracking-widest gap-2"
                 >
                    <Plus className="w-4 h-4" /> Add Another Firm
                 </Button>
            </div>
          </div>
        )}
      </section>
    </div>
  );
}
