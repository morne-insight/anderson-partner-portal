"use client";

import * as React from "react";
import {
  Combobox,
  ComboboxChip,
  ComboboxChipRemove,
  ComboboxChips,
  ComboboxClear,
  ComboboxContent,
  ComboboxControl,
  ComboboxEmpty,
  ComboboxIcon,
  ComboboxInput,
  ComboboxItem,
  ComboboxItemIndicator,
  ComboboxList,
  ComboboxValue,
} from "@/components/ui/base-combobox";
import { Label } from "@/components/ui/base-label";

export default function MultiSelectComboboxExample() {
  const containerRef = React.useRef<HTMLDivElement | null>(null);
  const id = React.useId();

  return (
    <Combobox items={langs} multiple>
      <div className="flex w-full max-w-xs flex-col gap-3">
        <Label htmlFor={id}>Programming languages</Label>
        <ComboboxControl ref={containerRef}>
          <ComboboxValue>
            <ComboboxInput id={id} placeholder="e.g. Apple" />
          </ComboboxValue>
          <ComboboxClear />
          <ComboboxIcon />
        </ComboboxControl>

        <ComboboxChips className="border-0 p-0! shadow-none">
          <ComboboxValue>
            {(value: ProgrammingLanguage[]) => (
              <React.Fragment>
                {value.map((language) => (
                  <ComboboxChip aria-label={language.value} key={language.id}>
                    {language.value}
                    <ComboboxChipRemove />
                  </ComboboxChip>
                ))}
              </React.Fragment>
            )}
          </ComboboxValue>
        </ComboboxChips>
      </div>

      <ComboboxContent anchor={containerRef}>
        <ComboboxEmpty>No languages found.</ComboboxEmpty>
        <ComboboxList>
          {(language: ProgrammingLanguage) => (
            <ComboboxItem key={language.id} value={language}>
              <ComboboxItemIndicator />
              <div className="col-start-2">{language.value}</div>
            </ComboboxItem>
          )}
        </ComboboxList>
      </ComboboxContent>
    </Combobox>
  );
}

interface ProgrammingLanguage {
  id: string;
  value: string;
}

const langs: ProgrammingLanguage[] = [
  { id: "js", value: "JavaScript" },
  { id: "ts", value: "TypeScript" },
  { id: "py", value: "Python" },
  { id: "java", value: "Java" },
  { id: "cpp", value: "C++" },
  { id: "cs", value: "C#" },
  { id: "php", value: "PHP" },
  { id: "ruby", value: "Ruby" },
  { id: "go", value: "Go" },
  { id: "rust", value: "Rust" },
  { id: "swift", value: "Swift" },
];
