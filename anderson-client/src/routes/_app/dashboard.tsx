import { createFileRoute } from "@tanstack/react-router";
import { ArrowUpRight, Briefcase, Globe, Users } from "lucide-react";
import {
  Bar,
  BarChart,
  CartesianGrid,
  Cell,
  Pie,
  PieChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { MOCK_OPPORTUNITIES, MOCK_PARTNERS } from "../../services/mock/data";

export const Route = createFileRoute("/_app/dashboard")({
  component: Dashboard,
});

function Dashboard() {
  // Aggregate partners by service type
  const serviceTypeCounts = MOCK_PARTNERS.reduce(
    (acc, partner) => {
      const type = partner.serviceType;
      acc[type] = (acc[type] || 0) + 1;
      return acc;
    },
    {} as Record<string, number>
  );

  const serviceTypeData = Object.keys(serviceTypeCounts)
    .map((name) => ({
      name,
      value: serviceTypeCounts[name],
    }))
    .sort((a, b) => b.value - a.value);

  const regionData = [
    { name: "Europe", value: 40 },
    { name: "Africa", value: 25 },
    { name: "North America", value: 20 },
    { name: "APAC", value: 15 },
  ];

  const COLORS = ["#DB0A20", "#111111", "#555555", "#999999", "#CCCCCC"];

  return (
    <div className="animate-fade-in space-y-12">
      <header className="mb-12 border-gray-200 border-b pb-6">
        <h2 className="mb-3 font-serif text-4xl text-black">
          Network Overview
        </h2>
        <p className="font-light text-gray-500 text-lg">
          Welcome back. Here is the pulse of the global network.
        </p>
      </header>

      {/* KPI Cards */}
      <div className="grid grid-cols-1 gap-8 md:grid-cols-3">
        <div className="group border border-gray-200 bg-white p-8 shadow-sm transition-shadow hover:shadow-md">
          <div className="flex items-start justify-between">
            <div>
              <p className="mb-1 font-bold text-gray-400 text-xs uppercase tracking-widest">
                Total Partners
              </p>
              <h3 className="mt-2 font-serif text-5xl text-black">
                {MOCK_PARTNERS.length}
              </h3>
            </div>
            <div className="p-0">
              <Users className="h-6 w-6 text-gray-300 transition-colors group-hover:text-red-600" />
            </div>
          </div>
          <div className="mt-6 flex items-center font-medium text-green-700 text-xs uppercase tracking-wide">
            <ArrowUpRight className="mr-1 h-3 w-3" />
            <span>12% growth this quarter</span>
          </div>
        </div>

        <div className="group border border-gray-200 bg-white p-8 shadow-sm transition-shadow hover:shadow-md">
          <div className="flex items-start justify-between">
            <div>
              <p className="mb-1 font-bold text-gray-400 text-xs uppercase tracking-widest">
                Active Opportunities
              </p>
              <h3 className="mt-2 font-serif text-5xl text-black">
                {MOCK_OPPORTUNITIES.length}
              </h3>
            </div>
            <div className="p-0">
              <Briefcase className="h-6 w-6 text-gray-300 transition-colors group-hover:text-red-600" />
            </div>
          </div>
          <div className="mt-6 flex items-center font-medium text-gray-500 text-xs uppercase tracking-wide">
            <span>2 urgent deadlines this week</span>
          </div>
        </div>

        <div className="group border border-gray-200 bg-white p-8 shadow-sm transition-shadow hover:shadow-md">
          <div className="flex items-start justify-between">
            <div>
              <p className="mb-1 font-bold text-gray-400 text-xs uppercase tracking-widest">
                Global Reach
              </p>
              <h3 className="mt-2 font-serif text-5xl text-black">18</h3>
            </div>
            <div className="p-0">
              <Globe className="h-6 w-6 text-gray-300 transition-colors group-hover:text-red-600" />
            </div>
          </div>
          <div className="mt-6 flex items-center font-medium text-gray-500 text-xs uppercase tracking-wide">
            <span>Countries represented</span>
          </div>
        </div>
      </div>

      {/* Charts Section */}
      <div className="grid grid-cols-1 gap-8 lg:grid-cols-2">
        {/* Service Type Distribution */}
        <div className="border border-gray-200 bg-white p-8 shadow-sm">
          <h3 className="mb-8 border-gray-100 border-b pb-4 font-serif text-black text-xl">
            Partners by Service Type
          </h3>
          <div className="h-72 w-full">
            <ResponsiveContainer height="100%" width="100%">
              <BarChart
                data={serviceTypeData}
                layout="vertical"
                margin={{ top: 5, right: 30, left: 60, bottom: 5 }}
              >
                <CartesianGrid
                  horizontal={false}
                  stroke="#e5e5e5"
                  strokeDasharray="3 3"
                />
                <XAxis hide type="number" />
                <YAxis
                  axisLine={false}
                  dataKey="name"
                  interval={0}
                  tick={{ fontSize: 10, fontFamily: "Inter", fill: "#555" }}
                  tickLine={false}
                  type="category"
                  width={140}
                />
                <Tooltip
                  contentStyle={{
                    backgroundColor: "#111",
                    color: "#fff",
                    border: "none",
                    borderRadius: "0px",
                    padding: "8px 12px",
                  }}
                  cursor={{ fill: "#f9f9f9" }}
                  itemStyle={{ color: "#fff", fontSize: "12px" }}
                />
                <Bar
                  barSize={16}
                  dataKey="value"
                  fill="#DB0A20"
                  radius={[0, 0, 0, 0]}
                />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Regional Coverage */}
        <div className="border border-gray-200 bg-white p-8 shadow-sm">
          <h3 className="mb-8 border-gray-100 border-b pb-4 font-serif text-black text-xl">
            Partner Distribution
          </h3>
          <div className="flex h-72 w-full justify-center">
            <ResponsiveContainer height="100%" width="100%">
              <PieChart>
                <Pie
                  cx="50%"
                  cy="50%"
                  data={regionData}
                  dataKey="value"
                  fill="#8884d8"
                  innerRadius={70}
                  outerRadius={90}
                  paddingAngle={2}
                  stroke="none"
                >
                  {regionData.map((entry, index) => (
                    <Cell
                      fill={COLORS[index % COLORS.length]}
                      key={`cell-${index}`}
                    />
                  ))}
                </Pie>
                <Tooltip
                  contentStyle={{
                    backgroundColor: "#111",
                    color: "#fff",
                    border: "none",
                    borderRadius: "0px",
                    padding: "8px 12px",
                  }}
                  itemStyle={{ color: "#fff", fontSize: "12px" }}
                />
              </PieChart>
            </ResponsiveContainer>
          </div>
          <div className="mt-4 flex flex-wrap justify-center gap-6">
            {regionData.map((entry, index) => (
              <div
                className="flex items-center font-medium text-gray-500 text-xs uppercase tracking-wide"
                key={index}
              >
                <span
                  className="mr-2 h-3 w-3"
                  style={{ backgroundColor: COLORS[index] }}
                />
                {entry.name}
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
