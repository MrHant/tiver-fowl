# DevContainer

Base image: `mcr.microsoft.com/devcontainers/dotnet:10.0-noble`, plus the .NET and Node devcontainer
features and a set of pre-configured VS Code extensions. See `devcontainer.json` for the full list.

## Setup scripts

`postCreateCommand` runs the scripts in this directory, then finishes with `dotnet restore`. All are
idempotent and safe to re-run by hand.

| Script | Purpose |
| ------ | ------- |
| `setup-task.sh` | Installs [go-task](https://taskfile.dev), the `task` runner used by `Taskfile.yml` |
| `setup-agents.sh` | Installs coding-agent CLIs (Claude Code, Codex, OpenCode, herdr) |
| `setup-browser-deps.sh` | Installs the native libraries Chrome/Chromedriver need to run headless |

If headless browser tests fail to launch with missing shared-library errors, re-running
`setup-browser-deps.sh` is the first thing to try.

## Re-running setup

```bash
.devcontainer/setup-task.sh
.devcontainer/setup-agents.sh
.devcontainer/setup-browser-deps.sh
dotnet restore
```
