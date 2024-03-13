using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var document = await _service.GetDocumentById(id);

            var documentResponseDto = new RefundDocumentResponseDto(document);

            return Ok(documentResponseDto);
        }

        [HttpGet()]
        public async Task<IActionResult> GetByStatus([FromQuery]Status status)
        {
            if (!Enum.IsDefined(typeof(Status), status))
            {
                return NotFound();
            }

            var documentsByStatus = await _service.GetDocumentsByStatus(status);

            //To do: criar service para a criação de dto, tirando o acoplamento
            //Talvez tirar isso daqui
            IEnumerable<RefundDocumentResponseDto> documentsResponseDtos = documentsByStatus
                .Select(index => new RefundDocumentResponseDto(index))
                .OrderBy(doc => doc.Total);

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

            var documentById = await _service.GetDocumentById(createdDocument.Id);

            var documentResponseDto = new RefundDocumentResponseDto(documentById);

            //To do: adicionar validação para returnar BadRequest em caso de entrada inválida
            return Created($"api/refunddocs/{documentResponseDto.Id}", documentResponseDto);
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
