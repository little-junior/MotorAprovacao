using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;
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
    [Produces("application/json")]
    public class RefundDocsController : ControllerBase
    {
        private readonly IRefundDocumentService _service;
        private readonly ICategoryRepository _categoryRepository;

        public RefundDocsController(IRefundDocumentService service, ICategoryRepository categoryRepository)
        {
            _service = service;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Finds a specific Refund Document by Id
        /// </summary>
        /// <param name="id">Id of Refund Document to return</param>
        /// <returns>A Refund Document</returns>
        /// <response code="200">Returns the Refund Document</response>
        /// <response code="404">Refund Document not found</response>
        /// <response code="400">Id format is invalid</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RefundDocumentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModelStateResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Finds Refund Documents by Status
        /// </summary>
        /// <param name="status">Refund Document's Status
        /// <para>0 - OnApproval, 1 - Approved, 2 - Disapproved</para>
        /// </param>
        /// <returns>A list of Refund Documents</returns>
        /// <response code="200">Returns the list of Refund Documents</response>
        /// <response code="400">Status value is invalid</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<RefundDocumentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Creates a Refund Document
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/RefundDocs
        ///     {
        ///         "total": 200,
        ///         "categoryId": 2,
        ///         "description": "Document description"
        ///     }
        /// </remarks>
        /// <param name="documentDto">Refund Document request body
        /// <para>The JSON object representing some fields of the Refund Document</para>
        /// </param>
        /// <returns>A new Refund Document</returns>
        /// <response code="201">Returns the newly created Refund Document</response>
        /// <response code="400">At least one field in the Refund Document Request Body is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(RefundDocumentResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModelStateResponse), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates a Refund Document by changing its Status to Approved
        /// </summary>
        /// <param name="id">Id of Refund Document to update</param>
        /// <returns></returns>
        /// <response code="204">Refund Document's Status was updated</response>
        /// <response code="400">Id format is invalid</response>
        /// <response code="404">Refund Document not found</response>
        /// <response code="409">Refund Document could not be updated</response>
        [HttpPatch("{id}/approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModelStateResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType(typeof(ErrorResponse))]
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
        /// <summary>
        /// Updates a Refund Document by changing its Status to Disapproved
        /// </summary>
        /// <param name="id">Id of Refund Document to update</param>
        /// <returns></returns>
        /// <response code="204">Refund Document's Status was updated</response>
        /// <response code="400">Id format is invalid</response>
        /// <response code="404">Refund Document not found</response>
        /// <response code="409">Refund Document could not be updated</response>
        [HttpPatch("{id}/disapprove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModelStateResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
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
