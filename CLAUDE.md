# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

This repository contains two related projects:

- **simple-api**: A minimal REST API for user management (uses Carter, SQLite, minimal APIs)
- **mcp-server**: A Model Context Protocol (MCP) server that exposes tools for interacting with the simple-api

## Architecture

The MCP server acts as a client to the simple-api. It uses `UsersApiClient` (HTTP) to call the REST API and exposes those capabilities as MCP tools (`UsersTools.cs`) and resources (`ApiDocumentationResource.cs`).

```
Claude <-> MCP Server <-> simple-api (REST)
                     <-> UsersApiClient
```

## Commands

```bash
# Build entire solution
dotnet build

# Run the simple-api (REST API)
dotnet run --project simple-api

# Run the MCP server (connects to simple-api at http://localhost:5000 by default)
dotnet run --project mcp-server

# Set custom API base address for MCP server
API_BASE_ADDRESS=http://localhost:5000 dotnet run --project mcp-server
```

## Key Patterns

### MCP Server Tool Implementation
Tools are discovered automatically from assembly via `[McpServerTool]` attribute on `public static async Task<string>` methods in `UsersTools.cs`. The methods take an `UsersApiClient` parameter which is injected via DI.

### MCP Server Resource Implementation
Resources use `[McpServerResource]` attribute on static methods in `ApiDocumentationResource.cs`. The method reads markdown documentation from `docs/users-api-routes.md`.

### simple-api Routes
Routes are defined in `Modules/UsersModule.cs` using Carter's `ICarterModule` pattern with `MapGet/MapPost/MapPut/MapDelete` extensions.

### DTOs
The `mcp-server/Dtos/Users/` folder mirrors the simple-api DTOs. These are hand-written records that match the JSON contract defined in `docs/users-api-routes.md`.

## Configuration

The MCP server loads `.env` files by walking up from the current directory or `AppContext.BaseDirectory`. Environment variables take precedence over `.env` values. The API base address is configured via `API_BASE_ADDRESS` (defaults to `http://localhost:5000`).

## Project Structure

```
mcp-servers/
├── mcp-server/           # MCP server project
│   ├── Dtos/Users/       # Request/Response DTOs (mirrors simple-api)
│   ├── UsersTools.cs     # MCP tools (GetUsers, CreateUser, etc.)
│   ├── UsersApiClient.cs  # HTTP client for simple-api
│   ├── ApiDocumentationResource.cs  # MCP resource for API docs
│   ├── Program.cs        # Host builder setup, DI configuration
│   └── docs/             # Markdown API documentation
└── simple-api/           # REST API project
    ├── Modules/          # Carter route modules
    ├── Entities/         # EF Core entities (User, UserNote)
    ├── Infrastructure/Data/  # AppDbContext, SQLite database
    └── Dtos/Users/       # API request/response records
```
