import type { ReactNode } from "react";
import logo from "../../logo.png";

interface AuthLayoutProps {
  children: ReactNode;
  title: string;
  subtitle: string;
}

export function AuthLayout({ children, title, subtitle }: AuthLayoutProps) {
  return (
    <div className="flex min-h-screen w-full bg-[#F9FAFB] font-sans">
      {/* Left Panel - Branding (Hidden on mobile) */}
      <div className="relative hidden w-1/2 flex-col justify-between overflow-hidden bg-[#111111] p-12 text-white lg:flex">
        {/* Abstract Background Elements */}
        <div className="absolute top-0 right-0 h-[500px] w-[500px] translate-x-1/3 -translate-y-1/3 rounded-full bg-[#DB0A20] opacity-10 blur-[120px]" />
        <div className="absolute bottom-0 left-0 h-[500px] w-[500px] -translate-x-1/3 translate-y-1/3 rounded-full bg-[#555555] opacity-10 blur-[100px]" />

        {/* Content */}
        <div className="relative z-10">
          <img
            alt="Anderson Logo"
            className="h-8 w-auto opacity-90"
            src={logo}
          />
        </div>

        <div className="relative z-10 max-w-lg">
          <h1
            className="mb-6 font-medium text-5xl text-white leading-tight"
            style={{ fontFamily: '"Playfair Display", serif' }}
          >
            Connecting Andersen Firms. Enabling Better Outcomes.
          </h1>
          <p className="font-light text-[#9CA3AF] text-md leading-relaxed">
            The Andersen Partner Portal is your gateway to collaboration across the Andersen network.
          </p>
          <p className="font-light text-[#9CA3AF] text-md leading-relaxed">
            &nbsp;
          </p>
          <p className="font-light text-[#9CA3AF] text-md leading-relaxed">
            Search for firms by capability and industry expertise, explore and respond to opportunities, keep your firm profile up to date, and submit required reports, all through a single, secure platform designed to support how Andersen firms work together.
          </p>
        </div>

        <div className="relative z-10 flex items-center gap-6 text-[#6B7280] text-sm">
          <span>© {new Date().getFullYear()} Insight Consulting.</span>
          {/* <a className="transition-colors hover:text-white" href="#">
            Privacy Policy
          </a>
          <a className="transition-colors hover:text-white" href="#">
            Terms of Service
          </a> */}
        </div>
      </div>

      {/* Right Panel - Form */}
      <div className="relative flex w-full flex-col items-center justify-center p-8 lg:w-1/2 lg:p-12">
        {/* Mobile Logo */}
        <div className="absolute top-8 left-8 lg:hidden">
          <img alt="Anderson Logo" className="h-8 w-auto" src={logo} />
        </div>

        <div className="fade-in slide-in-from-bottom-4 w-full max-w-[400px] animate-in space-y-8 duration-700">
          <div className="space-y-2 text-center lg:text-left">
            <h2
              className="font-semibold text-3xl text-[#111111]"
              style={{ fontFamily: '"Playfair Display", serif' }}
            >
              {title}
            </h2>
            <p className="text-[#6B7280]">{subtitle}</p>
          </div>

          {children}
        </div>
      </div>
    </div>
  );
}
