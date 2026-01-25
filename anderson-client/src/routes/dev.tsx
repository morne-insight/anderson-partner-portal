import { createFileRoute, Link } from "@tanstack/react-router";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/dev")({
  component: RouteComponent,
});

function RouteComponent() {
  // const { data } = useQuery({
  //   queryKey: ["opportunities"],
  //   queryFn: () => callApi({ data: { fn: "getApiOpportunitiesMe" } }), // Works!
  // });

  return (
    <div>
      <div className="flex flex-col gap-2">
        <Link to="/foo">Go to Foo</Link>
        <Link params={{ fooId: "123" }} to="/foo/$fooId">
          Go to Foo with id
        </Link>
      </div>
      <br />
      <button
        onClick={() => callApi({ data: { fn: "getApiUserDetail" } })}
        type="button"
      >
        Test API
      </button>
      {/* <pre>{JSON.stringify(data, null, 2)}</pre> */}
    </div>
  );
}
