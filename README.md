<h1 align="center"> Motor de Aprovação </h1>

## Sobre o projeto
Este projeto é a conclusão do curso **DiverseDEV**, programa de estágio e educação promovido pelo Mercado Eletrônico em parceria com a Ada. A proposta principal dele é aplicar os conhecimentos adquiridos na trilha back-end em C#.

## O desafio 
Desenvolver uma API que permita criar documentos de reembolso e automatize o processo de aprovação com base em regras definidas.

## A solução
Ao automatizar o processo de aprovação com um motor de decisão baseado em regras predefinidas, o objetivo é agilizar o fluxo de trabalho, reduzindo a necessidade de intervenção manual e aumentando a eficiência do processo como um todo. Isso também ajuda a garantir consistência nas decisões de aprovação e reduz o risco de erros ou inconsistências.

## 🔨 Etapas do projeto

- `Etapa 1` `API`: A aplicação utiliza documentação via Swagger e autorização via JWT, com middlewares para registro de ações. Além disso, oferece endpoints para Registro, Aprovação e Recusa de pedidos de reembolso, com tratamento de erros e validações integrados.
- `Etapa 2` `Implementação`: A implementação inclui o recebimento de documentos, o processamento desses documentos e a comparação com tabelas pré-definidas.
- `Etapa 3` `Banco de Dados`: No banco de dados, é utilizado o EntityFramework com PostgreSQL, incluindo migrations para manutenção da estrutura. Há quatro tabelas de entidades definidas, e a database é utilizada para autenticação.
- `Etapa 4` `Endpoints`: Há dois controllers, cada um com cinco endpoints. Um controller gerencia operações de documentos de reembolso, incluindo registrar, aprovar, rejeitar e obter documentos por status ou ID. O outro controller trata operações de autenticação, como login, criação ou alteração de cargo, registro de DTO e atualização do JWT.
- `Etapa 5` `Testes`: Os testes abrangem 75% dos branches e 81% das linhas nos modelos, utilizando as ferramentas xUnit, NSubstitute e FluentAssertions. 


## ✔️ Técnicas e tecnologias utilizadas

- ``C #``
- ``ASP.NET Core``
- ``Entity Framework Core``
- ``Banco de Dados PostgreSQL``
- ``Paradigma de orientação a objetos``


![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge)
