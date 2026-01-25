import { createFileRoute, Link } from "@tanstack/react-router";
import { ArrowLeft, MapPin, Globe, CheckCircle, Building, Briefcase, Users } from "lucide-react";
import { callApi } from "@/server/proxy";
import { ReviewComponent } from "@/components/Reviews/ReviewComponent";

export const Route = createFileRoute("/_app/partners/$id")({
  component: PartnerProfile,
  validateSearch: (search: Record<string, unknown>) => ({
    from: search.from as string | undefined,
  }),
  loader: async ({ params }) => {
    const [partner, countries, regions] = await Promise.all([
      callApi({ data: { fn: 'getApiCompaniesByIdPartner', args: { path: { id: params.id } } } }),
      callApi({ data: { fn: 'getApiCountries' } }),
      callApi({ data: { fn: 'getApiRegions' } }),
    ]);

    if (!partner) return { partner: null };

    // Transform PartnerProfile to Partner interface
    const transformedPartner = {
      id: partner.id,
      name: partner.name,
      website: partner.websiteUrl || "",
      description: partner.fullDescription || "",
      serviceType: partner.serviceTypes?.[0]?.name || "Partner",
      skills: partner.capabilities?.map((c: any) => c.name) || [],
      industries: partner.industries?.map((i: any) => i.name) || [],
      certifications: [], // Not in API currently
      verified: true,
      contacts: partner.contacts?.map((c: any) => ({
        name: `${c.firstName} ${c.lastName}`,
        email: c.emailAddress,
        isDefault: true // Mocking default
      })) || [],
      locations: partner.locations?.map((l: any) => ({
        country: countries.find((c: any) => c.id === l.countryId)?.name || "Unknown",
        region: regions.find((r: any) => r.id === l.regionId)?.name || "Unknown",
        isHeadOffice: l.isHeadOffice
      })) || [],
      users: partner.contacts?.map((c: any) => ({
        email: c.emailAddress,
        firstName: c.firstName,
        lastName: c.lastName,
        title: c.companyPosition || "Representative"
      })) || [],
      reviews: partner.reviews?.map((r: any) => ({
        rating: r.rating || 5,
        comment: r.comment || "",
        date: "Recently", // Mock date format
        reviewerName: "Verified Client"
      })) || []
    };

    return {
      partner: transformedPartner,
    };
  },
});

