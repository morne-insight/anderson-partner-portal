import {
  BlockTypeSelect,
  BoldItalicUnderlineToggles,
  headingsPlugin,
  ListsToggle,
  listsPlugin,
  MDXEditor,
  type MDXEditorMethods,
  Separator,
  toolbarPlugin,
  UndoRedo,
} from "@mdxeditor/editor";
import { ClientOnly, createFileRoute, Link } from "@tanstack/react-router";
import { useRef, useState } from "react";
import { toast } from "sonner";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/dev")({
  component: RouteComponent,
});

function RouteComponent() {
  const [md, setMd] = useState("");
  const ref = useRef<MDXEditorMethods>(null);

  return (
    <div>
      <div className="flex flex-col gap-2">
        <Link to="/foo">Go to Foo</Link>
        <Link params={{ fooId: "123" }} to="/foo/$fooId">
          Go to Foo with id
        </Link>
        <button
          onClick={() => callApi({ data: { fn: "getApiUserDetail" } })}
          type="button"
        >
          Test API
        </button>
        <button
          onClick={() => {
            const markdown = ref.current?.getMarkdown();
            toast(markdown);
          }}
          type="button"
        >
          Toast
        </button>
      </div>
      {/* <pre>{JSON.stringify(data, null, 2)}</pre> */}
      <ClientOnly>
        <MDXEditor
          markdown={`# Hello World

## Secondary Heading

This is some **bold text** and *italic text*.

- List item 1
- List item 2
- List item 3

Here's a paragraph with some content to test the formatting.`}
          onChange={console.log}
          plugins={[
            headingsPlugin(),
            listsPlugin(),
            toolbarPlugin({
              toolbarContents: () => (
                <>
                  <UndoRedo />
                  <Separator />
                  <BoldItalicUnderlineToggles />
                  <Separator />
                  <ListsToggle />
                  <Separator />
                  <BlockTypeSelect />
                </>
              ),
            }),
          ]}
          ref={ref}
        />
      </ClientOnly>
    </div>
  );
}
