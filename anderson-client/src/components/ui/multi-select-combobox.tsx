"use client";

import { Plus } from "lucide-react";
import { useState } from "react";
import {
  Combobox,
  ComboboxChip,
  ComboboxChipRemove,
  ComboboxChips,
  ComboboxContent,
  ComboboxControl,
  ComboboxEmpty,
  ComboboxInput,
  ComboboxItem,
  ComboboxItemIndicator,
  ComboboxList,
  ComboboxValue,
} from "@/components/ui/base-combobox";

export interface MultiSelectItem {
  id?: string | null;
  name?: string | null;
}

export interface MultiSelectComboboxProps<T extends MultiSelectItem> {
  items: T[];
  selectedIds: string[];
  selectedItemsOverride?: T[];
  onSelectionChange: (ids: string[]) => void;
  placeholder?: string;
  emptyMessage?: string;
  noSelectionMessage?: string;
  helperText?: string;
  chipClassName?: string;
  createButtonClassName?: string;
  onCreateNew?: (name: string) => void;
  isCreating?: boolean;
  getItemLabel?: (item: T) => string;
}

export function MultiSelectCombobox<T extends MultiSelectItem>({
  items,
  selectedIds,
  selectedItemsOverride,
  onSelectionChange,
  placeholder = "Search and select...",
  emptyMessage = "No items found.",
  noSelectionMessage = "No items selected.",
  helperText,
  chipClassName = "rounded-none border border-black bg-black px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider transition-all",
  createButtonClassName = "flex w-full items-center gap-2 px-2 py-2 text-left text-sm text-black hover:bg-gray-100",
  onCreateNew,
  isCreating = false,
  getItemLabel = (item) => item.name || "",
}: MultiSelectComboboxProps<T>) {
  const [inputValue, setInputValue] = useState("");
  console.log(Date.now());
  console.log("items", items);
  console.log("selectedIds", selectedIds);

  const selectedItems =
    selectedItemsOverride ??
    items.filter((item) => item.id && selectedIds.includes(item.id));

  const handleValueChange = (value: unknown) => {
    const selected = value as T[];
    onSelectionChange(
      selected.map((item) => item.id).filter((id): id is string => !!id)
    );
  };

  const showCreateButton =
    onCreateNew &&
    inputValue.trim() &&
    !items.some(
      (item) =>
        getItemLabel(item)?.toLowerCase() === inputValue.toLowerCase()
    );

  return (
    <Combobox
      items={items}
      multiple
      onInputValueChange={setInputValue}
      onValueChange={handleValueChange}
      value={selectedItems}
    >
      <ComboboxChips className="mb-4 border-0 p-0 shadow-none">
        <ComboboxValue>
          {(value: T[]) => (
            <>
              {value.length === 0 && (
                <p className="my-2 text-gray-400 text-xs italic">
                  {noSelectionMessage}
                </p>
              )}
              {value.map((item) => (
                <ComboboxChip
                  aria-label={getItemLabel(item)}
                  className={chipClassName}
                  key={item.id}
                >
                  {getItemLabel(item)}
                  <ComboboxChipRemove />
                </ComboboxChip>
              ))}
            </>
          )}
        </ComboboxValue>
      </ComboboxChips>

      {helperText && (
        <p className="my-2 text-gray-400 text-xs italic">{helperText}</p>
      )}

      <ComboboxControl>
        <ComboboxValue>
          <ComboboxInput placeholder={placeholder} />
        </ComboboxValue>
      </ComboboxControl>

      <ComboboxContent>
        <ComboboxEmpty>{emptyMessage}</ComboboxEmpty>
        <ComboboxList>
          {(item: T) => (
            <ComboboxItem key={item.id} value={item}>
              <ComboboxItemIndicator />
              <div className="col-start-2">{getItemLabel(item)}</div>
            </ComboboxItem>
          )}
        </ComboboxList>
        {showCreateButton && (
          <button
            className={createButtonClassName}
            disabled={isCreating}
            onClick={() => onCreateNew(inputValue.trim())}
            type="button"
          >
            <Plus className="h-4 w-4" />
            {isCreating ? "Creating..." : `Create "${inputValue.trim()}"`}
          </button>
        )}
      </ComboboxContent>
    </Combobox>
  );
}
