import { useQuery } from '@tanstack/react-query'
import type {
	CapabilityDto,
	CountryDto,
	IndustryDto,
	OpportunityTypeDto,
	RegionDto,
	ServiceSubTypeDto,
	ServiceTypeDto,
} from '@/api'
import { callApi } from '@/server/proxy'

// Cache duration for reference data (30 minutes)
const REFERENCE_DATA_STALE_TIME = 30 * 60 * 1000

export function useRegions() {
	return useQuery({
		queryKey: ['reference', 'regions'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiRegions' } })
			if (response.error) throw response.error
			return (response as RegionDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2, // Keep in cache for 1 hour
		refetchOnMount: 'always',
	})
}

export function useCountries() {
	return useQuery({
		queryKey: ['reference', 'countries'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiCountries' } })
			if (response.error) throw response.error
			return (response as CountryDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2, // Keep in cache for 1 hour
		refetchOnMount: 'always',
	})
}

export function useOpportunityTypes() {
	return useQuery({
		queryKey: ['reference', 'opportunityTypes'],
		queryFn: async () => {
			const response = await callApi({
				data: { fn: 'getApiOpportunityTypes' },
			})
			if (response.error) throw response.error
			return (response as OpportunityTypeDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2,
		refetchOnMount: 'always',
	})
}

export function useServiceTypes() {
	return useQuery({
		queryKey: ['reference', 'serviceTypes'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiServiceTypes' } })
			if (response.error) throw response.error
			return (response as ServiceTypeDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2,
		refetchOnMount: 'always',
	})
}

export function useCapabilities() {
	return useQuery({
		queryKey: ['reference', 'capabilities'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiCapabilities' } })
			if (response.error) throw response.error
			return (response as CapabilityDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2,
		refetchOnMount: 'always',
	})
}

export function useIndustries() {
	return useQuery({
		queryKey: ['reference', 'industries'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiIndustries' } })
			if (response.error) throw response.error
			return (response as IndustryDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2,
		refetchOnMount: 'always',
	})
}

export function useServiceSubTypes() {
	return useQuery({
		queryKey: ['reference', 'serviceSubTypes'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiServiceSubTypes' } })
			if (response.error) throw response.error
			return (response as ServiceSubTypeDto[]) || []
		},
		staleTime: REFERENCE_DATA_STALE_TIME,
		gcTime: REFERENCE_DATA_STALE_TIME * 2,
		refetchOnMount: 'always',
	})
}

export function useCompaniesMe() {
	return useQuery({
		queryKey: ['reference', 'companiesMe'],
		queryFn: async () => {
			const response = await callApi({ data: { fn: 'getApiCompaniesMe' } })
			if (response.error) throw response.error
			return response || []
		},
		staleTime: 5 * 60 * 1000, // 5 minutes for user-specific data
		gcTime: 10 * 60 * 1000, // Keep in cache for 10 minutes
		refetchOnMount: 'always',
	})
}

// Composite hook to prefetch all reference data at once
export function usePrefetchReferenceData() {
	const regions = useRegions()
	const countries = useCountries()
	const opportunityTypes = useOpportunityTypes()
	const serviceTypes = useServiceTypes()
	const serviceSubTypes = useServiceSubTypes()
	const capabilities = useCapabilities()
	const industries = useIndustries()
	const companiesMe = useCompaniesMe()

	return {
		regions,
		countries,
		opportunityTypes,
		serviceTypes,
		serviceSubTypes,
		capabilities,
		industries,
		companiesMe,
		isLoading:
			regions.isLoading ||
			countries.isLoading ||
			opportunityTypes.isLoading ||
			serviceTypes.isLoading ||
			serviceSubTypes.isLoading ||
			capabilities.isLoading ||
			industries.isLoading ||
			companiesMe.isLoading,
		isError:
			regions.isError ||
			countries.isError ||
			opportunityTypes.isError ||
			serviceTypes.isError ||
			serviceSubTypes.isError ||
			capabilities.isError ||
			industries.isError ||
			companiesMe.isError,
	}
}
