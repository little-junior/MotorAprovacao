using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MotorAprovacao.Data.EF;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;
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
        [Fact]
        public async Task AprovarReembolso_ComRegraDeAprovação_AcimaDoLimiteDeValor_DeveAprovar()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "BancoTeste")
                .Options;
            var context = new AppDbContext(options);


            var approvalEngineMock = new ApprovalEngine(context);
            var repoMock = new RefundDocumentRepository(context);

            var documentService = new RefundDocumentService(approvalEngineMock, repoMock);
            var request = new RefundDocumentRequestDto
            {
                Total = 50m,
                CategoryId = 1,
                Description = "yay"
            };
            TestDataSeeder.SeedData(context);

            // Act
            var document = await documentService.CreateDocument(request);
            await approvalEngineMock.ProcessDocument(document);

            // Assert
            document.Status.Should().Be(Status.Approved);
        }


        [Fact]
        public async Task AprovarReembolso_ComRegraDeAprovação_AbaixoDoLimiteDeValor_DeveAprovar()
        {
            // Arrange

            // Act

            // Assert
        }


        [Fact]
        public async Task RecusarReembolso_ComRegraDeRecusa_AcimaDoLimiteDeValor_DeveRecusar()
        {
            // Arrange

            // Act

            // Assert
        }


        [Fact]
        public async Task RecusarReembolso_ComRegraDeRecusa_TodasAsCategorias_DeveRecusar()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task AprovarReembolso_SemRegrasEspecificas_DeveAprovar()
        {
            // Arrange

            // Act

            // Assert
        }


        [Fact]
        public async Task RecusarReembolso_ComRegraDeRecusa_Transporte_DeveRecusar()
        {
            // Arrange

            // Act

            // Assert
        }


        [Fact]
        public async Task CriarReembolso_ComDadosValidos_EstadoInicial_DeveSalvarNoBancoDeDados()
        {
            // Arrange

            // Act

            // Assert
        }


        [Fact]
        public async Task CriarReembolso_ComDadosInvalidos_DeveRetornarErro()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
