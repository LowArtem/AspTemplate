# Asp.net advanced app template

Template solution for Web API development with asp.net\
It has default structure and all important libraries installed and configured. Such as:
- Redis
- Serilog
- PostgreSQL
- FluentValidation
- AutoMapper
- Swagger (OpenAPI)

There are also configured DevOps scripts for deploying web app to the server with some monitoring services configured:
- Grafana
- Prometheus

## Prerequisites

- Installed docker on your server
- Activated [wsl](https://learn.microsoft.com/windows/wsl/install) if you are Windows user
- Installed [ansible](https://docs.ansible.com/ansible/latest/installation_guide/intro_installation.html#pip-install) on your machine (for Linux/Mac users) or in wsl (for Windows users)
- Installed [taskfile](https://taskfile.dev) on your local machine to simplify using deployment commands
- Installed [ssh-keys](https://www.ssh.com/academy/ssh/keygen) for your server (including on wsl) to the default folder `~/.ssh/id_rsa` (or you can configure it in ansible playbooks) so that ansible can connect to you server.

## Installation

```bash
git clone https://github.com/LowArtem/AspTemplate
```

Clone this repository and change all `AspAdvancedApp` occurrences to the name you want the project to have.\
**Don't forget to change file names, project names, solution name, script settings.**

There is also a swagger default displaying parameters that you will want to change at `AspAdvancedApp.Api/appsettings.json` under the `Swagger` block.

Change PostgreSQL settings (username, password, db name, etc) in `DevOps/docker-compose.yml` and all `appsettings.json` files.

## License

[MIT](https://choosealicense.com/licenses/mit/)
