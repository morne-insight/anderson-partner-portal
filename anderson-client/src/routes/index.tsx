import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/")({ component: App });

function App() {

  return (
    <div className="min-h-screen bg-linear-to-b from-slate-900 via-slate-800 to-slate-900">
      <section className="relative overflow-hidden px-6 py-20 text-center">
        <div className="absolute inset-0 bg-linear-to-r from-cyan-500/10 via-blue-500/10 to-purple-500/10" >
        <div className="relative mx-auto max-w-5xl">
          
            <Link to="/dashboard">Go to Dashboard</Link>
          </div>
        </div>
      </section>
    </div>
  );
}
