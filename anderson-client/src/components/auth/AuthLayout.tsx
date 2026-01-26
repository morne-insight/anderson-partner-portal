import { ReactNode } from "react";
import logo from "../../logo.svg";

interface AuthLayoutProps {
    children: ReactNode;
    title: string;
    subtitle: string;
}

export function AuthLayout({ children, title, subtitle }: AuthLayoutProps) {
    return (
        <div className="flex min-h-screen w-full bg-[#F9FAFB] font-sans">
            {/* Left Panel - Branding (Hidden on mobile) */}
            <div className="hidden lg:flex w-1/2 flex-col justify-between bg-[#111111] p-12 text-white relative overflow-hidden">
                {/* Abstract Background Elements */}
                <div className="absolute top-0 right-0 w-[500px] h-[500px] bg-[#DB0A20] rounded-full blur-[120px] opacity-10 translate-x-1/3 -translate-y-1/3"></div>
                <div className="absolute bottom-0 left-0 w-[500px] h-[500px] bg-[#555555] rounded-full blur-[100px] opacity-10 -translate-x-1/3 translate-y-1/3"></div>

                {/* Content */}
                <div className="relative z-10">
                    <img src={logo} alt="Anderson Logo" className="h-8 w-auto opacity-90" />
                </div>

                <div className="relative z-10 max-w-lg">
                    <h1 className="text-5xl font-medium leading-tight text-white mb-6" style={{ fontFamily: '"Playfair Display", serif' }}>
                        Experience the Future of Business Insight
                    </h1>
                    <p className="text-lg text-[#9CA3AF] font-light leading-relaxed">
                        Anderson provides the tools you need to analyze, understand, and grow your business with confidence.
                    </p>
                </div>

                <div className="relative z-10 flex items-center gap-6 text-sm text-[#6B7280]">
                    <span>© {new Date().getFullYear()} Anderson Inc.</span>
                    <a href="#" className="hover:text-white transition-colors">Privacy Policy</a>
                    <a href="#" className="hover:text-white transition-colors">Terms of Service</a>
                </div>
            </div>

            {/* Right Panel - Form */}
            <div className="flex w-full lg:w-1/2 flex-col justify-center items-center p-8 lg:p-12 relative">
                {/* Mobile Logo */}
                <div className="lg:hidden absolute top-8 left-8">
                    <img src={logo} alt="Anderson Logo" className="h-8 w-auto" />
                </div>

                <div className="w-full max-w-[400px] space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
                    <div className="text-center lg:text-left space-y-2">
                        <h2 className="text-3xl font-semibold text-[#111111]" style={{ fontFamily: '"Playfair Display", serif' }}>
                            {title}
                        </h2>
                        <p className="text-[#6B7280]">
                            {subtitle}
                        </p>
                    </div>

                    {children}
                </div>
            </div>
        </div>
    );
}
