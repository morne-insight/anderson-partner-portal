import { Store } from "@tanstack/react-store";

export interface PaginationState {
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  pageCount: number;
}

export interface DirectoryFilterState {
  selectedRegion: string;
  selectedCountry: string;
  selectedService: string;
  selectedCoreService: string;
  selectedIndustry: string;
  selectedCapability: string;
  nameFilter: string;
  pagination: PaginationState;
  isLoading: boolean;
}

const initialState: DirectoryFilterState = {
  selectedRegion: "All",
  selectedCountry: "All",
  selectedService: "All",
  selectedCoreService: "All",
  selectedIndustry: "All",
  selectedCapability: "All",
  nameFilter: "",
  pagination: {
    pageNumber: 1,
    pageSize: 5,
    totalCount: 0,
    pageCount: 0,
  },
  isLoading: false,
};

export const directoryFilterStore = new Store(initialState);

// Actions
export const setSelectedRegion = (selectedRegion: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedRegion,
    selectedCountry: "All", // Reset country when region changes
  }));
};

export const setSelectedCountry = (selectedCountry: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedCountry,
  }));
};

export const setSelectedService = (selectedService: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedService,
    selectedCoreService: "All", // Reset core service when service line changes
  }));
};

export const setSelectedCoreService = (selectedCoreService: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedCoreService,
  }));
};

export const setSelectedIndustry = (selectedIndustry: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedIndustry,
  }));
};

export const setSelectedCapability = (selectedCapability: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    selectedCapability,
  }));
};

export const setNameFilter = (nameFilter: string) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    nameFilter,
  }));
};

export const setPagination = (pagination: Partial<PaginationState>) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    pagination: {
      ...state.pagination,
      ...pagination,
    },
  }));
};

export const setPageNumber = (pageNumber: number) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    pagination: {
      ...state.pagination,
      pageNumber,
    },
  }));
};

export const setIsLoading = (isLoading: boolean) => {
  directoryFilterStore.setState((state) => ({
    ...state,
    isLoading,
  }));
};

export const clearAllDirectoryFilters = () => {
  directoryFilterStore.setState((state) => ({
    ...initialState,
    pagination: {
      ...state.pagination,
      pageNumber: 1,
    },
  }));
};

export const resetDirectoryFilters = () => {
  directoryFilterStore.setState(() => initialState);
};
