import { useQueryClient } from '@tanstack/react-query'
import { Check, Database, Edit2, MapPin, Plus, Search, ShieldCheck, Trash2, X } from 'lucide-react'
import { useCallback, useEffect, useMemo, useState } from 'react'
import { usePrefetchReferenceData } from '@/hooks/useReferenceData'
import { callApi } from '@/server/proxy'

type TabId =
    | 'regions'
    | 'countries'
    | 'serviceTypes'
    | 'serviceSubTypes'
    | 'coreServices'
    | 'industries'
    | 'opportunityTypes'

type TableRow = {
    id: string
    name: string
    parentId?: string
    parentName?: string
}

const tabs: Array<{ id: TabId; label: string; singularLabel: string }> = [
    { id: 'regions', label: 'Regions', singularLabel: 'Region' },
    { id: 'countries', label: 'Countries', singularLabel: 'Country' },
    { id: 'serviceTypes', label: 'Service Types', singularLabel: 'Service Type' },
    {
        id: 'serviceSubTypes',
        label: 'Service specialisations',
        singularLabel: 'Service specialisation',
    },
    { id: 'coreServices', label: 'Core Services', singularLabel: 'Core Service' },
    { id: 'industries', label: 'Industries', singularLabel: 'Industry' },
    { id: 'opportunityTypes', label: 'Opportunity Types', singularLabel: 'Opportunity Type' },
]

