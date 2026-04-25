[![CI](https://github.com/ThomasDixini/AplicativoEsteticaDemonstracao/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/ThomasDixini/AplicativoEsteticaDemonstracao/actions/workflows/dotnet-ci.yml)

🌍 **Languages:** [Português](README.md) | [English](README.en.md)

## 📑 Summary

- [⚙️ Prerequisites](#️-prerequisites)
- [🐳 Setup](#-setup)
- [✅ Project Overview](#-project-overview)
- [🏗️ Architecture](#️-architecture)

---

# ⚙️ Prerequisites

To run this project, you only need:
- **Docker and Docker Compose** (version 3.8 or higher)

# 🐳 Setup

1. Clone the repository:
```bash
   git clone https://github.com/ThomasDixini/AplicativoEsteticaDemonstracao.git
   cd <project-folder>
```

2. Start the containers:
```bash
   docker compose up --build
```

3. The backend will be available at `http://localhost:8080`  
   The RabbitMQ management panel will be available at `http://localhost:15672`

# ✅ Project Overview

This project is a demonstration application for an aesthetic clinic scheduling system, focused on service decoupling, asynchronous processing, and scalability.

The solution consists of a .NET backend API responsible for managing appointments and publishing messages for asynchronous processing by the notification system.

RabbitMQ is used as the message broker, allowing scheduling events to be sent to message queues asynchronously, avoiding direct coupling between the API and the notification service.

When an appointment is created or canceled, the API publishes a message to specific queues based on the context (Administrators and Customers). This separation provides greater flexibility for evolving business rules independently.

Messages are consumed by a Worker Service (Consumer), which processes events in the background and sends push notifications to users via Expo Push Notifications.

This approach improves performance, scalability, and resilience by ensuring that API operations are not blocked by secondary tasks. Additionally, RabbitMQ increases system reliability since messages remain in the queue until successfully processed, and allows horizontal scaling of consumers as demand grows.

The solution includes:

- REST API in .NET for managing core functionality
- RabbitMQ for messaging and asynchronous communication
- Worker Service (Background Consumer) for queue processing

The project was structured with a focus on separation of concerns, promoting low coupling between components and facilitating maintenance and future evolution of the system.

# 🏗️ Architecture

<img width="1536" height="1024" alt="Image" src="https://github.com/user-attachments/assets/fb4961d9-54a0-4b9b-a0fa-38f4ea3b9a41" />