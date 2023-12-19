# Asp.net advanced app template

Template solution for Web API development with asp.net\
It has default structure and all important libraries installed and configured. Such as:

- Redis
- Serilog
- FluentValidation
- AutoMapper
- Swagger (OpenAPI)
- Entity Framework Core (with PostgreSQL)
- Dapper

There are also configured DevOps scripts for deploying web app to the server with some monitoring services configured:

- Grafana
- Prometheus

Template has [portainer](https://portainer.io) for simple access to running containers.\
There is also configured nginx that proxies all requests to a specific service.

Template has configured github action that runs deployment ansible playbook on every push/pull_request on master (you can change this in `.github/workflows/deploy.yml` file).\
To make it work you need to add to your github repository a [secret](https://docs.github.com/actions/security-guides/using-secrets-in-github-actions) named SSH_PRIVATE_KEY with public part of your server's ssh-key.

## Prerequisites

- Installed [docker](https://docs.docker.com/engine/install/) on your server
- Activated [wsl](https://learn.microsoft.com/windows/wsl/install) if you are Windows user
- Installed [ansible](https://docs.ansible.com/ansible/latest/installation_guide/intro_installation.html#pip-install) on your machine (for Linux/Mac users) or in wsl (for Windows users)
- Installed [taskfile](https://taskfile.dev) on your machine to simplify using deployment commands
- Installed [ssh-keys](https://www.ssh.com/academy/ssh/keygen) for your server (on wsl for Windows) to the default folder `~/.ssh/id_rsa` (or you can configure it in ansible playbooks) so that ansible can connect to your server

## Installation

```bash
git clone https://github.com/LowArtem/AspTemplate
```

Clone this repository and change all `AspAdvancedApp` occurrences to the name you want to assign to the project.\
**Don't forget to change file names, project names, solution name, script settings.**

There are also swagger default displaying parameters that you would want to change at `AspAdvancedApp.Api/appsettings.json` under the `Swagger` block.

Change PostgreSQL settings (username, password, db name, etc) in `DevOps/docker-compose.yml` and all `appsettings.json` files.

Delete `.git` folder in the root directory and init your own git repository:

```bash
git init
```

Enjoy your work!

## License

[MIT](https://choosealicense.com/licenses/mit/)
