# Core Architecture

This directory contains non-UI services that will become the foundation for the AI agent.

## Rules

- Core services must not reference `System.Windows.Forms`.
- Domain/game code should not call Win32 memory APIs directly.
- AI tools must depend on domain operations, not raw memory primitives.
- UI code remains responsible for presentation and user interaction.

## Current migration

`Core/Memory/MemoryService.cs` provides the first low-level abstraction around process memory.
`Core/Memory/ProcessMemoryContext.cs` separates runtime process state from `Form1`.

The existing `Form1`/`MemoryFuncs.cs` implementation remains unchanged during this phase. Future changes should migrate callers incrementally and keep the existing editor functional while the architecture is extracted.
