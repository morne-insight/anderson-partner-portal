import { Link, useRouterState } from "@tanstack/react-router";
import {
  FileText,
  Globe,
  LayoutDashboard,
  LogOut,
  Search,
  ShieldCheck,
  Users,
} from "lucide-react";
import type React from "react";
import { useAuth } from "../../contexts/auth-context";

interface SidebarProps {
  isAdmin?: boolean;
}

export const Sidebar: React.FC<SidebarProps> = ({ isAdmin }) => {
  const { user, logout } = useAuth();
  const routerState = useRouterState();
  const currentPath = routerState.location.pathname;

  const navItems = [
    { to: "/dashboard", label: "Overview", icon: LayoutDashboard, exact: true },
    { to: "/partners", label: "Find Partners", icon: Search },
    { to: "/opportunities", label: "Opportunities", icon: Globe },
    { to: "/profile", label: "My Profile", icon: FileText },
    { to: "/directory", label: "Network Directory", icon: Users },
    // { to: "/dev", label: "Dev", icon: Users },
  ];

  if (isAdmin) {
    navItems.push({ to: "/admin", label: "Back Office", icon: ShieldCheck });
  }

  const isActive = (path: string, exact = false) => {
    if (exact) return currentPath === path;
    return currentPath.startsWith(path);
  };

  return (
    <aside className="z-20 hidden h-full w-72 flex-col bg-black text-white md:flex">
      <div className="border-gray-800 border-b p-8">
        <h1 className="font-bold font-serif text-2xl text-white tracking-wider">
          ANDERSEN<span className="text-red-600">.</span>
        </h1>
        <p className="mt-2 text-[10px] text-gray-400 uppercase tracking-[0.2em]">
          Partner Portal
        </p>
      </div>

      <nav className="flex-1 overflow-y-auto py-8">
        <ul className="space-y-1">
          {navItems.map((item) => (
            <li key={item.to}>
              <Link
                className={`group flex w-full items-center border-l-2 px-8 py-4 font-medium text-sm transition-all duration-200 ${isActive(item.to, item.exact)
                    ? "border-red-600 bg-white/5 text-white"
                    : "border-transparent text-gray-400 hover:border-gray-700 hover:bg-white/5 hover:text-white"
                  }`}
                to={item.to}
              >
                <item.icon
                  className={`mr-4 h-4 w-4 transition-colors ${isActive(item.to, item.exact)
                      ? item.to === "/admin"
                        ? "text-yellow-400"
                        : "text-red-600"
                      : "group-hover:text-white"
                    }`}
                />
                <span
                  className={`text-xs uppercase tracking-widest ${item.to === "/admin" && isActive(item.to)
                      ? "text-yellow-400"
                      : ""
                    }`}
                >
                  {item.label}
                </span>
              </Link>
            </li>
          ))}
        </ul>
      </nav>

      <div className="border-gray-800 border-t p-8">
        {user && (
          <div className="flex flex-col gap-6">
            <div className="flex items-center gap-3">
              <div className="flex h-8 w-8 items-center justify-center rounded-full bg-red-600 font-bold text-white text-xs ring-2 ring-black">
                {user.email?.[0]?.toUpperCase()}
              </div>
              <div className="flex flex-col overflow-hidden">
                <span
                  className="w-40 truncate font-medium text-sm text-white"
                  title={user.email}
                >
                  {user.email}
                </span>
                <span className="font-semibold text-[10px] text-gray-500 uppercase tracking-widest">
                  Partner
                </span>
              </div>
            </div>
            <button
              className="group flex items-center gap-3 font-bold text-gray-400 text-xs uppercase tracking-widest transition-colors hover:text-red-500"
              onClick={() => logout()}
            >
              <LogOut className="h-4 w-4 transition-transform group-hover:-translate-x-1" />
              Sign Out
            </button>
          </div>
        )}
      </div>
    </aside>
  );
};
