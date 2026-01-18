import { createFileRoute } from '@tanstack/react-router'
import { callApi } from "@/server/proxy";
import { useQuery } from "@tanstack/react-query";

export const Route = createFileRoute('/_app/dev')({
  component: RouteComponent,
})

function RouteComponent() {

const { data } = useQuery({
  queryKey: ['opportunities'],
  queryFn: () => callApi({ data: { fn: 'getApiOpportunitiesMe' } }) // Works!
});

  return (<div>
    <button onClick={() => callApi({ data: { fn: 'getApiOpportunitiesMe' } })}>
      Test API      
    </button>
    <pre>{JSON.stringify(data, null, 2)}</pre>
  </div>)
}