function BackOffice() {
    const queryClient = useQueryClient()
    const {
        regions,
        countries,
        serviceTypes,
        serviceSubTypes,
        capabilities,
        industries,
        opportunityTypes,
        isLoading,
        isError,
    } = usePrefetchReferenceData()

    const [activeSubTab, setActiveSubTab] = useState<TabId>('regions')
    const [searchQuery, setSearchQuery] = useState('')
    const [editingId, setEditingId] = useState<string | null>(null)
    const [editValue, setEditValue] = useState('')
    const [editParentId, setEditParentId] = useState('')
    const [isAdding, setIsAdding] = useState(false)
    const [newValue, setNewValue] = useState('')
    const [newParentId, setNewParentId] = useState('')

    const regionOptions = useMemo(
        () =>
            (regions.data ?? [])
                .filter((region) => region.id && region.name)
                .map((region) => ({ id: region.id as string, name: region.name as string }))
                .sort((a, b) => a.name.localeCompare(b.name)),
        [regions.data]
    )

    const serviceTypeOptions = useMemo(
        () =>
            (serviceTypes.data ?? [])
                .filter((serviceType) => serviceType.id && serviceType.name)
                .map((serviceType) => ({ id: serviceType.id as string, name: serviceType.name as string }))
                .sort((a, b) => a.name.localeCompare(b.name)),
        [serviceTypes.data]
    )

    const currentTab = tabs.find((tab) => tab.id === activeSubTab) ?? tabs[0]

    const currentRows = useMemo<TableRow[]>(() => {
        if (activeSubTab === 'regions') {
            return (regions.data ?? [])
                .filter((region) => region.id && region.name)
                .map((region) => ({ id: region.id as string, name: region.name as string }))
        }

        if (activeSubTab === 'countries') {
            return (countries.data ?? [])
                .filter((country) => country.id && country.name)
                .map((country) => ({
                    id: country.id as string,
                    name: country.name as string,
                    parentId: country.regionId ?? '',
                    parentName:
                        regionOptions.find((region) => region.id === country.regionId)?.name ?? 'Unknown region',
                }))
        }

        if (activeSubTab === 'serviceTypes') {
            return (serviceTypes.data ?? [])
                .filter((serviceType) => serviceType.id && serviceType.name)
                .map((serviceType) => ({ id: serviceType.id as string, name: serviceType.name as string }))
        }

        if (activeSubTab === 'serviceSubTypes') {
            return (serviceSubTypes.data ?? [])
                .filter((serviceSubType) => serviceSubType.id && serviceSubType.name)
                .map((serviceSubType) => ({
                    id: serviceSubType.id as string,
                    name: serviceSubType.name as string,
                    parentId: serviceSubType.serviceTypeId,
                    parentName:
                        serviceTypeOptions.find((serviceType) => serviceType.id === serviceSubType.serviceTypeId)
                            ?.name ?? 'Unknown service type',
                }))
        }

        if (activeSubTab === 'coreServices') {
            return (capabilities.data ?? [])
                .filter((capability) => capability.id && capability.name)
                .map((capability) => ({ id: capability.id as string, name: capability.name as string }))
        }

        if (activeSubTab === 'industries') {
            return (industries.data ?? [])
                .filter((industry) => industry.id && industry.name)
                .map((industry) => ({ id: industry.id as string, name: industry.name as string }))
        }

        return (opportunityTypes.data ?? [])
            .filter((opportunityType) => opportunityType.id && opportunityType.name)
            .map((opportunityType) => ({ id: opportunityType.id as string, name: opportunityType.name as string }))
    }, [
        activeSubTab,
        regions.data,
        countries.data,
        serviceTypes.data,
        serviceSubTypes.data,
        capabilities.data,
        industries.data,
        opportunityTypes.data,
        regionOptions,
        serviceTypeOptions,
    ])

    const filteredRows = useMemo(
        () =>
            currentRows
                .filter((row) => row.name.toLowerCase().includes(searchQuery.toLowerCase()))
                .sort((a, b) => a.name.localeCompare(b.name)),
        [currentRows, searchQuery]
    )

    const resetRowEditor = useCallback(() => {
        setIsAdding(false)
        setEditingId(null)
        setEditValue('')
        setEditParentId('')
        setNewValue('')
        setNewParentId('')
    }, [])

    const refetchLatestReferenceData = useCallback(async () => {
        await queryClient.invalidateQueries({ queryKey: ['reference'] })
        await Promise.all([
            regions.refetch(),
            countries.refetch(),
            serviceTypes.refetch(),
            serviceSubTypes.refetch(),
            capabilities.refetch(),
            industries.refetch(),
            opportunityTypes.refetch(),
        ])
    }, [
        queryClient,
        regions,
        countries,
        serviceTypes,
        serviceSubTypes,
        capabilities,
        industries,
        opportunityTypes,
    ])

    useEffect(() => {
        void refetchLatestReferenceData()
    }, [refetchLatestReferenceData])

    const handleAdd = async () => {
        const trimmedValue = newValue.trim()
        if (!trimmedValue) {
            return
        }

        if (currentRows.some((row) => row.name.toLowerCase() === trimmedValue.toLowerCase())) {
            alert('This item already exists.')
            return
        }

        if (activeSubTab === 'countries' && !newParentId) {
            alert('Please select a region for this country.')
            return
        }

        if (activeSubTab === 'serviceSubTypes' && !newParentId) {
            alert('Please select a parent service type.')
            return
        }

        try {
            if (activeSubTab === 'regions') {
                await callApi({ data: { fn: 'postApiRegions', args: { body: { name: trimmedValue, description: '' } } } })
            } else if (activeSubTab === 'countries') {
                await callApi({
                    data: {
                        fn: 'postApiCountries',
                        args: { body: { name: trimmedValue, description: '', regionId: newParentId } },
                    },
                })
            } else if (activeSubTab === 'serviceTypes') {
                await callApi({
                    data: { fn: 'postApiServiceTypes', args: { body: { name: trimmedValue, description: '' } } },
                })
            } else if (activeSubTab === 'serviceSubTypes') {
                await callApi({
                    data: {
                        fn: 'postApiServiceSubTypes',
                        args: { body: { serviceTypeId: newParentId, name: trimmedValue, description: '' } },
                    },
                })
            } else if (activeSubTab === 'coreServices') {
                await callApi({
                    data: { fn: 'postApiCapabilities', args: { body: { name: trimmedValue, description: '' } } },
                })
            } else if (activeSubTab === 'industries') {
                await callApi({
                    data: { fn: 'postApiIndustries', args: { body: { name: trimmedValue, description: '' } } },
                })
            } else {
                await callApi({
                    data: { fn: 'postApiOpportunityTypes', args: { body: { name: trimmedValue, description: '' } } },
                })
            }

            await refetchLatestReferenceData()
            resetRowEditor()
        } catch (error) {
            console.error(error)
            alert(`Failed to create ${currentTab.singularLabel.toLowerCase()}.`)
        }
    }

    const handleUpdate = async () => {
        const trimmedValue = editValue.trim()
        if (!editingId || !trimmedValue) {
            return
        }

        if (activeSubTab === 'countries' && !editParentId) {
            alert('Please select a region for this country.')
            return
        }

        if (activeSubTab === 'serviceSubTypes' && !editParentId) {
            alert('Please select a parent service type.')
            return
        }

        try {
            if (activeSubTab === 'regions') {
                await callApi({
                    data: {
                        fn: 'putApiRegionsById',
                        args: { path: { id: editingId }, body: { id: editingId, name: trimmedValue, description: '' } },
                    },
                })
            } else if (activeSubTab === 'countries') {
                await callApi({
                    data: {
                        fn: 'putApiCountriesById',
                        args: {
                            path: { id: editingId },
                            body: { id: editingId, name: trimmedValue, description: '', regionId: editParentId },
                        },
                    },
                })
            } else if (activeSubTab === 'serviceTypes') {
                await callApi({
                    data: {
                        fn: 'putApiServiceTypesById',
                        args: { path: { id: editingId }, body: { id: editingId, name: trimmedValue, description: '' } },
                    },
                })
            } else if (activeSubTab === 'serviceSubTypes') {
                await callApi({
                    data: {
                        fn: 'putApiServiceSubTypesById',
                        args: {
                            path: { id: editingId },
                            body: { id: editingId, name: trimmedValue, description: '', serviceTypeId: editParentId },
                        },
                    },
                })
            } else if (activeSubTab === 'coreServices') {
                await callApi({
                    data: {
                        fn: 'putApiCapabilitiesById',
                        args: { path: { id: editingId }, body: { id: editingId, name: trimmedValue, description: '' } },
                    },
                })
            } else if (activeSubTab === 'industries') {
                await callApi({
                    data: {
                        fn: 'putApiIndustriesById',
                        args: { path: { id: editingId }, body: { id: editingId, name: trimmedValue, description: '' } },
                    },
                })
            } else {
                await callApi({
                    data: {
                        fn: 'putApiOpportunityTypesById',
                        args: { path: { id: editingId }, body: { id: editingId, name: trimmedValue, description: '' } },
                    },
                })
            }

            await refetchLatestReferenceData()
            resetRowEditor()
        } catch (error) {
            console.error(error)
            alert(`Failed to update ${currentTab.singularLabel.toLowerCase()}.`)
        }
    }

    const handleDelete = async (row: TableRow) => {
        if (
            !window.confirm(
                `Are you sure you want to delete "${row.name}"? This might affect existing profiles.`
            )
        ) {
            return
        }

        try {
            if (activeSubTab === 'regions') {
                await callApi({ data: { fn: 'deleteApiRegionsById', args: { path: { id: row.id } } } })
            } else if (activeSubTab === 'countries') {
                await callApi({ data: { fn: 'deleteApiCountriesById', args: { path: { id: row.id } } } })
            } else if (activeSubTab === 'serviceTypes') {
                await callApi({ data: { fn: 'deleteApiServiceTypesById', args: { path: { id: row.id } } } })
            } else if (activeSubTab === 'serviceSubTypes') {
                await callApi({ data: { fn: 'deleteApiServiceSubTypesById', args: { path: { id: row.id } } } })
            } else if (activeSubTab === 'coreServices') {
                await callApi({ data: { fn: 'deleteApiCapabilitiesById', args: { path: { id: row.id } } } })
            } else if (activeSubTab === 'industries') {
                await callApi({ data: { fn: 'deleteApiIndustriesById', args: { path: { id: row.id } } } })
            } else {
                await callApi({ data: { fn: 'deleteApiOpportunityTypesById', args: { path: { id: row.id } } } })
            }

            await refetchLatestReferenceData()
            resetRowEditor()
        } catch (error) {
            console.error(error)
            alert(`Failed to delete ${currentTab.singularLabel.toLowerCase()}.`)
        }
    }

    const secondColumnLabel =
        activeSubTab === 'countries'
            ? 'Region'
            : activeSubTab === 'serviceSubTypes'
                ? 'Service Type'
                : null

    if (isLoading) {
        return (
            <div className="animate-fade-in space-y-10">
                <header className="flex items-end justify-between border-gray-200 border-b pb-6">
                    <div>
                        <div className="mb-2 flex items-center gap-3">
                            <ShieldCheck className="h-8 w-8 text-yellow-500" />
                            <h2 className="font-serif text-4xl text-black">Back Office</h2>
                        </div>
                        <p className="font-light text-gray-500 text-lg">Loading latest master data...</p>
                    </div>
                </header>
            </div>
        )
    }

    if (isError) {
        return (
            <div className="animate-fade-in space-y-10">
                <header className="flex items-end justify-between border-gray-200 border-b pb-6">
                    <div>
                        <div className="mb-2 flex items-center gap-3">
                            <ShieldCheck className="h-8 w-8 text-yellow-500" />
                            <h2 className="font-serif text-4xl text-black">Back Office</h2>
                        </div>
                        <p className="font-light text-gray-500 text-lg">
                            Failed to load master data. Please refresh and try again.
                        </p>
                    </div>
                </header>
            </div>
        )
    }

    return (
        <div className="animate-fade-in space-y-10">
            <header className="flex items-end justify-between border-gray-200 border-b pb-6">
                <div>
                    <div className="mb-2 flex items-center gap-3">
                        <ShieldCheck className="h-8 w-8 text-yellow-500" />
                        <h2 className="font-serif text-4xl text-black">Back Office</h2>
                    </div>
                    <p className="font-light text-gray-500 text-lg">System Administration & Master Data Management</p>
                </div>

                <div className="flex items-center gap-4 border border-yellow-200 bg-yellow-50 px-4 py-2">
                    <span className="font-bold text-[10px] text-yellow-700 uppercase tracking-widest">
                        Admin Session Active
                    </span>
                    <div className="h-2 w-2 animate-pulse rounded-full bg-yellow-500" />
                </div>
            </header>

            <div className="no-scrollbar flex gap-2 overflow-x-auto pb-2">
                {tabs.map((tab) => (
                    <button
                        type="button"
                        key={tab.id}
                        onClick={() => {
                            setActiveSubTab(tab.id)
                            setSearchQuery('')
                            resetRowEditor()
                        }}
                        className={`border px-6 py-3 font-bold text-[10px] uppercase tracking-widest transition-all ${activeSubTab === tab.id
                            ? 'border-black bg-black text-white'
                            : 'border-gray-200 bg-white text-gray-500 hover:border-gray-400'
                            }`}
                    >
                        {tab.label}
                    </button>
                ))}
            </div>

            <div className="overflow-hidden border border-gray-200 bg-white shadow-sm">
                <div className="flex flex-col items-center justify-between gap-4 border-gray-100 border-b bg-gray-50/50 p-6 md:flex-row">
                    <div className="relative w-full md:w-96">
                        <Search className="absolute top-3 left-3 h-4 w-4 text-gray-400" />
                        <input
                            type="text"
                            placeholder={`Search ${currentTab.label.toLowerCase()}...`}
                            value={searchQuery}
                            onChange={(event) => setSearchQuery(event.target.value)}
                            className="w-full border border-gray-200 bg-white py-2 pr-4 pl-10 text-sm outline-none transition-colors focus:border-black"
                        />
                    </div>

                    <button
                        type="button"
                        onClick={() => {
                            setIsAdding(true)
                            setEditingId(null)
                            setEditValue('')
                            setEditParentId('')
                            setNewValue('')
                            if (activeSubTab === 'countries') {
                                setNewParentId(regionOptions[0]?.id ?? '')
                            } else if (activeSubTab === 'serviceSubTypes') {
                                setNewParentId(serviceTypeOptions[0]?.id ?? '')
                            } else {
                                setNewParentId('')
                            }
                        }}
                        className="flex w-full items-center justify-center gap-2 bg-black px-6 py-2 font-bold text-[10px] text-white uppercase tracking-widest transition-colors hover:bg-red-600 md:w-auto"
                    >
                        <Plus className="h-4 w-4" /> Add New {currentTab.singularLabel}
                    </button>
                </div>

                <div className="overflow-x-auto">
                    <table className="w-full text-left">
                        <thead>
                            <tr className="border-gray-100 border-b bg-gray-50">
                                <th className="px-8 py-4 font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                                    Name
                                </th>
                                {secondColumnLabel && (
                                    <th className="px-8 py-4 font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                                        {secondColumnLabel}
                                    </th>
                                )}
                                <th className="px-8 py-4 text-right font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                                    Actions
                                </th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-gray-50">
                            {isAdding && (
                                <tr className="animate-fade-in bg-yellow-50">
                                    <td className="px-8 py-4">
                                        <input
                                            autoFocus
                                            type="text"
                                            value={newValue}
                                            onChange={(event) => setNewValue(event.target.value)}
                                            placeholder={`Enter new ${currentTab.singularLabel.toLowerCase()}...`}
                                            className="w-full max-w-md border border-yellow-300 px-3 py-2 text-sm outline-none focus:ring-1 focus:ring-yellow-500"
                                        />
                                    </td>
                                    {activeSubTab === 'countries' && (
                                        <td className="px-8 py-4">
                                            <select
                                                value={newParentId}
                                                onChange={(event) => setNewParentId(event.target.value)}
                                                className="w-full max-w-xs border border-yellow-300 bg-white px-3 py-2 text-sm outline-none"
                                            >
                                                <option value="">Select Region...</option>
                                                {regionOptions.map((region) => (
                                                    <option key={region.id} value={region.id}>
                                                        {region.name}
                                                    </option>
                                                ))}
                                            </select>
                                        </td>
                                    )}
                                    {activeSubTab === 'serviceSubTypes' && (
                                        <td className="px-8 py-4">
                                            <select
                                                value={newParentId}
                                                onChange={(event) => setNewParentId(event.target.value)}
                                                className="w-full max-w-xs border border-yellow-300 bg-white px-3 py-2 text-sm outline-none"
                                            >
                                                <option value="">Select Service Type...</option>
                                                {serviceTypeOptions.map((serviceType) => (
                                                    <option key={serviceType.id} value={serviceType.id}>
                                                        {serviceType.name}
                                                    </option>
                                                ))}
                                            </select>
                                        </td>
                                    )}
                                    <td className="px-8 py-4 text-right">
                                        <div className="flex justify-end gap-2">
                                            <button type="button" onClick={handleAdd} className="p-2 text-green-600 hover:bg-green-100">
                                                <Check className="h-4 w-4" />
                                            </button>
                                            <button type="button" onClick={resetRowEditor} className="p-2 text-red-600 hover:bg-red-100">
                                                <X className="h-4 w-4" />
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            )}

                            {filteredRows.length === 0 && !isAdding ? (
                                <tr>
                                    <td colSpan={secondColumnLabel ? 3 : 2} className="px-8 py-12 text-center text-gray-400 text-sm italic">
                                        No items found matching your search.
                                    </td>
                                </tr>
                            ) : (
                                filteredRows.map((row) => {
                                    const isEditing = editingId === row.id

                                    return (
                                        <tr key={row.id} className="group transition-colors hover:bg-gray-50">
                                            <td className="px-8 py-4">
                                                {isEditing ? (
                                                    <input
                                                        autoFocus
                                                        type="text"
                                                        value={editValue}
                                                        onChange={(event) => setEditValue(event.target.value)}
                                                        className="w-full max-w-md border border-black px-3 py-2 text-sm outline-none"
                                                    />
                                                ) : (
                                                    <span className="font-medium text-gray-700 text-sm">{row.name}</span>
                                                )}
                                            </td>

                                            {activeSubTab === 'countries' && (
                                                <td className="px-8 py-4">
                                                    {isEditing ? (
                                                        <select
                                                            value={editParentId}
                                                            onChange={(event) => setEditParentId(event.target.value)}
                                                            className="w-full max-w-xs border border-black bg-white px-3 py-2 text-sm outline-none"
                                                        >
                                                            <option value="">Select Region...</option>
                                                            {regionOptions.map((region) => (
                                                                <option key={region.id} value={region.id}>
                                                                    {region.name}
                                                                </option>
                                                            ))}
                                                        </select>
                                                    ) : (
                                                        <div className="flex items-center font-bold text-[10px] text-gray-500 uppercase tracking-widest">
                                                            <MapPin className="mr-2 h-3 w-3 text-red-600" />
                                                            {row.parentName}
                                                        </div>
                                                    )}
                                                </td>
                                            )}

                                            {activeSubTab === 'serviceSubTypes' && (
                                                <td className="px-8 py-4">
                                                    {isEditing ? (
                                                        <select
                                                            value={editParentId}
                                                            onChange={(event) => setEditParentId(event.target.value)}
                                                            className="w-full max-w-xs border border-black bg-white px-3 py-2 text-sm outline-none"
                                                        >
                                                            <option value="">Select Service Type...</option>
                                                            {serviceTypeOptions.map((serviceType) => (
                                                                <option key={serviceType.id} value={serviceType.id}>
                                                                    {serviceType.name}
                                                                </option>
                                                            ))}
                                                        </select>
                                                    ) : (
                                                        <span className="font-bold text-[10px] text-gray-500 uppercase tracking-widest">
                                                            {row.parentName}
                                                        </span>
                                                    )}
                                                </td>
                                            )}

                                            <td className="px-8 py-4 text-right">
                                                {isEditing ? (
                                                    <div className="flex justify-end gap-2">
                                                        <button
                                                            type="button"
                                                            onClick={handleUpdate}
                                                            className="p-2 text-green-600 hover:bg-green-100"
                                                        >
                                                            <Check className="h-4 w-4" />
                                                        </button>
                                                        <button
                                                            type="button"
                                                            onClick={resetRowEditor}
                                                            className="p-2 text-red-600 hover:bg-red-100"
                                                        >
                                                            <X className="h-4 w-4" />
                                                        </button>
                                                    </div>
                                                ) : (
                                                    <div className="flex justify-end gap-4 opacity-0 transition-opacity group-hover:opacity-100">
                                                        <button
                                                            type="button"
                                                            onClick={() => {
                                                                setIsAdding(false)
                                                                setEditingId(row.id)
                                                                setEditValue(row.name)
                                                                setEditParentId(row.parentId ?? '')
                                                            }}
                                                            className="flex items-center gap-1 font-bold text-[10px] text-gray-400 uppercase tracking-widest hover:text-black"
                                                        >
                                                            <Edit2 className="h-3.5 w-3.5" /> Edit
                                                        </button>
                                                        <button
                                                            type="button"
                                                            onClick={() => void handleDelete(row)}
                                                            className="flex items-center gap-1 font-bold text-[10px] text-gray-400 uppercase tracking-widest hover:text-red-600"
                                                        >
                                                            <Trash2 className="h-3.5 w-3.5" /> Delete
                                                        </button>
                                                    </div>
                                                )}
                                            </td>
                                        </tr>
                                    )
                                })
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            <div className="flex items-start gap-6 border border-gray-200 bg-white p-8 shadow-sm">
                <div className="border border-blue-100 bg-blue-50 p-3 text-blue-600">
                    <Database className="h-5 w-5" />
                </div>
                <div>
                    <h4 className="mb-1 font-bold text-black text-sm uppercase tracking-widest">
                        Database Integrity
                    </h4>
                    <p className="text-gray-500 text-xs leading-relaxed">
                        Changes made here are applied globally across the portal. Deleting items will remove them from
                        all related entities.
                    </p>
                </div>
            </div>
        </div>
    )
}

export default BackOffice
