import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { User, MapPin, Briefcase, Search } from "lucide-react";
import React, { useState } from "react";
import { MOCK_PARTNERS } from "../../services/mock/data";

export const Route = createFileRoute("/_app/directory")({
  component: NetworkDirectory,
});

function NetworkDirectory() {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState("");

  const filteredPartners = MOCK_PARTNERS.filter(p => 
    p.name.toLowerCase().includes(searchTerm.toLowerCase()) || 
    p.locations.some(l => l.country.toLowerCase().includes(searchTerm.toLowerCase()))
  );

  return (
    <div className="space-y-10 animate-fade-in">
       <header className="border-b border-gray-200 pb-6 flex justify-between items-end">
        <div>
          <h2 className="text-4xl font-serif text-black mb-3">Network Directory</h2>
          <p className="text-gray-500 font-light text-lg">
            Complete index of all member firms.
          </p>
        </div>
      </header>
      
      <div className="bg-white border border-gray-200 shadow-sm">
          {/* Toolbar */}
          <div className="p-4 border-b border-gray-100 flex items-center gap-4 bg-gray-50/50">
             <div className="relative flex-1 max-w-md">
                 <Search className="absolute top-2.5 left-3 w-4 h-4 text-gray-400"/>
                 <input 
                    type="text" 
                    placeholder="Filter by firm name or country..." 
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 text-sm focus:border-black outline-none"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
             </div>
          </div>

          <div className="overflow-x-auto">
              <table className="w-full text-left">
                  <thead className="bg-gray-50 text-[10px] font-bold uppercase tracking-widest text-gray-500">
                      <tr>
                          <th className="px-6 py-4">Firm Name</th>
                          <th className="px-6 py-4">Service Line</th>
                          <th className="px-6 py-4">Head Office</th>
                          <th className="px-6 py-4">Liaison</th>
                          <th className="px-6 py-4 text-right">Action</th>
                      </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-100">
                      {filteredPartners.map(partner => {
                          const headOffice = partner.locations.find(l => l.isHeadOffice) || partner.locations[0];
                          const contact = partner.contacts.find(c => c.isDefault) || partner.contacts[0];

                          return (
                              <tr key={partner.id} className="hover:bg-gray-50 transition-colors">
                                  <td className="px-6 py-4">
                                      <span className="font-serif text-lg text-black">{partner.name}</span>
                                  </td>
                                  <td className="px-6 py-4">
                                      <span className="flex items-center text-xs font-medium text-gray-600 uppercase tracking-wide">
                                        <Briefcase className="w-3 h-3 mr-2" /> {partner.serviceType}
                                      </span>
                                  </td>
                                  <td className="px-6 py-4">
                                      <span className="flex items-center text-xs text-gray-500">
                                        <MapPin className="w-3 h-3 mr-2 text-gray-400" /> {headOffice?.country}
                                      </span>
                                  </td>
                                  <td className="px-6 py-4">
                                      <span className="flex items-center text-xs text-gray-500">
                                        <User className="w-3 h-3 mr-2 text-gray-400" /> {contact?.name}
                                      </span>
                                  </td>
                                  <td className="px-6 py-4 text-right">
                                      <button
                                          onClick={() => navigate({ to: `/partners/${partner.id}` })}
                                          className="text-red-600 hover:text-black text-[10px] font-bold uppercase tracking-widest"
                                      >
                                          View Profile
                                      </button>
                                  </td>
                              </tr>
                          )
                      })}
                  </tbody>
              </table>
          </div>
      </div>
    </div>
  );
}
