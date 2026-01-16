import { createFileRoute } from "@tanstack/react-router";
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import { ArrowUpRight, Users, Globe, Briefcase } from 'lucide-react';
import React from "react";
import { MOCK_PARTNERS, MOCK_OPPORTUNITIES } from "../../services/mock/data";

export const Route = createFileRoute("/_app/dashboard")({
  component: Dashboard,
});

function Dashboard() {
  // Aggregate partners by service type
  const serviceTypeCounts = MOCK_PARTNERS.reduce((acc, partner) => {
    const type = partner.serviceType;
    acc[type] = (acc[type] || 0) + 1;
    return acc;
  }, {} as Record<string, number>);

  const serviceTypeData = Object.keys(serviceTypeCounts)
    .map(name => ({
      name,
      value: serviceTypeCounts[name]
    }))
    .sort((a, b) => b.value - a.value);

  const regionData = [
    { name: 'Europe', value: 40 },
    { name: 'Africa', value: 25 },
    { name: 'North America', value: 20 },
    { name: 'APAC', value: 15 },
  ];

  const COLORS = ['#DB0A20', '#111111', '#555555', '#999999', '#CCCCCC'];

  return (
    <div className="space-y-12 animate-fade-in">
      <header className="mb-12 border-b border-gray-200 pb-6">
        <h2 className="text-4xl font-serif text-black mb-3">Network Overview</h2>
        <p className="text-gray-500 font-light text-lg">Welcome back. Here is the pulse of the global network.</p>
      </header>

      {/* KPI Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
        <div className="bg-white p-8 border border-gray-200 shadow-sm hover:shadow-md transition-shadow group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-1">Total Partners</p>
              <h3 className="text-5xl font-serif text-black mt-2">{MOCK_PARTNERS.length}</h3>
            </div>
            <div className="p-0">
              <Users className="w-6 h-6 text-gray-300 group-hover:text-red-600 transition-colors" />
            </div>
          </div>
          <div className="mt-6 flex items-center text-xs font-medium text-green-700 uppercase tracking-wide">
            <ArrowUpRight className="w-3 h-3 mr-1" />
            <span>12% growth this quarter</span>
          </div>
        </div>

        <div className="bg-white p-8 border border-gray-200 shadow-sm hover:shadow-md transition-shadow group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-1">Active Opportunities</p>
              <h3 className="text-5xl font-serif text-black mt-2">{MOCK_OPPORTUNITIES.length}</h3>
            </div>
            <div className="p-0">
              <Briefcase className="w-6 h-6 text-gray-300 group-hover:text-red-600 transition-colors" />
            </div>
          </div>
          <div className="mt-6 flex items-center text-xs font-medium text-gray-500 uppercase tracking-wide">
            <span>2 urgent deadlines this week</span>
          </div>
        </div>

        <div className="bg-white p-8 border border-gray-200 shadow-sm hover:shadow-md transition-shadow group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-1">Global Reach</p>
              <h3 className="text-5xl font-serif text-black mt-2">18</h3>
            </div>
            <div className="p-0">
              <Globe className="w-6 h-6 text-gray-300 group-hover:text-red-600 transition-colors" />
            </div>
          </div>
          <div className="mt-6 flex items-center text-xs font-medium text-gray-500 uppercase tracking-wide">
            <span>Countries represented</span>
          </div>
        </div>
      </div>

      {/* Charts Section */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        
        {/* Service Type Distribution */}
        <div className="bg-white p-8 border border-gray-200 shadow-sm">
          <h3 className="text-xl font-serif text-black mb-8 border-b border-gray-100 pb-4">Partners by Service Type</h3>
          <div className="h-72 w-full">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={serviceTypeData} layout="vertical" margin={{ top: 5, right: 30, left: 60, bottom: 5 }}>
                <CartesianGrid strokeDasharray="3 3" horizontal={false} stroke="#e5e5e5" />
                <XAxis type="number" hide />
                <YAxis dataKey="name" type="category" width={140} tick={{fontSize: 10, fontFamily: 'Inter', fill: '#555'}} interval={0} tickLine={false} axisLine={false} />
                <Tooltip 
                  contentStyle={{ backgroundColor: '#111', color: '#fff', border: 'none', borderRadius: '0px', padding: '8px 12px' }}
                  itemStyle={{ color: '#fff', fontSize: '12px' }}
                  cursor={{fill: '#f9f9f9'}}
                />
                <Bar dataKey="value" fill="#DB0A20" barSize={16} radius={[0, 0, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Regional Coverage */}
        <div className="bg-white p-8 border border-gray-200 shadow-sm">
          <h3 className="text-xl font-serif text-black mb-8 border-b border-gray-100 pb-4">Partner Distribution</h3>
          <div className="h-72 w-full flex justify-center">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={regionData}
                  cx="50%"
                  cy="50%"
                  innerRadius={70}
                  outerRadius={90}
                  fill="#8884d8"
                  paddingAngle={2}
                  dataKey="value"
                  stroke="none"
                >
                  {regionData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip 
                  contentStyle={{ backgroundColor: '#111', color: '#fff', border: 'none', borderRadius: '0px', padding: '8px 12px' }}
                  itemStyle={{ color: '#fff', fontSize: '12px' }}
                />
              </PieChart>
            </ResponsiveContainer>
          </div>
          <div className="flex flex-wrap justify-center gap-6 mt-4">
             {regionData.map((entry, index) => (
               <div key={index} className="flex items-center text-xs uppercase tracking-wide text-gray-500 font-medium">
                 <span className="w-3 h-3 mr-2" style={{backgroundColor: COLORS[index]}}></span>
                 {entry.name}
               </div>
             ))}
          </div>
        </div>
      </div>
    </div>
  );
}