function PartnerProfile() {
  const { partner } = Route.useLoaderData();
  const search = Route.useSearch();
  const fromDirectory = search?.from === 'directory';

  if (!partner) {
    return (
      <div className="p-12 text-center">
        <h2 className="text-2xl font-serif text-black">Partner not found</h2>
        <Link to="/partners" className="text-red-600 hover:underline mt-4 block">Return to Directory</Link>
      </div>
    )
  }

  const headOffice = partner.locations.find((l: any) => l.isHeadOffice) || partner.locations[0];

  return (
    <div className="space-y-8 animate-fade-in">
      <nav className="flex items-center text-xs text-gray-500 mb-8 uppercase tracking-wider font-medium">
        <Link
          to={fromDirectory ? "/directory" : "/partners"}
          className="flex items-center hover:text-red-600 transition-colors"
        >
          <ArrowLeft className="w-3 h-3 mr-2" />
          {fromDirectory ? "Back to Directory" : "Back to Search"}
        </Link>
        <span className="mx-3 text-gray-300">/</span>
        <span className="text-gray-900">{partner.name}</span>
      </nav>

      {/* Hero Section */}
      <div className="bg-white border-t-4 border-t-red-600 border-x border-b border-gray-200 shadow-sm">
        <div className="p-10">
          <div className="flex flex-col md:flex-row justify-between items-start gap-8">
            <div>
              <div className="flex items-center gap-3 mb-4">
                <h1 className="text-5xl font-serif text-black leading-tight">{partner.name}</h1>
                {partner.verified && (
                  <span title="Verified Partner" className="mt-2">
                    <CheckCircle className="w-6 h-6 text-gray-400" />
                  </span>
                )}
              </div>
              <div className="flex flex-wrap items-center gap-4">
                <div className="flex items-center text-gray-500 text-sm font-medium uppercase tracking-wider">
                  <MapPin className="w-4 h-4 mr-2 text-red-600" />
                  {headOffice?.country} <span className="mx-2 text-gray-300">|</span> {headOffice?.region}
                  <span className="ml-3 bg-gray-100 text-[10px] px-2 py-0.5 border border-gray-200 text-gray-400 font-bold uppercase tracking-widest">
                    Head Office
                  </span>
                </div>
                <div className="flex items-center bg-black text-white text-[10px] px-3 py-1 font-bold uppercase tracking-[0.2em]">
                  <Briefcase className="w-3 h-3 mr-2" />
                  {partner.serviceType}
                </div>
              </div>
            </div>

            <div className="flex flex-col sm:flex-row gap-4 w-full md:w-auto">
              <button
                onClick={() => { }}
                className="px-10 py-3 bg-red-600 text-white text-xs font-bold uppercase tracking-[0.15em] hover:bg-white hover:text-red-600 border border-red-600 transition-colors min-w-[160px]"
              >
                Connect
              </button>
            </div>
          </div>

          <p className="mt-8 text-xl text-gray-600 leading-relaxed max-w-5xl font-light border-l-2 border-gray-200 pl-6">
            {partner.description}
          </p>

          <div className="mt-10 flex flex-wrap gap-8 pt-8 border-t border-gray-100">
            <a
              href={partner.website}
              target="_blank"
              rel="noopener noreferrer"
              className="flex items-center text-xs font-bold uppercase tracking-wider text-red-600 hover:underline"
            >
              <Globe className="w-4 h-4 mr-2" />
              Visit Website
            </a>
            <div className="flex items-center text-xs font-bold uppercase tracking-wider text-gray-500">
              <Building className="w-4 h-4 mr-2" />
              Global Presence: {partner.locations.length} Locations
            </div>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Main Info */}
        <div className="lg:col-span-3 space-y-8">
          {/* Team Section */}
          <div className="bg-white p-8 border border-gray-200 shadow-sm">
            <h3 className="text-xl font-serif text-black mb-6 flex items-center border-b border-gray-100 pb-4">
              <Users className="w-5 h-5 mr-3 text-red-600" />
              Key Personnel
            </h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {partner.users.map((user: any) => (
                <div
                  key={user.email}
                  className="flex items-center gap-4 p-4 border border-gray-50 bg-gray-50/50"
                >
                  <div className="w-12 h-12 bg-black text-white flex items-center justify-center font-serif text-lg">
                    {user.firstName.charAt(0)}
                    {user.lastName.charAt(0)}
                  </div>
                  <div>
                    <p className="font-bold text-black text-sm">
                      {user.firstName} {user.lastName}
                    </p>
                    <p className="text-[10px] text-gray-400 uppercase tracking-widest">{user.title}</p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Skills & Industries */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            <div className="bg-white p-8 border border-gray-200 shadow-sm">
              <h3 className="text-xl font-serif text-black mb-6 border-b border-gray-100 pb-4">
                Capabilities
              </h3>
              <div className="flex flex-wrap gap-2">
                {partner.skills.map((skill: string) => (
                  <span
                    key={skill}
                    className="px-3 py-1 bg-gray-50 text-gray-600 text-[10px] font-bold border border-gray-100 uppercase tracking-widest"
                  >
                    {skill}
                  </span>
                ))}
              </div>
            </div>
            <div className="bg-white p-8 border border-gray-200 shadow-sm">
              <h3 className="text-xl font-serif text-black mb-6 border-b border-gray-100 pb-4">
                Industries
              </h3>
              <div className="flex flex-wrap gap-2">
                {partner.industries.map((ind: string) => (
                  <span
                    key={ind}
                    className="px-3 py-1 bg-black text-white text-[10px] font-bold uppercase tracking-widest"
                  >
                    {ind}
                  </span>
                ))}
              </div>
            </div>
          </div>

          {/* Locations */}
          <div className="bg-white p-8 border border-gray-200 shadow-sm">
            <h3 className="text-xl font-serif text-black mb-6 border-b border-gray-100 pb-4">
              Office Locations
            </h3>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              {partner.locations
                .sort((a: any, b: any) => (a.isHeadOffice ? -1 : 1))
                .map((loc: any, i: number) => (
                  <div
                    key={i}
                    className={`p-4 border ${loc.isHeadOffice ? "border-red-600 bg-red-50/20" : "border-gray-100"
                      }`}
                  >
                    <div className="flex justify-between items-start">
                      <p className="text-sm font-bold text-black">{loc.country}</p>
                      {loc.isHeadOffice && (
                        <span className="text-[9px] bg-red-600 text-white px-2 py-0.5 font-bold uppercase tracking-tighter">
                          Primary
                        </span>
                      )}
                    </div>
                    <p className="text-[10px] text-gray-400 uppercase tracking-widest mt-1">
                      {loc.region}
                    </p>
                  </div>
                ))}
            </div>
          </div>

          {/* Reviews */}
          <ReviewComponent partnerId={partner.id} />
        </div>

        {/* Sidebar Info */}
        <div className="space-y-8">
          {/* Contact Directory and Accreditations sections hidden */}
        </div>
      </div>
    </div>
  );
}
