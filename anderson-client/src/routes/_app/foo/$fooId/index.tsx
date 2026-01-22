import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/_app/foo/$fooId/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div>
      Hello "/foo/$fooId" as Index!
      <br />
      <div className="flex flex-col gap-2">
        <Link to="/dev">Go to Dev</Link>
        <Link to="/foo">Go to Foo</Link>
        <Link params={{ fooId: "123" }} to="/foo/$fooId/bar">
          Go to Foo with id and bar
        </Link>
        <Link params={{ fooId: "123", barId: "456" }} to="/foo/$fooId/bar/$barId">
          Go to Foo with id and bar with id
        </Link>
      </div>
    </div>  
  );
}
