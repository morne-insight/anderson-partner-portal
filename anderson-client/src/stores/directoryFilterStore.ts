import { Store } from '@tanstack/react-store'

export interface DirectoryFilterState {
  selectedRegion: string
  selectedCountry: string
  selectedService: string
  selectedIndustry: string
  selectedCapability: string
  nameFilter: string
}

const initialState: DirectoryFilterState = {
  selectedRegion: 'All',
  selectedCountry: 'All',
  selectedService: 'All',
  selectedIndustry: 'All',
  selectedCapability: 'All',
  nameFilter: ''
}

export const directoryFilterStore = new Store(initialState)

// Actions
export const setSelectedRegion = (selectedRegion: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedRegion,
    selectedCountry: 'All' // Reset country when region changes
  }))
}

export const setSelectedCountry = (selectedCountry: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedCountry
  }))
}

export const setSelectedService = (selectedService: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedService
  }))
}

export const setSelectedIndustry = (selectedIndustry: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedIndustry
  }))
}

export const setSelectedCapability = (selectedCapability: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedCapability
  }))
}

export const setNameFilter = (nameFilter: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    nameFilter
  }))
}

export const clearAllDirectoryFilters = () => {
  directoryFilterStore.setState(() => initialState)
}

export const resetDirectoryFilters = () => {
  directoryFilterStore.setState(() => initialState)
}
