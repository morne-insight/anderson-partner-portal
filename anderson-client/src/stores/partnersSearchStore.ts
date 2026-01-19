import { Store } from '@tanstack/react-store'

export interface SearchFilters {
  activeRegionFilter: string
  selectedServiceType: string
  selectedCountry: string
  selectedCapabilities: string[]
}

export interface SearchState {
  query: string
  results: any[]
  isSearching: boolean
  filters: SearchFilters
  showServiceDropdown: boolean
  showRegionDropdown: boolean
  showCountryDropdown: boolean
}

const initialState: SearchState = {
  query: '',
  results: [],
  isSearching: false,
  filters: {
    activeRegionFilter: 'All',
    selectedServiceType: 'All',
    selectedCountry: 'All',
    selectedCapabilities: []
  },
  showServiceDropdown: false,
  showRegionDropdown: false,
  showCountryDropdown: false
}

export const partnersSearchStore = new Store(initialState)

// Actions
export const setQuery = (query: string) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    query
  }))
}

export const setResults = (results: any[]) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    results
  }))
}

export const setIsSearching = (isSearching: boolean) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    isSearching
  }))
}

export const setActiveRegionFilter = (activeRegionFilter: string) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    filters: {
      ...state.filters,
      activeRegionFilter,
      selectedCountry: 'All' // Reset country when region changes
    }
  }))
}

export const setSelectedServiceType = (selectedServiceType: string) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    filters: {
      ...state.filters,
      selectedServiceType
    }
  }))
}

export const setSelectedCountry = (selectedCountry: string) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    filters: {
      ...state.filters,
      selectedCountry
    }
  }))
}

export const setSelectedCapabilities = (selectedCapabilities: string[]) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    filters: {
      ...state.filters,
      selectedCapabilities
    }
  }))
}

export const setShowServiceDropdown = (show: boolean) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    showServiceDropdown: show,
    showRegionDropdown: false,
    showCountryDropdown: false
  }))
}

export const setShowRegionDropdown = (show: boolean) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    showRegionDropdown: show,
    showServiceDropdown: false,
    showCountryDropdown: false
  }))
}

export const setShowCountryDropdown = (show: boolean) => {
  partnersSearchStore.setState((state) => ({
    ...state,
    showCountryDropdown: show,
    showServiceDropdown: false,
    showRegionDropdown: false
  }))
}

export const clearFilters = () => {
  partnersSearchStore.setState((state) => ({
    ...state,
    filters: {
      activeRegionFilter: 'All',
      selectedServiceType: 'All',
      selectedCountry: 'All',
      selectedCapabilities: []
    }
  }))
}

export const clearSearch = () => {
  partnersSearchStore.setState(() => initialState)
}
