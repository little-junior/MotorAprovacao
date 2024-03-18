<h1 align="center"> Motor de Aprovação </h1>
<br>

## Sobre o projeto
Este projeto é a conclusão do curso **DiverseDEV**, programa de estágio e educação promovido pelo Mercado Eletrônico em parceria com a Ada. A proposta principal dele é aplicar os conhecimentos adquiridos na trilha back-end em C#.
<br>

## O desafio 
Desenvolver uma API que permita criar documentos de reembolso e automatize o processo de aprovação com base em regras definidas.
<br>

## A solução
Ao automatizar o processo de aprovação com um motor de decisão baseado em regras predefinidas, o objetivo é agilizar o fluxo de trabalho, reduzindo a necessidade de intervenção manual e aumentando a eficiência do processo como um todo. Isso também ajuda a garantir consistência nas decisões de aprovação e reduz o risco de erros ou inconsistências.
<br>

## 🛠️ Descrição Sucinta da API
Na API Motor de Aprovação, os funcionários têm acesso a várias ferramentas úteis. Eles podem encontrar documentos de reembolso específicos por meio de um ID, ou visualizar documentos com base em seus status. Além disso, têm a capacidade de criar novos documentos de reembolso para despesas elegíveis. Os responsáveis podem atualizar o status dos documentos, marcando-os como aprovados após revisão ou como não aprovados se a despesa não for elegível. Essas funcionalidades proporcionam um processo eficiente e organizado para o gerenciamento de reembolsos. Para acesso a API os usuários devem possuir um autorização e funções que podem ser criadas dentro da API.
<br>


## 🔨 Características do projeto

- `Características 1` `API`: A aplicação utiliza documentação via Swagger e autorização via JWT, com middlewares para registro de ações. Além disso, oferece endpoints para Registro, Aprovação e Recusa de pedidos de reembolso, com tratamento de erros e validações integrados.
- `Características 2` `Implementação`: A implementação inclui o recebimento de documentos, o processamento desses documentos e a comparação com tabelas pré-definidas.
- `Características 3` `Banco de Dados`: No banco de dados, é utilizado o EntityFramework com PostgreSQL, incluindo migrations para manutenção da estrutura. Há quatro tabelas de entidades definidas, e a database é utilizada para autenticação.
- `Características 4` `Endpoints`: Há dois controllers, cada um com cinco endpoints. Um controller gerencia operações de documentos de reembolso, incluindo registrar, aprovar, rejeitar e obter documentos por status ou ID. O outro controller trata operações de autenticação, como login, criação ou alteração de cargo, registro de DTO e atualização do JWT.
- `Características 5` `Testes`: Os testes abrangem 75% dos branches e 81% das linhas nos modelos, utilizando as ferramentas xUnit, NSubstitute e FluentAssertions. 
<br>

## 📁 Endpoints
### RefundDocs
- **Encontrar documento por ID:** Insira o ID específico para localizar um documento de reembolso.
- **Encontrar por status:** Veja documentos por status: processamento, aprovados ou rejeitados.
- **Criar documento:** Preencha os detalhes da despesa para criar um novo documento de reembolso.
- **Atualizar status para aprovado:** Após revisão, mude o status para "aprovado" para iniciar o processo de reembolso.
- **Atualizar status para não aprovado:** Se necessário, altere o status para "não aprovado" com uma explicação adequada.

![RefundDocs](https://github.com/little-junior/MotorAprovacao/assets/89549170/8cc44232-4dec-4bb3-b148-6f44387d1e17)

### Auth
- **Autenticação de usuário:** Faça login no sistema utilizando suas credenciais de usuário para acessar as funcionalidades.
- **Criar usuário:** Crie novos usuário para atribuir permissões específicas dentro do sistema.
- **Papel do usuário:** Defina o papel de cada usuário atribuindo as funções criadas, garantindo acesso adequado às funcionalidades.
- **Registre-se DTO:** Registre novos usuários no sistema, fornecendo informações necessárias para criar um novo perfil de usuário.

![Auth](https://github.com/little-junior/MotorAprovacao/assets/89549170/32a47a1f-f742-47ad-a07c-537cefb4046c)

<br>

## ✔️ Técnicas e tecnologias utilizadas

- ``C #``
- ``ASP.NET Core``
- ``Entity Framework Core``
- ``Banco de Dados PostgreSQL``
- ``Paradigma de orientação a objetos``
<br>

![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge)
  
