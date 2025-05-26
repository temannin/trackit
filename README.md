# TrackIt (Experimental)

> **TrackIt** is an experimental platform for tracking online prices of dynamic items (like flights, hotels, or products) using Python and Selenium automation. The backend can execute user-supplied Python scripts (including Selenium-based scrapers) in isolated Docker containers, making it easy to automate and monitor price changes over time.

## Architecture

- **Frontend:**  
  - React (Vite)  
  - React Router v7  
  - Runs in its own container

- **Backend:**  
  - C# ASP.NET Core Web API  
  - Runs in its own container  
  - Executes Docker commands (including running containers) from inside the backend container using Docker-in-Docker (DIND) and Docker socket mounting

- **Communication:**  
  - Frontend communicates with backend via HTTP API

## Security Notice

- The backend container mounts the host Docker socket (`/var/run/docker.sock`) to enable Docker-in-Docker.
- This setup allows code running in the backend container to control the host Docker daemon, which is a **major security risk**.
- **Do not use this setup in production or on any sensitive system.**

## Development

- All source code is mounted into containers for live reload.
- You can use the devcontainer setup for VS Code for a seamless development experience.

## Features

- React frontend with modern routing
- C# backend with API endpoints
- Backend can execute code in Docker containers (e.g., for running Python code)
- Hot reload for both frontend and backend

## Limitations

- **Not production-ready**
- Security risks due to Docker socket mounting and DIND
- Experimental code execution features

## License

This project is for educational and experimental use only.

