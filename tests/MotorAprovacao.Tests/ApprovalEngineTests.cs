﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Entities.TheryDataClasses;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.RequestDtos;
using MotorAprovacao.WebApi.Services;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorAprovacao.Tests
{
    public class ApprovalEngineTests
    {
        private AppDbContext _context;
        private ApprovalEngine _approvalEngineMock;
        private RefundDocumentRepository _repoMock;
        private RefundDocumentService _documentService;

        /// <summary>
        /// Função que coloca todas as instancias necessárias para a testagem do código
        /// ajuda na diminuição do BoilePlate
        /// </summary>
        private void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _approvalEngineMock = new ApprovalEngine(_context);
            _repoMock = new RefundDocumentRepository(_context);
            _documentService = new RefundDocumentService(_approvalEngineMock, _repoMock);


            TestDataSeeder.SeedData(_context);
        }

        [Fact]
        public async Task AprovarReembolso_ComRegraDeAprovação_AcimaDoLimiteDeValor_DeveAprovar_Categoria2()
        {
            // Arrange
            Setup();

            var request = new RefundDocumentRequestDto
            {
                Total = 1200m,
                CategoryId = 2, // Usando CategoryId correto para a categoria 2
                Description = "Title"
            };

            // Act
            var document = await _documentService.CreateDocument(request);
            await _approvalEngineMock.ProcessDocument(document);

            // Assert
            document.Status.Should().Be(Status.Approved);
        }



        [Theory]
        [ClassData(typeof(RefundDocumentRequestDtoShouldAproveBelow))]
        public async Task AprovarReembolso_ComRegraDeAprovação_AbaixoDoLimiteDeValor_DeveAprovar(decimal total, int categoryId, string description)
        {
            // Arrange

            Setup();
            var request = new RefundDocumentRequestDto
            {
                Total = total,
                CategoryId = categoryId,
                Description = description
            };

            // Act
            var document = await _documentService.CreateDocument(request);
            await _approvalEngineMock.ProcessDocument(document);

            // Assert
            document.Status.Should().Be(Status.Approved);
        }


        [Theory]
        [ClassData(typeof(RefundDocumentRequestDtoShouldNotAprove))]
        public async Task RecusarReembolso_ComRegraDeRecusa_AcimaDoLimiteDeValor_DeveRecusar(decimal total, int categoryId, string description)
        {
            Setup();
            var request = new RefundDocumentRequestDto
            {
                Total = total,
                CategoryId = categoryId,
                Description = description
            };

            // Act
            var document = await _documentService.CreateDocument(request);
            await _approvalEngineMock.ProcessDocument(document);

            // Assert
            document.Status.Should().Be(Status.Disapproved);
        }


        [Fact]
        public async Task RecusarReembolso_ComRegraDeRecusa_TodasAsCategorias_DeveRecusar()
        {
            // Arrange
            Setup(); // Configuração do ambiente de teste

            var request = new RefundDocumentRequestDto
            {
                Total = 1200m, 
                CategoryId = 1,
                Description = "Title"
            };

            // Act
            var document = await _documentService.CreateDocument(request);

            // Assert
            document.Status.Should().Be(Status.Disapproved); 
        }



        [Fact]
        public async Task AprovarReembolso_SemRegrasEspecificas_DeveAprovar()
        {
            // Arrange
            Setup();
            var request = new RefundDocumentRequestDto
            {
                Total = 800m,
                CategoryId = 4,
                Description = "Title"
            };

            // Act
            var document = await _documentService.CreateDocument(request);
            await _approvalEngineMock.ProcessDocument(document);

            // Assert
            document.Status.Should().Be(Status.Approved);
        }



        [Fact]
        public async Task RecusarReembolso_ComRegraDeRecusa_Transporte_DeveRecusar()
        {
            // Arrange
            Setup();
            var request = new RefundDocumentRequestDto
            {
                Total = 1200m,
                CategoryId = 3,
                Description = "Title"
            };

            // Act
            var document = await _documentService.CreateDocument(request);
            await _approvalEngineMock.ProcessDocument(document);

            // Assert
            document.Status.Should().Be(Status.Disapproved);
        }


        [Fact]
        public async Task CriarReembolso_ComDadosValidos_EstadoInicial_DeveSalvarNoBancoDeDados()
        {
            // Arrange
            Setup();
            var request = new RefundDocumentRequestDto
            {
                Total = 200m,
                CategoryId = 1,
                Description = "Title"
            };

            // Act
            var document = await _documentService.CreateDocument(request);

            // Assert
            document.Should().NotBeNull();
            document.Status.Should().Be(Status.OnApproval);
            // aqui ficou faltando verificar se o id foi gerado pelo dt
        }

        [Fact]
        public async Task CriarReembolso_ComDadosInvalidos_DeveRetornarErro()
        {
            // Arrange
            Setup();
            var request = new RefundDocumentRequestDto
            {
                Total = -100m,
                CategoryId = 10,
                Description = "Title"
            };

            // Act
            Func<Task> createDocumentAction = async () => await _documentService.CreateDocument(request);

            // Assert
            await createDocumentAction.Should().ThrowAsync<Exception>();
        }

    }
}
