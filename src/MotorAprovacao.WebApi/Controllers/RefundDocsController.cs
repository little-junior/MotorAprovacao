﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.ErrorHandlers;
using MotorAprovacao.WebApi.Filters;
using MotorAprovacao.WebApi.RequestDtos;
using MotorAprovacao.WebApi.ResponseDtos;
using MotorAprovacao.WebApi.Services;

namespace MotorAprovacao.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefundDocsController : ControllerBase
    {
        private readonly IRefundDocumentService _service;
        private readonly ICategoryRepository _categoryRepository;
        public RefundDocsController(IRefundDocumentService service, ICategoryRepository categoryRepository)
        {
            _service = service;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var document = await _service.GetDocumentById(id);

            if (document == null)
            {
                return NotFound(new ErrorResponse("404 - Not Found", "Document not found."));
            }

            var documentResponseDto = new RefundDocumentResponseDto(document);

            return Ok(documentResponseDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetByStatus([FromQuery]int status)
        {
            if (!Enum.IsDefined(typeof(Status), status))
            {
                return BadRequest(new ErrorResponse("400 - Bad Request", $"The value '{status}' of parameter 'status' is invalid"));
            }

            var documentsByStatus = await _service.GetDocumentsByStatus((Status)status);

            //To do: implementar escolha de ordenação entre total ou ordem de criação
            IEnumerable<RefundDocumentResponseDto> documentsResponseDtos = documentsByStatus
                .OrderBy(doc => doc.Total)
                .Select(index => new RefundDocumentResponseDto(index));

            return Ok(documentsResponseDtos);
        }

        [HttpPost]
        [TypeFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> PostRequestDoc([FromBody] RefundDocumentRequestDto documentDto)
        {
            var categoryExistence = await _categoryRepository.CheckExistenceById(documentDto.CategoryId);

            if (!categoryExistence)
            {
                return BadRequest(new ErrorResponse("400 - Bad Request", $"The value {documentDto.CategoryId} of field 'categoryId' is invalid."));
            }

            var createdDocument = await _service.CreateDocument(documentDto);

            var documentById = await _service.GetDocumentById(createdDocument.Id);

            var documentResponseDto = new RefundDocumentResponseDto(documentById);

            return Created($"api/refunddocs/{documentResponseDto.Id}", documentResponseDto);
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> PatchApprove([FromRoute] Guid id)
        {
            var document = await _service.GetDocumentById(id);

            if (document == null)
            {
                return NotFound(new ErrorResponse("404 - Not Found", "Document not found."));
            }

            if (document.Status != Status.OnApproval)
            {
                return Conflict(new ErrorResponse("409 - Conflict", "Document can only be approved while in the 'OnApproval' state."));
            }

            await _service.ApproveDocument(id);

            return NoContent();
        }

        [HttpPatch("{id}/disapprove")]
        public async Task<IActionResult> PatchDisapprove([FromRoute] Guid id)
        {
            var document = await _service.GetDocumentById(id);

            if (document == null)
            {
                return NotFound(new ErrorResponse("404 - Not Found", "Document not found."));
            }

            if (document.Status != Status.OnApproval)
            {
                return Conflict(new ErrorResponse("409 - Conflict", "Document can only be disapproved while in the 'OnApproval' state."));
            }

            await _service.DisapproveDocument(id);

            return NoContent();
        }
    }
}
