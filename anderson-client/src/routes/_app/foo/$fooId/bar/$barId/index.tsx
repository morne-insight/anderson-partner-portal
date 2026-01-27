import { createFileRoute, Link } from "@tanstack/react-router";

export const Route = createFileRoute("/_app/foo/$fooId/bar/$barId/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <div>
      Hello "/_app/foo/$fooId/bar/$barId/"!
      <div className="flex flex-col gap-2">
        <Link to="/dev">Go to Dev</Link>
        <Link to="/foo">Go to Foo</Link>
        <Link params={{ fooId: "123" }} to="/foo/$fooId">
          Go to Foo with id
        </Link>
        <Link params={{ fooId: "123" }} to="/foo/$fooId/bar">
          Go to Foo with id and bar
        </Link>
      </div>
    </div>
  );
}
