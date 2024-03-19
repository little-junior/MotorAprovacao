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

## Como usar a API MotorAprovacao
Bem-vindo ao guia de uso da API MotorAprovacao! Este documento fornecerá orientações sobre como interagir com os endpoints disponíveis nesta API.

#### 1. Autenticação
Para acessar os recursos protegidos pela API, você precisará autenticar-se e obter um token de acesso válido. A autenticação é realizada através do endpoint /api/auth/login, que requer um corpo de solicitação contendo as credenciais de login (nome de usuário e senha). Uma vez autenticado com sucesso, o token de acesso será retornado na resposta da solicitação.

#### 2. Endpoints Disponíveis
A API MotorAprovacao oferece os seguintes endpoints para manipulação de documentos de reembolso:
- GET /api/refunddocs/{id}: Retorna os detalhes de um documento de reembolso específico com base no ID fornecido.
- GET /api/refunddocs?status={status}: Retorna uma lista de documentos de reembolso filtrados pelo status fornecido.
- POST /api/refunddocs: Cria um novo documento de reembolso com base nos dados fornecidos no corpo da solicitação.
- PATCH /api/refunddocs/{id}/approve: Aprova o documento de reembolso com o ID fornecido.
- PATCH /api/refunddocs/{id}/disapprove: Rejeita o documento de reembolso com o ID fornecido.

#### 3. Como usar os Endpoints
Para utilizar os endpoints da API, siga estas etapas:
-  `Etapa 1` `Autenticação`: Primeiro, obtenha um token de acesso válido autenticando-se no endpoint de login. Guarde esse token para ser usado nas solicitações futuras.
- `Etapa 2` `Enviar Solicitações`: Utilize um cliente HTTP, como Postman ou cURL, para enviar solicitações para os endpoints desejados. Inclua o token de acesso recebido no cabeçalho Authorization nas  solicitações.
- `Etapa 3` `Processar Respostas`: Ao receber uma resposta do servidor, verifique o código de status HTTP para determinar se a solicitação foi bem-sucedida ou se houve algum erro. As respostas também podem incluir dados relevantes no corpo da resposta.

## Modelo de Dados

#### O modelo de dados do banco inclui quatro entidades principais:

1. `RefundDocument`: Esta entidade representa um documento de reembolso submetido pelos usuários. Ela contém informações como ID único, descrição do documento, total do reembolso, categoria à qual o documento está associado, status atual do documento e data de criação.

2. `Category`: Cada documento de reembolso está associado a uma categoria específica, como Alimentação, Transporte, Hospedagem, Viagem, entre outras. Essa entidade armazena o nome da categoria e seu ID único.

3. `CategoryRules`: Esta entidade define as regras específicas para cada categoria. Inclui valores máximos para aprovação e desaprovação de documentos de reembolso relacionados à categoria.

4. `ApplicationUser`: Essa entidade estende a funcionalidade padrão do Identity Framework para armazenar informações adicionais sobre os usuários da aplicação. Inclui dados como tokens de atualização para autenticação e tempo de expiração desses tokens.

 #### Relacionamentos

- Um documento de reembolso pertence a uma categoria específica, estabelecendo um relacionamento de muitos para um com a entidade Category.
- Cada categoria possui suas próprias regras definidas na entidade CategoryRules, estabelecendo um relacionamento um para um.
- As regras de categoria estão diretamente relacionadas com a categoria correspondente.

 #### Tecnologias Utilizadas

- O banco de dados é implementado utilizando o PostgreSQL, um sistema de gerenciamento de banco de dados relacional de código aberto amplamente utilizado.
- O acesso ao banco de dados é facilitado pelo Entity Framework Core, uma estrutura ORM (Object-Relational Mapping) que simplifica o trabalho com bancos de dados relacionais em aplicativos .NET.

 #### Considerações de Desempenho e Segurança

- O PostgreSQL oferece recursos avançados de desempenho, escalabilidade e segurança, tornando-o uma escolha sólida para aplicativos empresariais.
- O Entity Framework Core ajuda a mitigar preocupações de segurança, fornecendo suporte para consultas parametrizadas e prevenção de injeção de SQL.

### Preparando o Banco de Dados Localmente

1-`Instalação do PostgreSQL`:
Certifique-se de ter o PostgreSQL instalado localmente. Você pode baixá-lo e instalá-lo a partir do site oficial do PostgreSQL.

2-`Configuração da Conexão com o Banco de Dados`:
No arquivo appsettings.json ou appsettings.Development.json, configure a string de conexão do banco de dados para corresponder às suas configurações locais do PostgreSQL.

3-`Aplicação das Migrações`:
Abra o terminal no diretório raiz do projeto da API e execute o seguinte comando para aplicar as migrações e atualizar o banco de dados:"dotnet ef database update"

Isso criará a estrutura do banco de dados com base nas migrações fornecidas pelo Entity Framework Core.

Após seguir esses passos, o banco de dados estará configurado localmente e pronto para ser utilizado pela aplicação.

## ✔️ Técnicas e tecnologias utilizadas

- ``C #``
- ``ASP.NET Core``
- ``Entity Framework Core``
- ``Banco de Dados PostgreSQL``
- ``Paradigma de orientação a objetos``
- ``xUnit, NSubstitute e FluentAssertions``
- ``Serilog``
- ``JWTBearer``


![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge)
