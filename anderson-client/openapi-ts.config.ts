import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
  input: "AndersonAPI.Api.json",
  output: {
    format: "biome",
    lint: "biome",
    path: "./src/api",
  },
  plugins: [
    "@hey-api/schemas",
    {
      dates: true,
      name: "@hey-api/transformers",
    },
    {
      enums: "javascript",
      name: "@hey-api/typescript",
    },
    {
      name: "@hey-api/sdk",
      transformer: true,
    },
  ],
});

// export default defineConfig({
//   input: "https://localhost:44395/openapi/v1.json",
//   output: "src/api",
// });
