﻿using Microsoft.AspNetCore.Mvc;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.RequestDtos;
using MotorAprovacao.WebApi.ResponseDtos;
using MotorAprovacao.WebApi.Services;

namespace MotorAprovacao.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundDocsController : ControllerBase
    {
        //To do: adicionar validações, verificações e tratar exceções
        private readonly IRefundDocumentService _service;

        public RefundDocsController(IRefundDocumentService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> GetByStatus([FromQuery]int status)
        {
            if (status != 0 || status != 1 || status != 2)
            {
                return NotFound();
            }

            var documentsByStatus = await _service.GetDocumentsByStatus((Status)status);

            //To do: criar service para a criação de dto, tirando o acoplamento
            IEnumerable<RefundDocumentResponseDto> documentsResponseDtos = documentsByStatus
                .Select(index => new RefundDocumentResponseDto(index))
                .OrderByDescending(doc => doc.Total);

            if (!documentsResponseDtos.Any()) 
            {
                return NoContent();
            }

            return Ok(documentsResponseDtos);
        }

        [HttpPost()]
        public async Task<IActionResult> PostRequestDoc([FromBody] RefundDocumentRequestDto documentDto)
        {
            var createdDocument = await _service.CreateDocument(documentDto);

            var documentResponseDto = new RefundDocumentResponseDto(createdDocument);

            //To do: adicionar validação para returnar BadRequest em caso de entrada inválida
            return Created($"api/refunddocs?status={createdDocument.Id}", documentResponseDto);
        }

        //To do: discutir sobre o método e a arquitetura da rota desses endpoints

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> PatchApprove([FromRoute] Guid id)
        {
            await _service.ApproveDocument(id);

            return NoContent();
        }

        [HttpPatch("{id}/disapprove")]
        public async Task<IActionResult> PatchDisapprove([FromRoute] Guid id)
        {
            await _service.DisapproveDocument(id);

            return NoContent();
        }
    }
}
