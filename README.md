[![CI](https://github.com/ThomasDixini/AplicativoEsteticaDemonstracao/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/ThomasDixini/AplicativoEsteticaDemonstracao/actions/workflows/dotnet-ci.yml)

🌍 **Languages:** [Português](README.md) | [English](README.en.md)

## 📑 Sumário

- [⚙️ Pré-requisitos](#️-pré-requisitos)
- [🐳 Configuração](#-configuração)
- [✅ Visão Geral do Projeto](#-visão-geral-do-projeto)
- [🏗️ Arquitetura do Projeto](#️-arquitetura-do-projeto)


  - [📱 Mobile App](#-mobile-app)
  - [🔔 Expo Notifications](#-expo-notifications)
  - [⚖️ NGINX – Load Balancer](#️-nginx--load-balancer)
  - [🧩 Backend (Arquitetura em Camadas)](#-backend-arquitetura-em-camadas)
  - [🔐 Autenticação – JWT (JSON Web Token)](#-autenticação--jwt-json-web-token)
  - [🧠 API Layer (Camada de API / Controllers)](#-api-layer-camada-de-api--controllers)
  - [⚙️ Service Layer (Camada de Serviços / Regras de Negócio)](#️-service-layer-camada-de-serviços--regras-de-negócio)
  - [🗄️ Data Access Layer (Camada de Acesso a Dados)](#️-data-access-layer-camada-de-acesso-a-dados)
  - [🗃️ Banco de Dados](#️-banco-de-dados)
- [🎥 Demonstração](#-demonstração)
  - [Screenshots](#screenshots)
- [🛠️ Configuração sem Docker](#️-configuração-sem-docker)
  - [Backend (.NET)](#backend-net)
  - [Mobile (React Native/Expo)](#mobile-react-nativeexpo)
- [🔐 Variáveis de Ambiente](#-variáveis-de-ambiente)

---


# ⚙️ Pré-requisitos

Para rodar esse projeto, você irá precisar somente do:
- **Docker e Docker Compose** (versão 3.8 ou superior)

# 🐳 Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/ThomasDixini/AplicativoEsteticaDemonstracao.git
   cd <pasta-do-projeto>
   ```

2. Execute os containers:
   ```bash
   docker compose up --build
   ```

3.  O backend estará disponível em `http://localhost:8080`
    RabbitMQ Server estará disponível em `http://localhost:15672`


# ✅ Visão Geral do Projeto

Este projeto é uma aplicação de demonstração de agendamento para clínica estética, com foco em desacoplamento entre serviços, processamento assíncrono e escalabilidade.

A solução é composta por uma API backend em .NET, responsável por gerenciar os agendamentos e publicar mensagens para o processamento assíncrono do sistema de notificações.

Para isso, foi utilizado o RabbitMQ como message broker, permitindo que eventos de agendamento sejam enviados para filas de mensagens de forma assíncrona, evitando acoplamento direto entre a API e o serviço de notificações.

Quando um agendamento é criado ou cancelado, a API publica uma mensagem em filas específicas de acordo com o contexto (Administradores e Clientes). Essa separação permite maior flexibilidade para evolução das regras de negócio de forma independente.

As mensagens são consumidas por um Worker Service (Consumer), responsável por processar os eventos em segundo plano e realizar o envio das notificações aos usuários utilizando o Expo Push Notifications.

Essa abordagem melhora a performance, escalabilidade e resiliência da aplicação, garantindo que operações da API não sejam bloqueadas por tarefas secundárias. Além disso, o uso do RabbitMQ aumenta a confiabilidade do sistema, pois as mensagens permanecem na fila até serem processadas com sucesso, e permite escalar horizontalmente os consumidores conforme a demanda.

A solução conta com:

API REST em .NET para gerenciamento das funcionalidades principais
RabbitMQ para mensageria e comunicação assíncrona
Worker Service (Background Consumer) para processamento das filas

O projeto foi estruturado com foco em separação de responsabilidades, promovendo baixo acoplamento entre os componentes e facilitando a manutenção e evolução do sistema.

# 🏗️ Arquitetura do Projeto

<img width="1536" height="1024" alt="Image" src="https://github.com/user-attachments/assets/fb4961d9-54a0-4b9b-a0fa-38f4ea3b9a41" />